using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class ReportCadre : AbstractReport
    {
        public List<ReportCadreDto> ReportDataList;
    }

    public class ReportCadreDto
    {
        public string Theme { get; set; }
        public ReportCadreDataDto Data { get; set; }
    }

    public class ReportCadreDataDto
    {
        public int Id { get; set; }
        public decimal count_itog_state { get; set; }
        public decimal count_itog_fact { get; set; }
        public decimal count_itog_vacancy { get; set; }

        public decimal count_leader_state { get; set; }
        public decimal count_leader_fact { get; set; }
        public decimal count_leader_vacancy { get; set; }
        public decimal count_deputy_leader_state { get; set; }

        public decimal count_deputy_leader_fact { get; set; }
        public decimal count_deputy_leader_vacancy { get; set; }
        public decimal count_expert_doctor_state { get; set; }
        public decimal count_expert_doctor_fact { get; set; }
        public decimal count_expert_doctor_vacancy { get; set; }
        public decimal count_specialist_state { get; set; }
        public decimal count_specialist_fact { get; set; }
        public decimal count_specialist_vacancy { get; set; }
        public int count_grf15 { get; set; }
        public int count_grf16 { get; set; }
        public int count_grf17 { get; set; }
        public int count_grf18 { get; set; }
        public int count_grf19 { get; set; }
        public int count_grf20 { get; set; }
        public int count_grf21 { get; set; }
        public int count_grf22 { get; set; }
        public int count_grf23 { get; set; }
        public int count_grf24 { get; set; }
        public int count_grf25 { get; set; }
        public int count_grf26 { get; set; }
        
    }
}