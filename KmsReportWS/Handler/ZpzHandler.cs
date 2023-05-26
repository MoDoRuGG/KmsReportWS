using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;

namespace KmsReportWS.Handler
{
    public class ZpzHandler : BaseReportHandler
    {
        private readonly string _connStr = Settings.Default.ConnStr;
        public ZpzHandler(ReportType reportType) : base(reportType)
        {
        }

        public ReportZpzDataDto GetYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_Zpz.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "Zpz10"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportZpzDataDto
            {  
            CountSmo = (decimal)x.Sum(g => g.CountSmo)

            }).FirstOrDefault();

            return result;

        }

        public ReportZpzDataDto GetLethalYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_Zpz.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "ZpzLethal"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportZpzDataDto
            {
                CountSmo = (decimal)x.Sum(g => g.CountSmo)

            }).FirstOrDefault();

            return result;

        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as ReportZpz ??
                         throw new Exception("Error saving new report, because getting empty report");
            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data {
                    Id_Flow = flow.Id, Id_Report = flow.Id_Report_Type, Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var zpzDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (zpzDataList.Any())
                {
                    db.Report_Zpz.InsertAllOnSubmit(zpzDataList);
                }
                db.SubmitChanges();
            }
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportZpz ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id;
                if (idTheme != null)
                {
                    var dataReport = db.Report_Zpz.Where(x => x.Id_Report_Data == idTheme);
                    db.Report_Zpz.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    var zpzDataList = reportForms.Data.Select(data => MapThemeToPersist(idTheme.Value, data)).ToList();
                    if (zpzDataList.Any())
                    {
                        db.Report_Zpz.InsertAllOnSubmit(zpzDataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep_flow)
        {
            var outReport = new ReportZpz {ReportDataList = new List<ReportZpzDto>()};
            MapFromReportFlow(rep_flow, outReport);

            foreach (var themeData in rep_flow.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportZpzDto {Theme = theme, Data = new List<ReportZpzDataDto>()};

                var dataList = themeData.Report_Zpz.Select(MapThemeToPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        private ReportZpzDataDto MapThemeToPersist(Report_Zpz data) =>
            new ReportZpzDataDto {
                Code = data.RowNum,
                CountSmo = data.CountSmo ?? 0,
                CountSmoAnother = data.CountSmoAnother ?? 0,
                CountAssignment = data.CountAssignment ?? 0,
                CountInsured = data.CountInsured ?? 0,
                CountInsuredRepresentative = data.CountInsuredRepresentative ?? 0,
                CountTfoms = data.CountTfoms ?? 0,
                CountProsecutor = data.CountProsecutor ?? 0,
                CountOutOfSmo = data.CountOutOfSmo ?? 0,
                CountAmbulatory = data.CountAmbulatory ?? 0,
                CountDs = data.CountDs ?? 0,
                CountDsVmp = data.CountDsVmp ?? 0,
                CountStac = data.CountStac ?? 0,
                CountStacVmp = data.CountStacVmp ?? 0,
                CountOutOfSmoAnother = data.CountOutOfSmoAnother ?? 0,
                CountAmbulatoryAnother = data.CountAmbulatoryAnother ?? 0,
                CountDsAnother = data.CountDsAnother ?? 0,
                CountDsVmpAnother = data.CountDsVmpAnother ?? 0,
                CountStacAnother = data.CountStacAnother ?? 0,
                CountStacVmpAnother = data.CountStacVmpAnother ?? 0
            };

        private Report_Zpz MapThemeToPersist(int idThemeData, ReportZpzDataDto data) =>
            new Report_Zpz {
                Id_Report_Data = idThemeData,
                RowNum = data.Code,
                CountSmo = data.CountSmo,
                CountSmoAnother = data.CountSmoAnother,
                CountAssignment = data.CountAssignment,
                CountInsured = data.CountInsured,
                CountInsuredRepresentative = data.CountInsuredRepresentative,
                CountTfoms = data.CountTfoms,
                CountProsecutor = data.CountProsecutor,
                CountOutOfSmo = data.CountOutOfSmo,
                CountAmbulatory = data.CountAmbulatory,
                CountDs = data.CountDs,
                CountDsVmp = data.CountDsVmp,
                CountStac = data.CountStac,
                CountStacVmp = data.CountStacVmp,
                CountOutOfSmoAnother = data.CountOutOfSmoAnother,
                CountAmbulatoryAnother = data.CountAmbulatoryAnother,
                CountDsAnother = data.CountDsAnother,
                CountDsVmpAnother = data.CountDsVmpAnother,
                CountStacAnother = data.CountStacAnother,
                CountStacVmpAnother = data.CountStacVmpAnother
            };
    }
}