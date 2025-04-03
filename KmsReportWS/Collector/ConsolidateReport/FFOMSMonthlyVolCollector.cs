using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class FFOMSMonthlyVolCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;
        private readonly string _yymm;

        public FFOMSMonthlyVolCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public List<FFOMSMonthlyVol> Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            var filials = db.Region.Where(x => x.id != "RU" && x.id != "RU-KHA").Select(x => x.id);

            IEnumerable<Task<FFOMSMonthlyVol>> tasks = filials.Select(filial => CollectFilialData(db, filial));
            return tasks.Select(task => task.Result).ToList();
        }

        private async Task<FFOMSMonthlyVol> CollectFilialData(LinqToSqlKmsReportDataContext db, string filial)
        {
            var SKP_Task = CollectSKPAsync(_yymm, filial);
            var SDP_Task = CollectSDPAsync(_yymm, filial);
            var APP_Task = CollectAPPAsync(_yymm, filial);
            var SMP_Task = CollectSMPAsync(_yymm, filial);

            var skp = await SKP_Task;
            var sdp = await SDP_Task;
            var app = await APP_Task;
            var smp = await SMP_Task;

            return new FFOMSMonthlyVol
            {
                Filial = filial,
                FFOMSMonthlyVol_SKP = skp,
                FFOMSMonthlyVol_SDP = sdp,
                FFOMSMonthlyVol_APP = app,
                FFOMSMonthlyVol_SMP = smp
            };
        }

        public async Task<List<FFOMSMonthlyVol_SKP>> CollectSKPAsync(string yymm, string region)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            var data = db.FFOMS_MonthlyVol(yymm, "Стационарная помощь", region)
                         .Where(table => table.Id_Region != "RU-KHA")
                         .OrderBy(table => table.RowNum)
                         .ToList();

            var monthlyVolData_SKP = data.Select(item => new FFOMSMonthlyVol_SKP
            {
                RowNum = item.RowNum,
                CountSluch = item.CountSluch ?? 0,
                CountAppliedSluch = item.CountAppliedSluch ?? 0,
                CountSluchMEE = item.CountSluchMEE ?? 0,
                CountSluchEKMP = item.CountSluchEKMP ?? 0
            }).ToList();

            return monthlyVolData_SKP;
        }

        public async Task<List<FFOMSMonthlyVol_SDP>> CollectSDPAsync(string yymm, string region)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            var data = db.FFOMS_MonthlyVol(yymm, "Дневной стационар", region)
                         .Where(table => table.Id_Region != "RU-KHA")
                         .OrderBy(table => table.RowNum)
                         .ToList();

            var monthlyVolData_SDP = data.Select(item => new FFOMSMonthlyVol_SDP
            {
                RowNum = item.RowNum,
                CountSluch = item.CountSluch ?? 0,
                CountAppliedSluch = item.CountAppliedSluch ?? 0,
                CountSluchMEE = item.CountSluchMEE ?? 0,
                CountSluchEKMP = item.CountSluchEKMP ?? 0
            }).ToList();

            return monthlyVolData_SDP;
        }

        public async Task<List<FFOMSMonthlyVol_APP>> CollectAPPAsync(string yymm, string region)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            var data = db.FFOMS_MonthlyVol(yymm, "АПП", region)
                         .Where(table => table.Id_Region != "RU-KHA")
                         .OrderBy(table => table.RowNum)
                         .ToList();

            var monthlyVolData_APP = data.Select(item => new FFOMSMonthlyVol_APP
            {
                RowNum = item.RowNum,
                CountSluch = item.CountSluch ?? 0,
                CountAppliedSluch = item.CountAppliedSluch ?? 0,
                CountSluchMEE = item.CountSluchMEE ?? 0,
                CountSluchEKMP = item.CountSluchEKMP ?? 0
            }).ToList();

            return monthlyVolData_APP;
        }

        public async Task<List<FFOMSMonthlyVol_SMP>> CollectSMPAsync(string yymm, string region)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            var data = db.FFOMS_MonthlyVol(yymm, "Скорая медицинская помощь", region)
                         .Where(table => table.Id_Region != "RU-KHA")
                         .OrderBy(table => table.RowNum)
                         .ToList();

            var monthlyVolData_SMP = data.Select(item => new FFOMSMonthlyVol_SMP
            {
                RowNum = item.RowNum,
                CountSluch = item.CountSluch ?? 0,
                CountAppliedSluch = item.CountAppliedSluch ?? 0,
                CountSluchMEE = item.CountSluchMEE ?? 0,
                CountSluchEKMP = item.CountSluchEKMP ?? 0
            }).ToList();

            return monthlyVolData_SMP;
        }
    }
}