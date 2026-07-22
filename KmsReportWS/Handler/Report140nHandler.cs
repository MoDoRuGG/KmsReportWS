using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using NLog;

namespace KmsReportWS.Handler
{
    public class Report140nHandler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;

        public Report140nHandler() : base(ReportType.R140n)
        {
        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }

        public Report140nDataDto GetYearData(string yymm, string theme, string fillial)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            // 1. Получаем Id_Report_Data для "Таблица 10"
            var reportDataIds = (from rd in db.Report_Data
                                 join rf in db.Report_Flow on rd.Id_Flow equals rf.Id
                                 where rf.Id_Region == fillial
                                    && rd.Theme == "Таблица 10"
                                    && rf.Yymm == yymm
                                 select rd.Id).ToList();

            // 2. Если данных нет, возвращаем DTO с нулями
            if (!reportDataIds.Any())
            {
                return new Report140nDataDto { Iidvn = 0, Iidn = 0 };
            }

            // 3. Считаем суммы из Report_Zpz2025 по найденным ID
            var zpz10result = db.Report_Zpz2025
                .Where(x => reportDataIds.Contains(x.Id_Report_Data))
                .GroupBy(x => 1)
                .Select(g => new
                {
                    p3 = g.Sum(x => (x.RowNum == "2.1" || x.RowNum == "2.2" || x.RowNum == "2.3")
                                    ? (x.CountSmoAnother ?? 0m) : 0m),
                    p4 = g.Sum(x => x.RowNum == "1.4"
                                    ? (x.CountSmoAnother ?? 0m) : 0m)
                })
                .FirstOrDefault() ?? new { p3 = 0m, p4 = 0m };

            return new Report140nDataDto
            {
                Iidvn = zpz10result.p3,
                Iidn = zpz10result.p4
            };
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as Report140n ??
                      throw new Exception("Error saving new report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data
                {

                    Id_Flow = flow.Id,
                    Id_Report = flow.Id_Report_Type,
                    Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var fList = MapMainThemeFromPersist(themeData.Id, reportForms.Data);
                if (fList != null)
                {
                    db.Report_140n.InsertOnSubmit(fList);
                }

                db.SubmitChanges();
            }
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as Report140n ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id ?? 0;
                if (idTheme == 0)
                {
                    Log.Error(
                        $"Error getting data. idTheme = 0; IdFlow = {inReport.IdFlow}, Theme = {reportForms.Theme}");
                    continue;
                }


                var row = db.Report_140n
                       .SingleOrDefault(x => x.Id_Report_Data == idTheme);

                if (report != null)
                {
                    row.CZLdost = reportForms.Data.CZLdost;
                    row.CZLsmo = reportForms.Data.CZLsmo;
                    row.KSErez = reportForms.Data.KSErez;
                    row.KSE = reportForms.Data.KSE;
                    row.PPMinadvn = reportForms.Data.PPMinadvn;
                    row.Iidvn = reportForms.Data.Iidvn;
                    row.PPMinfdn = reportForms.Data.PPMinfdn;
                    row.Iidn = reportForms.Data.Iidn;
                    row.KOJdosud = reportForms.Data.KOJdosud;
                    row.KOJsud = reportForms.Data.KOJsud;
                    row.KOJzl = reportForms.Data.KOJzl;
                    row.KOJzlsmo = reportForms.Data.KOJzlsmo;
                    row.KZAsobl = reportForms.Data.KZAsobl;
                    row.KZAvsego = reportForms.Data.KZAvsego;
                    row.DT = reportForms.Data.DT;
                    row.Scpo = reportForms.Data.Scpo;
                    row.KEKMPpodtv = reportForms.Data.KEKMPpodtv;
                    row.KEKMPtfoms = reportForms.Data.KEKMPtfoms;
                    row.KZSMOpodtv = reportForms.Data.KZSMOpodtv;
                    row.KPMOtfoms = reportForms.Data.KPMOtfoms;

                }
                else
                {
                    var rep = MapMainThemeFromPersist(idTheme, reportForms.Data);
                    db.Report_140n.InsertOnSubmit(rep);
                }

                db.SubmitChanges();

            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new Report140n { ReportDataList = new List<Report140nDto>() };
            MapFromReportFlow(rep, outReport);

            // !!! ГЛАВНОЕ ИСПРАВЛЕНИЕ: Получаем данные из ЗПЗ один раз для всего отчёта
            var zpzData = GetYearData(rep.Yymm, "Таблица 10", rep.Id_Region);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new Report140nDto
                {
                    Theme = theme,
                    Data = new Report140nDataDto(),
                };

                var dataList = themeData.Report_140n.Select(MapReportDto).ToList();
                if (dataList.Any())
                {
                    dto.Data = dataList.First();

                    // !!! Перезаписываем значения из ЗПЗ
                    dto.Data.Iidvn = zpzData.Iidvn;
                    dto.Data.Iidn = zpzData.Iidn;
                }

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        private Report_140n MapMainThemeFromPersist(int idThemeData, Report140nDataDto data)
        {
            if (data != null)
            {
                return new Report_140n
                {
                    Id = data.Id,
                    CZLdost = data.CZLdost,
                    CZLsmo = data.CZLsmo,
                    KSErez = data.KSErez,
                    KSE = data.KSE,
                    PPMinadvn = data.PPMinadvn,
                    Iidvn = data.Iidvn,
                    PPMinfdn = data.PPMinfdn,
                    Iidn = data.Iidn,
                    KOJdosud = data.KOJdosud,
                    KOJsud = data.KOJsud,
                    KOJzl = data.KOJzl,
                    KOJzlsmo = data.KOJzlsmo,
                    KZAsobl = data.KZAsobl,
                    KZAvsego = data.KZAvsego,
                    DT = data.DT,
                    Scpo = data.Scpo,
                    KEKMPpodtv = data.KEKMPpodtv,
                    KEKMPtfoms = data.KEKMPtfoms,
                    KZSMOpodtv = data.KZSMOpodtv,
                    KPMOtfoms = data.KPMOtfoms,

                    Id_Report_Data = idThemeData
                };
            }


            return new Report_140n
            {
                Id = 0,
                CZLdost = 0,
                CZLsmo = 0,
                KSErez = 0,
                KSE = 0,
                PPMinadvn = 0,
                Iidvn = 0,
                PPMinfdn = 0,
                Iidn = 0,
                KOJdosud = 0,
                KOJsud = 0,
                KOJzl = 0,
                KOJzlsmo = 0,
                KZAsobl = 0,
                KZAvsego = 0,
                DT = 0,
                Scpo = 0,
                KEKMPpodtv = 0,
                KEKMPtfoms = 0,
                KZSMOpodtv = 0,
                KPMOtfoms = 0,
                Id_Report_Data = idThemeData
            };
        }

        private Report140nDataDto MapReportDto(Report_140n report) =>

            new Report140nDataDto
            {
                Id = report.Id,
                CZLdost = report.CZLdost ?? 0,
                CZLsmo = report.CZLsmo ?? 0,
                KSErez = report.KSErez ?? 0,
                KSE = report.KSE ?? 0,
                PPMinadvn = report.PPMinadvn ?? 0,
                Iidvn = report.Iidvn ?? 0,
                PPMinfdn = report.PPMinfdn ?? 0,
                Iidn = report.Iidn ?? 0,
                KOJdosud = report.KOJdosud ?? 0,
                KOJsud = report.KOJsud ?? 0,
                KOJzl = report.KOJzl ?? 0,
                KOJzlsmo = report.KOJzlsmo ?? 0,
                KZAsobl = report.KZAsobl ?? 0,
                KZAvsego = report.KZAvsego ?? 0,
                DT = report.DT ?? 0,
                Scpo = report.Scpo ?? 0,
                KEKMPpodtv = report.KEKMPpodtv ?? 0,
                KEKMPtfoms = report.KEKMPtfoms ?? 0,
                KZSMOpodtv = report.KZSMOpodtv ?? 0,
                KPMOtfoms = report.KPMOtfoms ?? 0,
            };
    }
}