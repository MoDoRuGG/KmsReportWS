using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateCadreCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<CReportCadreTable1> CreateReportCadreTable1(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from table in db.cadre_rapport(yymm,"Отдел ЗПЗ и ЭКМП")         //  функция вывода табличного значения в SQL
                    where table.Id_Region != "RU-KHA" && table.Id_Region != "RU-LEN"        
                    group new { table } by new { table.Id_Region }
                into x
                    select new CReportCadreTable1
                    {
                        Filial = x.Key.Id_Region,
                        Data = new ReportCadreDataDto
                        {
                            count_itog_state = x.Sum(g => g.table.count_itog_state ?? 0),
                            count_itog_fact = x.Sum(g => g.table.count_itog_fact ?? 0),
                            count_itog_vacancy = x.Sum(g => g.table.count_itog_vacancy ?? 0),
                            count_leader_state = x.Sum(g => g.table.count_leader_state ?? 0),
                            count_leader_fact = x.Sum(g => g.table.count_leader_fact ?? 0),
                            count_leader_vacancy = x.Sum(g => g.table.count_leader_vacancy ?? 0),
                            count_deputy_leader_state = x.Sum(g => g.table.count_deputy_leader_state ?? 0),
                            count_deputy_leader_fact = x.Sum(g => g.table.count_deputy_leader_fact ?? 0),
                            count_deputy_leader_vacancy = x.Sum(g => g.table.count_deputy_leader_vacancy ?? 0),
                            count_expert_doctor_state = x.Sum(g => g.table.count_expert_doctor_state ?? 0),
                            count_expert_doctor_fact = x.Sum(g => g.table.count_expert_doctor_fact ?? 0),
                            count_expert_doctor_vacancy = x.Sum(g => g.table.count_expert_doctor_vacancy ?? 0),
                            count_grf15 = x.Sum(g => g.table.count_grf15 ?? 0),
                            count_grf16 = x.Sum(g => g.table.count_grf16 ?? 0),
                            count_grf17 = x.Sum(g => g.table.count_grf17 ?? 0),
                            count_grf18 = x.Sum(g => g.table.count_grf18 ?? 0),
                            count_grf19 = x.Sum(g => g.table.count_grf19 ?? 0),
                            count_grf20 = x.Sum(g => g.table.count_grf20 ?? 0),
                            count_grf21 = x.Sum(g => g.table.count_grf21 ?? 0),
                            count_grf22 = x.Sum(g => g.table.count_grf22 ?? 0),
                            count_grf23 = x.Sum(g => g.table.count_grf23 ?? 0),
                            count_grf24 = x.Sum(g => g.table.count_grf24 ?? 0),
                            count_grf25 = x.Sum(g => g.table.count_grf25 ?? 0),
                            count_grf26 = x.Sum(g => g.table.count_grf26 ?? 0),
                            count_specialist_state = x.Sum(g => g.table.count_specialist_state ?? 0),
                            count_specialist_fact = x.Sum(g => g.table.count_specialist_fact ?? 0),
                            count_specialist_vacancy = x.Sum(g => g.table.count_specialist_vacancy ?? 0)
                        }
                    }).ToList();
        }

        public List<CReportCadreTable2> CreateReportCadreTable2(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from table in db.cadre_rapport(yymm, "ОИ и ЗПЗ")                //  функция вывода табличного значения в SQL
                    group new { table } by new { table.Id_Region }
                            into x
                    select new CReportCadreTable2
                    {
                        Filial = x.Key.Id_Region,
                        Data = new ReportCadreDataDto
                        {
                            count_itog_state = x.Sum(g => g.table.count_itog_state ?? 0),
                            count_itog_fact = x.Sum(g => g.table.count_itog_fact ?? 0),
                            count_itog_vacancy = x.Sum(g => g.table.count_itog_vacancy ?? 0),
                            count_leader_state = x.Sum(g => g.table.count_leader_state ?? 0),
                            count_leader_fact = x.Sum(g => g.table.count_leader_fact ?? 0),
                            count_leader_vacancy = x.Sum(g => g.table.count_leader_vacancy ?? 0),
                            count_deputy_leader_state = x.Sum(g => g.table.count_deputy_leader_state ?? 0),
                            count_deputy_leader_fact = x.Sum(g => g.table.count_deputy_leader_fact ?? 0),
                            count_deputy_leader_vacancy = x.Sum(g => g.table.count_deputy_leader_vacancy ?? 0),
                            count_expert_doctor_state = x.Sum(g => g.table.count_expert_doctor_state ?? 0),
                            count_expert_doctor_fact = x.Sum(g => g.table.count_expert_doctor_fact ?? 0),
                            count_expert_doctor_vacancy = x.Sum(g => g.table.count_expert_doctor_vacancy ?? 0),
                            count_grf15 = x.Sum(g => g.table.count_grf15 ?? 0),
                            count_grf16 = x.Sum(g => g.table.count_grf16 ?? 0),
                            count_grf17 = x.Sum(g => g.table.count_grf17 ?? 0),
                            count_grf18 = x.Sum(g => g.table.count_grf18 ?? 0),
                            count_grf19 = x.Sum(g => g.table.count_grf19 ?? 0),
                            count_grf20 = x.Sum(g => g.table.count_grf20 ?? 0),
                            count_grf21 = x.Sum(g => g.table.count_grf21 ?? 0),
                            count_grf22 = x.Sum(g => g.table.count_grf22 ?? 0),
                            count_grf23 = x.Sum(g => g.table.count_grf23 ?? 0),
                            count_grf24 = x.Sum(g => g.table.count_grf24 ?? 0),
                            count_grf25 = x.Sum(g => g.table.count_grf25 ?? 0),
                            count_grf26 = x.Sum(g => g.table.count_grf26 ?? 0),
                            count_specialist_state = x.Sum(g => g.table.count_specialist_state ?? 0),
                            count_specialist_fact = x.Sum(g => g.table.count_specialist_fact ?? 0),
                            count_specialist_vacancy = x.Sum(g => g.table.count_specialist_vacancy ?? 0)
                        }
                    }).ToList();
        }
    }
}