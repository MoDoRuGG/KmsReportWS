using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using KmsReportWS.Model.ConcolidateReport;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateVCRReportFilialCollector
    {
        private readonly string _connStr = Settings.Default.ConnStr;

        public List<CReportVCRFilial> CreateReportConsolidateVCRFilial(string yymm)
        {
            using var db = new LinqToSqlKmsReportDataContext(_connStr);
            return (from table in db.p_VCRMonitoring_SvodFilial(yymm)         //  функция вывода табличного значения в SQL
                    group new { table } by new { table.Id_Region }
                into x
                    select new CReportVCRFilial
                    {
                        Filial = x.Key.Id_Region,
                        Data = new ReportVCRFilialDataDto
                        {
                            _1_ExpertWithEducation = x.Sum(g => g.table._1_ExpertWithEducation) ?? 0,
                            _1_ExpertWithoutEducation = x.Sum(g => g.table._1_ExpertWithoutEducation) ?? 0,
                            _1_total = x.Sum(g => g.table._1_total) ?? 0,
                            _11_ExpertWithEducation = x.Sum(g => g.table._11_ExpertWithEducation) ?? 0,
                            _11_ExpertWithoutEducation = x.Sum(g => g.table._11_ExpertWithoutEducation) ?? 0,
                            _11_total = x.Sum(g => g.table._11_total) ?? 0,
                            _12_ExpertWithEducation = x.Sum(g => g.table._12_ExpertWithEducation) ?? 0,
                            _12_ExpertWithoutEducation = x.Sum(g => g.table._12_ExpertWithoutEducation) ?? 0,
                            _12_total = x.Sum(g => g.table._12_total) ?? 0,
                            _2_ExpertWithEducation = x.Sum(g => g.table._2_ExpertWithEducation) ?? 0,
                            _2_ExpertWithoutEducation = x.Sum(g => g.table._2_ExpertWithoutEducation) ?? 0,
                            _2_total = x.Sum(g => g.table._2_total) ?? 0,
                            _21_ExpertWithEducation = x.Sum(g => g.table._21_ExpertWithEducation) ?? 0,
                            _21_ExpertWithoutEducation = x.Sum(g => g.table._21_ExpertWithoutEducation) ?? 0,
                            _21_total = x.Sum(g => g.table._21_total) ?? 0,
                            _211_ExpertWithEducation = x.Sum(g => g.table._211_ExpertWithEducation) ?? 0,
                            _211_ExpertWithoutEducation = x.Sum(g => g.table._211_ExpertWithoutEducation) ?? 0,
                            _211_total = x.Sum(g => g.table._211_total) ?? 0,
                            _212_ExpertWithEducation = x.Sum(g => g.table._212_ExpertWithEducation) ?? 0,
                            _212_ExpertWithoutEducation = x.Sum(g => g.table._212_ExpertWithoutEducation) ?? 0,
                            _212_total = x.Sum(g => g.table._212_total) ?? 0,
                            _213_ExpertWithEducation = x.Sum(g => g.table._213_ExpertWithEducation) ?? 0,
                            _213_ExpertWithoutEducation = x.Sum(g => g.table._213_ExpertWithoutEducation) ?? 0,
                            _213_total = x.Sum(g => g.table._213_total) ?? 0,
                            _214_ExpertWithEducation = x.Sum(g => g.table._214_ExpertWithEducation) ?? 0,
                            _214_ExpertWithoutEducation = x.Sum(g => g.table._214_ExpertWithoutEducation) ?? 0,
                            _214_total = x.Sum(g => g.table._214_total) ?? 0,
                            _215_ExpertWithEducation = x.Sum(g => g.table._215_ExpertWithEducation) ?? 0,
                            _215_ExpertWithoutEducation = x.Sum(g => g.table._215_ExpertWithoutEducation) ?? 0,
                            _215_total = x.Sum(g => g.table._215_total) ?? 0,
                            _216_ExpertWithEducation = x.Sum(g => g.table._216_ExpertWithEducation) ?? 0,
                            _216_ExpertWithoutEducation = x.Sum(g => g.table._216_ExpertWithoutEducation) ?? 0,
                            _216_total = x.Sum(g => g.table._216_total) ?? 0,
                            _217_ExpertWithEducation = x.Sum(g => g.table._217_ExpertWithEducation) ?? 0,
                            _217_ExpertWithoutEducation = x.Sum(g => g.table._217_ExpertWithoutEducation) ?? 0,
                            _217_total = x.Sum(g => g.table._217_total) ?? 0,
                            _218_ExpertWithEducation = x.Sum(g => g.table._218_ExpertWithEducation) ?? 0,
                            _218_ExpertWithoutEducation = x.Sum(g => g.table._218_ExpertWithoutEducation) ?? 0,
                            _218_total = x.Sum(g => g.table._218_total) ?? 0,
                            _219_ExpertWithEducation = x.Sum(g => g.table._219_ExpertWithEducation) ?? 0,
                            _219_ExpertWithoutEducation = x.Sum(g => g.table._219_ExpertWithoutEducation) ?? 0,
                            _219_total = x.Sum(g => g.table._219_total) ?? 0,
                            _2110_ExpertWithEducation = x.Sum(g => g.table._2110_ExpertWithEducation) ?? 0,
                            _2110_ExpertWithoutEducation = x.Sum(g => g.table._2110_ExpertWithoutEducation) ?? 0,
                            _2110_total = x.Sum(g => g.table._2110_total) ?? 0,
                            _22_ExpertWithEducation = x.Sum(g => g.table._22_ExpertWithEducation) ?? 0,
                            _22_ExpertWithoutEducation = x.Sum(g => g.table._22_ExpertWithoutEducation) ?? 0,
                            _22_total = x.Sum(g => g.table._22_total) ?? 0,
                            _221_ExpertWithEducation = x.Sum(g => g.table._221_ExpertWithEducation) ?? 0,
                            _221_ExpertWithoutEducation = x.Sum(g => g.table._221_ExpertWithoutEducation) ?? 0,
                            _221_total = x.Sum(g => g.table._221_total) ?? 0,
                            _222_ExpertWithEducation = x.Sum(g => g.table._222_ExpertWithEducation) ?? 0,
                            _222_ExpertWithoutEducation = x.Sum(g => g.table._222_ExpertWithoutEducation) ?? 0,
                            _222_total = x.Sum(g => g.table._222_total) ?? 0,
                            _223_ExpertWithEducation = x.Sum(g => g.table._223_ExpertWithEducation) ?? 0,
                            _223_ExpertWithoutEducation = x.Sum(g => g.table._223_ExpertWithoutEducation) ?? 0,
                            _223_total = x.Sum(g => g.table._223_total) ?? 0,
                            _224_ExpertWithEducation = x.Sum(g => g.table._224_ExpertWithEducation) ?? 0,
                            _224_ExpertWithoutEducation = x.Sum(g => g.table._224_ExpertWithoutEducation) ?? 0,
                            _224_total = x.Sum(g => g.table._224_total) ?? 0,
                            _225_ExpertWithEducation = x.Sum(g => g.table._225_ExpertWithEducation) ?? 0,
                            _225_ExpertWithoutEducation = x.Sum(g => g.table._225_ExpertWithoutEducation) ?? 0,
                            _225_total = x.Sum(g => g.table._225_total) ?? 0,
                            _226_ExpertWithEducation = x.Sum(g => g.table._226_ExpertWithEducation) ?? 0,
                            _226_ExpertWithoutEducation = x.Sum(g => g.table._226_ExpertWithoutEducation) ?? 0,
                            _226_total = x.Sum(g => g.table._226_total) ?? 0,
                            _227_ExpertWithEducation = x.Sum(g => g.table._227_ExpertWithEducation) ?? 0,
                            _227_ExpertWithoutEducation = x.Sum(g => g.table._227_ExpertWithoutEducation) ?? 0,
                            _227_total = x.Sum(g => g.table._227_total) ?? 0,
                            _228_ExpertWithEducation = x.Sum(g => g.table._228_ExpertWithEducation) ?? 0,
                            _228_ExpertWithoutEducation = x.Sum(g => g.table._228_ExpertWithoutEducation) ?? 0,
                            _228_total = x.Sum(g => g.table._228_total) ?? 0,
                        }
                    }).ToList();
        }
    }
}