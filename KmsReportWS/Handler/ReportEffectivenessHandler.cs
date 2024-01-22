using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using NLog;

namespace KmsReportWS.Handler
{
    public class ReportEffectivenessHandler : BaseReportHandler
    {

        private readonly string _connStr = Settings.Default.ConnStr;

        public ReportEffectivenessHandler(ReportType reportType) : base(reportType)
        {
        }

        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }
        public ReportEffectivenessDataDto GetYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_Effectiveness.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "Effective"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportEffectivenessDataDto
            {
                full_name = (string)x.SelectMany(g => g.full_name),
                expertise_type = (string)x.SelectMany(g => g.expertise_type),
                expert_speciality = (string)x.SelectMany(g => g.expert_speciality),

            }).FirstOrDefault();

            return result;

        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as ReportEffectiveness ??
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

                var effectivenessDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (effectivenessDataList.Any())
                {
                    db.Report_Effectiveness.InsertAllOnSubmit(effectivenessDataList);
                }
                db.SubmitChanges();
            }
        }


        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportEffectiveness ??
                         throw new Exception("Error update report, because getting empty report");

            foreach (var reportForms in report.ReportDataList)
            {
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id;
                if (idTheme != null)
                {
                    var dataReport = db.Report_Effectiveness.Where(x => x.Id_Report_Data == idTheme);
                    db.Report_Effectiveness.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    var effectivenessDataList = reportForms.Data.Select(data => MapThemeToPersist(idTheme.Value, data)).ToList();
                    if (effectivenessDataList.Any())
                    {
                        db.Report_Effectiveness.InsertAllOnSubmit(effectivenessDataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep_flow)
        {
            var outReport = new ReportEffectiveness { ReportDataList = new List<ReportEffectivenessDto>() };
            MapFromReportFlow(rep_flow, outReport);

            foreach (var themeData in rep_flow.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportEffectivenessDto { Theme = theme, Data = new List<ReportEffectivenessDataDto>() };

                var dataList = themeData.Report_Effectiveness.Select(MapThemeToPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        private ReportEffectivenessDataDto MapThemeToPersist(Report_Effectiveness data) =>
            new ReportEffectivenessDataDto
            {
                    CodeRowNum = data.RowNum,
                    full_name = data.full_name,
                    expert_busyness = data.expert_busyness ?? 0,
                    expert_speciality = data.expert_speciality,
                    expertise_type = data.expertise_type,
                    mee_quantity_plan = data.mee_quantity_plan ?? 0,
                    mee_quantity_fact = data.mee_quantity_fact ?? 0,
                    mee_quantity_percent = data.mee_quantity_percent ?? 0,
                    mee_yeild_plan = data.mee_yeild_plan ?? 0,
                    mee_yeild_fact = data.mee_yeild_fact ?? 0,
                    mee_yeild_percent = data.mee_yeild_percent ?? 0,
                    ekmp_quantity_plan = data.ekmp_quantity_plan ?? 0,
                    ekmp_quantity_fact = data.ekmp_quantity_fact ?? 0,
                    ekmp_quantity_percent = data.ekmp_quantity_percent ?? 0,
                    ekmp_yeild_plan = data.ekmp_yeild_plan ?? 0,
                    ekmp_yeild_fact = data.ekmp_yeild_fact ?? 0,
                    ekmp_yeild_percent = data.ekmp_yeild_percent ?? 0,
            };

        private Report_Effectiveness MapThemeToPersist(int idThemeData, ReportEffectivenessDataDto data) =>
            new Report_Effectiveness
            {
                Id_Report_Data = idThemeData,
                RowNum = data.CodeRowNum,
                full_name = data.full_name,
                expert_busyness = data.expert_busyness,
                expert_speciality = data.expert_speciality,
                expertise_type = data.expertise_type,
                mee_quantity_plan = data.mee_quantity_plan,
                mee_quantity_fact = data.mee_quantity_fact,
                mee_quantity_percent = data.mee_quantity_percent,
                mee_yeild_plan = data.mee_yeild_plan,
                mee_yeild_fact = data.mee_yeild_fact,
                mee_yeild_percent = data.mee_yeild_percent,
                ekmp_quantity_plan = data.ekmp_quantity_plan,
                ekmp_quantity_fact = data.ekmp_quantity_fact,
                ekmp_quantity_percent = data.ekmp_quantity_percent,
                ekmp_yeild_plan = data.ekmp_yeild_plan,
                ekmp_yeild_fact = data.ekmp_yeild_fact,
                ekmp_yeild_percent = data.ekmp_yeild_percent,
            };
    }
}