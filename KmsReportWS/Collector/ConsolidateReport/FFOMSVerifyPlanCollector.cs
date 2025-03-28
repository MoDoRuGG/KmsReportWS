using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class FFOMSVerifyPlanCollector
    {
        private static readonly string[] Statuses = {
            ReportStatus.Submit.GetDescriptionSt(), ReportStatus.Done.GetDescriptionSt()
        };

        private static readonly string ConnStr = Settings.Default.ConnStr;

        private readonly string _yymm;

        public FFOMSVerifyPlanCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<FFOMSVerifyPlan> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(ConnStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);
            IEnumerable<Task<FFOMSVerifyPlan>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(x => x.Result).ToList();
            
            
        }

        private async Task<FFOMSVerifyPlan> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var T3FFOMSVERPLTask = CollectFFOMSVERPL(db, filial);

            var VerPL = await T3FFOMSVERPLTask;

            return new FFOMSVerifyPlan
            {
                Filial = filial,
                DataVerifyPlan = VerPL
            };
        }


        private IQueryable<Report_Violations> CollectVerifyPlan(LinqToSqlKmsReportDataContext db, string theme, string region) =>
            from flow in db.Report_Flow
            join data in db.Report_Data on flow.Id equals data.Id_Flow
            join f in db.Report_Violations on data.Id equals f.Id_Report_Data
            where flow.Yymm == _yymm
                  && data.Theme == "Планы проверок"
                  && flow.Id_Region == region
                  && flow.Id_Report_Type == "VerifyPlan"
            select f;

        private async Task<List<FFOMSVerifyPlandata>> CollectFFOMSVERPL(LinqToSqlKmsReportDataContext db, string region)
        {
            var table3 = CollectVerifyPlan(db, "", region);

            var rows = new List<string>
            {
                "1","2","3","4","5","6.1","6.2","7.1","7.2","8.1","8.2","9.1","9.2"
            };

            var T3FFOMSVERPL = rows.Select(row => new FFOMSVerifyPlandata
            {
                RowNum = row,
                Count = Convert.ToInt32(table3.Where(x => x.RowNum == row).Sum(x => x.Count)),
            }).ToList();

            return T3FFOMSVERPL;
        }

    }
}