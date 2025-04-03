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
    public class FFOMSVolumesByTypesCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;
        private readonly string _yymm;

        public FFOMSVolumesByTypesCollector(string yymm)
        {
            this._yymm = yymm;
        }

        public FFOMSVolumesByTypes Collect()
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);

            // Вызываем CollectFilialData и ждем завершения задачи
            return CollectFilialData(db).Result;
        }

        private async Task<FFOMSVolumesByTypes> CollectFilialData(LinqToSqlKmsReportDataContext db)
        {
            var fullTask = CollectFull(db);
            var filTask = CollectFil(db);

            var volfull = await fullTask;
            var volfil = await filTask;

            return new FFOMSVolumesByTypes
            {
                VolFull = volfull,
                VolFil = volfil,
            };
        }

        private async Task<List<FFOMSVolumesByTypesFull>> CollectFull(LinqToSqlKmsReportDataContext db)
        {
            return (from table in db.FFOMSVolumesByTypesFull(_yymm)         //  функция вывода табличного значения в SQL      
                    select new FFOMSVolumesByTypesFull
                    {
                        mee_unpl = table.mee_unpl ?? 0,
                        mee_pl = table.mee_pl ?? 0,
                        ekmp_unpl = table.ekmp_unpl ?? 0,
                        ekmp_pl = table.ekmp_pl ?? 0,
                        mek_app = table.mek_app ?? 0,
                        mee_app_unpl = table.mee_app_unpl ?? 0,
                        mee_app_pl = table.mee_app_pl ?? 0,
                        ekmp_app_unpl = table.ekmp_app_unpl ?? 0,
                        ekmp_app_pl = table.ekmp_app_pl ?? 0,
                        mek_skp = table.mek_skp ?? 0,
                        mee_skp_unpl = table.mee_skp_unpl ?? 0,
                        mee_skp_pl = table.mee_skp_pl ?? 0,
                        ekmp_skp_unpl = table.ekmp_skp_unpl ?? 0,
                        ekmp_skp_pl = table.ekmp_skp_pl ?? 0,
                        mek_smp = table.mek_smp ?? 0,
                        mee_smp_unpl = table.mee_smp_unpl ?? 0,
                        mee_smp_pl = table.mee_smp_pl ?? 0,
                        ekmp_smp_unpl = table.ekmp_smp_unpl ?? 0,
                        ekmp_smp_pl = table.ekmp_smp_pl ?? 0,
                        mek_sdp = table.mek_sdp ?? 0,
                        mee_sdp_unpl = table.mee_sdp_unpl ?? 0,
                        mee_sdp_pl = table.mee_sdp_pl ?? 0,
                        ekmp_sdp_unpl = table.ekmp_sdp_unpl ?? 0,
                        ekmp_sdp_pl = table.ekmp_sdp_pl ?? 0,
                    }).ToList();
        }

        private async Task<List<FFOMSVolumesByTypesByFilials>> CollectFil(LinqToSqlKmsReportDataContext db)
        {
            return (from table in db.FFOMSVolumesByTypesByFilials(_yymm)         //  функция вывода табличного значения в SQL      
                    where table.Id_Region != "RU-KHA"
                    select new FFOMSVolumesByTypesByFilials
                    {   
                        Code = table.Id_Region,
                        Filial = table.name,
                        mek_app = table.mek_app ?? 0,
                        mee_app = table.mee_app ?? 0,
                        ekmp_app = table.ekmp_app ?? 0,
                        mek_skp = table.mek_skp ?? 0,
                        mee_skp = table.mee_skp ?? 0,
                        ekmp_skp = table.ekmp_skp ?? 0,
                        mek_smp = table.mek_smp ?? 0,
                        mee_smp = table.mee_smp ?? 0,
                        ekmp_smp = table.ekmp_smp ?? 0,
                        mek_sdp = table.mek_sdp ?? 0,
                        mee_sdp = table.mee_sdp ?? 0,
                        ekmp_sdp = table.ekmp_sdp ?? 0,
                    }).ToList();
        }
    }
}