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
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private readonly string _connStr = Settings.Default.ConnStr;
        private string _themeName = "Effectiveness";

        public ReportEffectivenessHandler(ReportType reportType) : base(reportType)
        {
            if (reportType == ReportType.Effective)
            {
                _themeName = "Effectiveness";
            }
            else
            {
                _themeName = "";
            }
        }

        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow, AbstractReport inReport)
        {
            var report = inReport as ReportEffectiveness ??
                           throw new Exception("Error saving new report, because getting empty report");

            var themeData = new Report_Data
            {
                Id_Flow = flow.Id,
                Id_Report = flow.Id_Report_Type,
                Theme = _themeName,
                General_field_1 = 0,
                General_field_2 = 0
            };
            db.Report_Data.InsertOnSubmit(themeData);


            db.SubmitChanges();

            db.Report_ExpertEffectiveness.InsertAllOnSubmit(MapReportFromPersist(report, themeData.Id));
            db.SubmitChanges();


        }

        protected List<Report_ExpertEffectiveness> MapReportFromPersist(ReportEffectiveness rep, int idReportData)
        {
            var result = new List<Report_ExpertEffectiveness>();

            foreach (var row in rep.ReportDataList)
            {
                result.Add(new Report_ExpertEffectiveness
                {
                    Id_Report_Data = idReportData,
                    RowNum = row.CodeRowNum,
                    full_name = row.full_name,
                    expert_busyness = row.expert_busyness,
                    expert_speciality = row.expert_speciality,
                    expertise_type = row.expertise_type,
                    mee_quantity_plan = row.mee_quantity_plan,
                    mee_quantity_fact = row.mee_quantity_fact,
                    mee_quantity_percent = row.mee_quantity_percent,
                    mee_yeild_plan = row.mee_yeild_plan,
                    mee_yeild_fact = row.mee_yeild_fact,
                    mee_yeild_percent = row.mee_yeild_percent,
                    ekmp_quantity_plan = row.ekmp_quantity_plan,
                    ekmp_quantity_fact = row.ekmp_quantity_fact,
                    ekmp_quantity_percent = row.ekmp_quantity_percent,
                    ekmp_yeild_plan = row.ekmp_yeild_plan,
                    ekmp_yeild_fact = row.ekmp_yeild_fact,
                    ekmp_yeild_percent = row.ekmp_yeild_percent

                });

            }

            return result;

        }


        private Report_ExpertEffectiveness MapReportFromPersist(ReportEffectivenessDto data, int idReportData)
        {
            return new Report_ExpertEffectiveness
            {
                Id_Report_Data = idReportData,
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
                ekmp_yeild_percent = data.ekmp_yeild_percent
            };
        }


        private ReportEffectivenessDto MapReportFromPersist(Report_ExpertEffectiveness data)
        {
            return new ReportEffectivenessDto
            {
                Id = data.Id,
                CodeRowNum = data.RowNum,
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



        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportEffectiveness ??
                         throw new Exception("Error update report, because getting empty report");

            var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow)?.Id ?? 0;
            if (idTheme == 0)
            {
                Log.Error(
                    $"Error getting data. idTheme = 0; IdFlow = {inReport.IdFlow}");
                return;
            }

            foreach (var row in report.ReportDataList)
            {
                var effective = db.Report_ExpertEffectiveness.SingleOrDefault(x => x.RowNum == row.CodeRowNum && x.Id_Report_Data == idTheme);
                if (effective != null)
                {
                    effective.RowNum = row.CodeRowNum;
                    effective.full_name = row.full_name;
                    effective.expert_busyness = row.expert_busyness;
                    effective.expert_speciality = row.expert_speciality;
                    effective.expertise_type = row.expertise_type;
                    effective.mee_quantity_plan = row.mee_quantity_plan;
                    effective.mee_quantity_fact = row.mee_quantity_fact;
                    effective.mee_quantity_percent = row.mee_quantity_percent;
                    effective.mee_yeild_plan = row.mee_yeild_plan;
                    effective.mee_yeild_fact = row.mee_yeild_fact;
                    effective.mee_quantity_percent = row.mee_quantity_percent;
                    effective.ekmp_quantity_plan = row.ekmp_quantity_plan;
                    effective.ekmp_quantity_fact = row.ekmp_quantity_fact;
                    effective.ekmp_quantity_percent = row.ekmp_quantity_percent;
                    effective.ekmp_yeild_plan = row.ekmp_yeild_plan;
                    effective.ekmp_yeild_fact = row.ekmp_yeild_fact;
                    effective.ekmp_yeild_percent = row.ekmp_yeild_percent;
                }
                else
                {
                    var effectiveIns = MapReportFromPersist(row, idTheme);
                    db.Report_ExpertEffectiveness.InsertOnSubmit(effectiveIns);

                }
                db.SubmitChanges();

            }
        }

        protected override AbstractReport MapReportFromPersist(Report_Flow rep)
        {
            var outReport = new ReportEffectiveness { ReportDataList = new List<ReportEffectivenessDto>() };
            MapFromReportFlow(rep, outReport);


            foreach (var themeData in rep.Report_Data)
            {
                var dataList = themeData.Report_ExpertEffectiveness.Select(MapReportFromPersist).ToList();
                outReport.ReportDataList.AddRange(dataList);
            }

            return outReport;
        }
    }
}