using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Handler
{
    public class IizlHandler : BaseReportHandler
    {
        public IizlHandler(ReportType reportType) : base(reportType)
        {
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as ReportIizl ??
                         throw new Exception("Error saving new report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data {
                    Id_Flow = flow.Id,
                    Id_Report = flow.Id_Report_Type,
                    Theme = reportForms.Theme,
                    General_field_1 = reportForms.TotalPersFirst,
                    General_field_2 = reportForms.TotalPersRepeat
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var iizlDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (iizlDataList.Any())
                {
                    db.Report_Iilzs.InsertAllOnSubmit(iizlDataList);
                }

                db.SubmitChanges();
            }
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportIizl ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var theme =
                    db.Report_Data.SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme);
                if (theme != null)
                {
                    var dataReport = db.Report_Iilzs.Where(x => x.Id_Report_Data == theme.Id);
                    db.Report_Iilzs.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    theme.General_field_1 = reportForms.TotalPersFirst;
                    theme.General_field_2 = reportForms.TotalPersRepeat;

                    var dataList = reportForms.Data.Select(data => MapThemeToPersist(theme.Id, data)).ToList();
                    if (dataList.Any())
                    {
                        db.Report_Iilzs.InsertAllOnSubmit(dataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportIizl {ReportDataList = new List<ReportIizlDto>()};
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportIizlDto {
                    Theme = theme,
                    Data = new List<ReportIizlDataDto>(),
                    TotalPersFirst = Convert.ToInt32(themeData.General_field_1 ?? 0),
                    TotalPersRepeat = Convert.ToInt32(themeData.General_field_2 ?? 0)
                };

                var dataList = themeData.Report_Iilz.Select(MapThemeFromPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        private ReportIizlDataDto MapThemeFromPersist(Report_Iilz data) =>
            new ReportIizlDataDto {
                AccountingDocument = data.Accounting_Document,
                Code = data.Code,
                CountMessages = data.Count_Messages ?? 0,
                CountPersFirst = data.Count_Pers_First ?? 0,
                CountPersRepeat = data.Count_Pers_Repeat ?? 0,
                TotalCost = data.Total_Cost ?? 0,
                AverageCostOfInforming1PL = data.average_cost_of_informing_1_PL ?? 0,
                AverageCostPerMessage = data.average_cost_per_message ?? 0
            };

        private Report_Iilz MapThemeToPersist(int idThemeData, ReportIizlDataDto data) =>
            new Report_Iilz {
                Id_Report_Data = idThemeData,
                Total_Cost = data.TotalCost,
                Count_Pers_First = data.CountPersFirst,
                Count_Pers_Repeat = data.CountPersRepeat,
                Accounting_Document = data.AccountingDocument,
                Code = data.Code,
                Count_Messages = data.CountMessages,
                average_cost_of_informing_1_PL = data.AverageCostOfInforming1PL,
                average_cost_per_message = data.AverageCostPerMessage
            };
    }
}