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

            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_140n
                .Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
                            && x.Report_Data.Theme == theme
                            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
                            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm))
                .GroupBy(x => x.Report_Data.Theme)
                .Select(g => new Report140nDataDto
                {
                    CZLdost = g.Sum(x => x.CZLdost ?? 0),
                    CZLsmo = g.Sum(x => x.CZLsmo ?? 0),
                    KSErez = g.Sum(x => x.KSErez ?? 0),
                    KSE = g.Sum(x => x.KSE ?? 0),
                    PPMinadvn = g.Sum(x => x.PPMinadvn ?? 0),
                    Iidvn = g.Sum(x => x.Iidvn ?? 0),
                    PPMinfdn = g.Sum(x => x.PPMinfdn ?? 0),
                    Iidn = g.Sum(x => x.Iidn ?? 0),
                    KOJdosud = g.Sum(x => x.KOJdosud ?? 0),
                    KOJsud = g.Sum(x => x.KOJsud ?? 0),
                    KOJzl = g.Sum(x => x.KOJzl ?? 0),
                    KOJzlsmo = g.Sum(x => x.KOJzlsmo ?? 0),
                    KZAsobl = g.Sum(x => x.KZAsobl ?? 0),
                    KZAvsego = g.Sum(x => x.KZAvsego ?? 0),
                    DT = g.Sum(x => x.DT ?? 0),
                    Scpo = g.Sum(x => x.Scpo ?? 0),
                    KEKMPpodtv = g.Sum(x => x.KEKMPpodtv ?? 0),
                    KEKMPtfoms = g.Sum(x => x.KEKMPtfoms ?? 0),
                    KZSMOpodtv = g.Sum(x => x.KZSMOpodtv ?? 0),
                    KPMOtfoms = g.Sum(x => x.KPMOtfoms ?? 0)
                })
                .FirstOrDefault();

            return result;
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

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();

                var dto = new Report140nDto
                {
                    Theme = theme,
                    Data = new Report140nDataDto(),

                };


                var dataList = themeData.Report_140n.Select(MapReportDto);
                dto.Data = dataList.First();

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