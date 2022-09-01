using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Handler
{
    public class PgHandler : BaseReportHandler
    {
        public PgHandler(ReportType reportType) : base(reportType)
        {
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as ReportPg ??
                         throw new Exception("Error saving new report, because getting empty report");
            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data {
                    Id_Flow = flow.Id, Id_Report = flow.Id_Report_Type, Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var pgDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (pgDataList.Any())
                {
                    db.Report_Pg.InsertAllOnSubmit(pgDataList);
                }
                db.SubmitChanges();
            }
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportPg ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id;
                if (idTheme != null)
                {
                    var dataReport = db.Report_Pg.Where(x => x.Id_Report_Data == idTheme);
                    db.Report_Pg.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    var pgDataList = reportForms.Data.Select(data => MapThemeToPersist(idTheme.Value, data)).ToList();
                    if (pgDataList.Any())
                    {
                        db.Report_Pg.InsertAllOnSubmit(pgDataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep_flow)
        {
            var outReport = new ReportPg {ReportDataList = new List<ReportPgDto>()};
            MapFromReportFlow(rep_flow, outReport);

            foreach (var themeData in rep_flow.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportPgDto {Theme = theme, Data = new List<ReportPgDataDto>()};

                var dataList = themeData.Report_Pg.Select(MapThemeToPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        private ReportPgDataDto MapThemeToPersist(Report_Pg data) =>
            new ReportPgDataDto {
                Code = data.RowNum,
                CountSmo = data.CountSmo ?? 0,
                CountSmoAnother = data.CountSmoAnother ?? 0,
                CountInsured = data.CountInsured ?? 0,
                CountInsuredRepresentative = data.CountInsuredRepresentative ?? 0,
                CountTfoms = data.CountTfoms ?? 0,
                CountProsecutor = data.CountProsecutor ?? 0,
                CountOutOfSmo = data.CountOutOfSmo ?? 0,
                CountAmbulatory = data.CountAmbulatory ?? 0,
                CountDs = data.CountDs ?? 0,
                CountDsVmp = data.CountDsVmp ?? 0,
                CountStac = data.CountStac,
                CountStacVmp = data.CountStacVmp ?? 0,
                CountOutOfSmoAnother = data.CountOutOfSmoAnother ?? 0,
                CountAmbulatoryAnother = data.CountAmbulatoryAnother ?? 0,
                CountDsAnother = data.CountDsAnother ?? 0,
                CountDsVmpAnother = data.CountDsVmpAnother ?? 0,
                CountStacAnother = data.CountStacAnother ?? 0,
                CountStacVmpAnother = data.CountStacVmpAnother ?? 0
            };

        private Report_Pg MapThemeToPersist(int idThemeData, ReportPgDataDto data) =>
            new Report_Pg {
                Id_Report_Data = idThemeData,
                RowNum = data.Code,
                CountSmo = data.CountSmo,
                CountSmoAnother = data.CountSmoAnother,
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