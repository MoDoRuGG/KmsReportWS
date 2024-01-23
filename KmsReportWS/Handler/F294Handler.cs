using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using NLog;

namespace KmsReportWS.Handler
{
    public class F294Handler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public F294Handler() : base(ReportType.F294)
        {
        }
        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as Report294 ??
                         throw new Exception("Error saving new report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data {
                    Id_Flow = flow.Id, Id_Report = flow.Id_Report_Type, Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var f294DataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (f294DataList.Any())
                {
                    db.Report_f294.InsertAllOnSubmit(f294DataList);
                }

                db.SubmitChanges();
            }
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as Report294 ??
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
                foreach (var data in reportForms.Data)
                {
                    var f294 = db.Report_f294.SingleOrDefault(x => x.Id_Report_Data == idTheme
                                                                   && x.RowNum == data.RowNum);
                    if (f294 != null)
                    {
                        f294.RowNum = data.RowNum;
                        f294.CountPpl = data.CountPpl;
                        f294.CountSms = data.CountSms;
                        f294.CountPost = data.CountPost;
                        f294.CountPhone = data.CountPhone;
                        f294.CountMessangers = data.CountMessengers;
                        f294.CountEmail = data.CountEmail;
                        f294.CountAddress = data.CountAddress;
                        f294.CountAnother = data.CountAnother;
                        f294.CountOncologicalDisease = data.CountOncologicalDisease;
                        f294.CountEndocrineDisease = data.CountEndocrineDisease;
                        f294.CountBronchoDisease = data.CountBronchoDisease;
                        f294.CountBloodDisease = data.CountBloodDisease;
                        f294.CountAnotherDisease = data.CountAnotherDisease;
                    }
                    else
                    {
                        var f294Ins = MapThemeToPersist(idTheme, data);
                        db.Report_f294.InsertOnSubmit(f294Ins);
                    }
                }
            }

            db.SubmitChanges();
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new Report294 {ReportDataList = new List<Report294Dto>()};
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new Report294Dto {Theme = theme, Data = new List<Report294DataDto>()};

                var dataList = themeData.Report_f294.Select(MapThemeFromPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        private Report294DataDto MapThemeFromPersist(Report_f294 data) =>
            new Report294DataDto {
                RowNum = data.RowNum,
                CountPpl = data.CountPpl ?? 0,
                CountSms = data.CountSms ?? 0,
                CountPost = data.CountPost ?? 0,
                CountPhone = data.CountPhone ?? 0,
                CountMessengers = data.CountMessangers ?? 0,
                CountEmail = data.CountEmail ?? 0,
                CountAddress = data.CountAddress ?? 0,
                CountAnother = data.CountAnother ?? 0,
                CountOncologicalDisease = data.CountOncologicalDisease ?? 0,
                CountEndocrineDisease = data.CountEndocrineDisease ?? 0,
                CountBronchoDisease = data.CountBronchoDisease ?? 0,
                CountBloodDisease = data.CountBloodDisease ?? 0,
                CountAnotherDisease = data.CountAnotherDisease ?? 0
            };

        private Report_f294 MapThemeToPersist(int idThemeData, Report294DataDto data) =>
            new Report_f294 {
                Id_Report_Data = idThemeData,
                CountAddress = data.CountAddress,
                CountAnother = data.CountAnother,
                CountAnotherDisease = data.CountAnotherDisease,
                CountBloodDisease = data.CountBloodDisease,
                CountBronchoDisease = data.CountBronchoDisease,
                CountEndocrineDisease = data.CountEndocrineDisease,
                CountOncologicalDisease = data.CountOncologicalDisease,
                CountEmail = data.CountEmail,
                CountMessangers = data.CountMessengers,
                CountPhone = data.CountPhone,
                CountPost = data.CountPost,
                CountPpl = data.CountPpl,
                CountSms = data.CountSms,
                RowNum = data.RowNum
            };
    }
}