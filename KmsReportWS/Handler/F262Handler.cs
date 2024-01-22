using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using NLog;

namespace KmsReportWS.Handler
{
    public class F262Handler : BaseReportHandler
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public F262Handler() : base(ReportType.F262)
        {
        }
        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as Report262 ??
                         throw new Exception("Error saving new report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var themeData = new Report_Data {
                    Id_Flow = flow.Id, Id_Report = flow.Id_Report_Type, Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                var f262DataList = reportForms.Data.Select(data => MapMainThemeToPersist(themeData.Id, data)).ToList();
                if (f262DataList.Any())
                {
                    db.Report_f262.InsertAllOnSubmit(f262DataList);
                }

                var table3List = reportForms.Table3.Select(data => MapTable3ThemeToPersist(themeData.Id, data))
                    .ToList();
                if (table3List.Any())
                {
                    db.Report262_Table3.InsertAllOnSubmit(table3List);
                }

                db.SubmitChanges();
            }
        }

        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as Report262 ??
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
                    var f262 = db.Report_f262
                        .SingleOrDefault(x => x.Id_Report_Data == idTheme && x.Row_Num == data.RowNum);
                    if (f262 != null)
                    {
                        f262.Count_Sms = data.CountSms;
                        f262.Count_Ppl = data.CountPpl;
                        f262.Count_Post = data.CountPost;
                        f262.Count_Phone = data.CountPhone;
                        f262.Count_Messangers = data.CountMessengers;
                        f262.Count_Address = data.CountAddress;
                        f262.Count_Email = data.CountEmail;
                        f262.Count_Another = data.CountAnother;
                        f262.Count_Ppl_Full = data.CountPplFull;
                    }
                    else
                    {
                        var f262Ins = MapMainThemeToPersist(idTheme, data);
                        db.Report_f262.InsertOnSubmit(f262Ins);
                    }
                }

                db.SubmitChanges();

                var dataForDelete = db.Report262_Table3.Where(x => x.Id_Report_Data == idTheme);
                db.Report262_Table3.DeleteAllOnSubmit(dataForDelete);
                db.SubmitChanges();

                var table3List = reportForms.Table3.Select(data => MapTable3ThemeToPersist(idTheme, data)).ToList();
                if (table3List.Any())
                {
                    db.Report262_Table3.InsertAllOnSubmit(table3List);
                }

                db.SubmitChanges();
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new Report262 {ReportDataList = new List<Report262Dto>()};
            MapFromReportFlow(rep, outReport);

            foreach (var themeData in rep.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new Report262Dto {
                    Theme = theme, Data = new List<Report262DataDto>(), Table3 = new List<Report262Table3Data>()
                };
                if (theme == "Таблица 3")
                {
                    var table3DataList = themeData.Report262_Table3.Select(MapTable3FromPersist);
                    dto.Table3.AddRange(table3DataList);
                }
                else
                {
                    var dataList = themeData.Report_f262.Select(MapMainThemeFromPersist);
                    dto.Data.AddRange(dataList);
                }

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        private Report262Table3Data MapTable3FromPersist(Report262_Table3 table3) =>
            new Report262Table3Data {
                CountChannelAnother = table3.count_channel_another ?? 0,
                CountChannelAnotherChild = table3.count_channel_another_child ?? 0,
                CountChannelPhone = table3.count_channel_phone ?? 0,
                CountChannelPhoneChild = table3.count_channel_phone_child ?? 0,
                CountChannelSp = table3.count_channel_sp ?? 0,
                CountChannelSpChild = table3.count_channel_sp_child ?? 0,
                CountChannelTerminal = table3.count_channel_terminal ?? 0,
                CountChannelTerminalChild = table3.count_channel_terminal_child ?? 0,
                CountUnit = table3.count_unit ?? 0,
                CountUnitChild = table3.count_unit_child ?? 0,
                CountUnitWithSp = table3.count_unit_with_sp ?? 0,
                CountUnitWithSpChild = table3.count_unit_with_sp_child ?? 0,
                Mo = table3.mo
            };

        private Report262DataDto MapMainThemeFromPersist(Report_f262 data) =>
            new Report262DataDto {
                CountAddress = data.Count_Address ?? 0,
                CountAnother = data.Count_Another ?? 0,
                CountEmail = data.Count_Email ?? 0,
                CountMessengers = data.Count_Messangers ?? 0,
                CountPhone = data.Count_Phone ?? 0,
                CountPost = data.Count_Post ?? 0,
                CountPpl = data.Count_Ppl ?? 0,
                CountPplFull = data.Count_Ppl_Full ?? 0,
                CountSms = data.Count_Sms ?? 0,
                RowNum = data.Row_Num ?? 0
            };

        private Report262_Table3 MapTable3ThemeToPersist(int idThemeData, Report262Table3Data data) =>
            new Report262_Table3 {
                count_channel_another = data.CountChannelAnother,
                count_channel_another_child = data.CountChannelAnotherChild,
                count_channel_phone = data.CountChannelPhone,
                count_channel_phone_child = data.CountChannelPhoneChild,
                count_channel_sp = data.CountChannelSp,
                count_channel_sp_child = data.CountChannelSpChild,
                count_channel_terminal = data.CountChannelTerminal,
                count_channel_terminal_child = data.CountChannelTerminalChild,
                count_unit = data.CountUnit,
                count_unit_child = data.CountUnitChild,
                count_unit_with_sp = data.CountUnitWithSp,
                count_unit_with_sp_child = data.CountUnitWithSpChild,
                mo = data.Mo,
                Id_Report_Data = idThemeData
            };

        private Report_f262 MapMainThemeToPersist(int idthemeData, Report262DataDto data) =>
            new Report_f262 {
                Id_Report_Data = idthemeData,
                Row_Num = data.RowNum,
                Count_Sms = data.CountSms,
                Count_Ppl = data.CountPpl,
                Count_Post = data.CountPost,
                Count_Phone = data.CountPhone,
                Count_Messangers = data.CountMessengers,
                Count_Address = data.CountAddress,
                Count_Email = data.CountEmail,
                Count_Another = data.CountAnother,
                Count_Ppl_Full = data.CountPplFull
            };
    }
}