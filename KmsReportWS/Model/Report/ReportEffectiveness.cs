using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class ReportEffectiveness : AbstractReport
    {
        public List<ReportEffectivenessDto> ReportDataList { get; set; }
    }

    public class ReportEffectivenessDto
    {
        public string Theme { get; set; }
        public List<ReportEffectivenessDataDto> Data { get; set; }
    }

    public class ReportEffectivenessDataDto
    {
        public string CodeRowNum { get; set; }
        public string full_name { get; set; }
        public decimal expert_busyness { get; set; }
        public string expert_speciality { get; set; }
        public string expertise_type { get; set; }
        public decimal mee_quantity_plan { get; set; }
        public decimal mee_quantity_fact { get; set; }
        public decimal mee_quantity_percent { get; set; }
        public decimal mee_yeild_plan { get; set; }
        public decimal mee_yeild_fact { get; set; }
        public decimal mee_yeild_percent { get; set; }
        public decimal ekmp_quantity_plan { get; set; }
        public decimal ekmp_quantity_fact { get; set; }
        public decimal ekmp_quantity_percent { get; set; }
        public decimal ekmp_yeild_plan { get; set; }
        public decimal ekmp_yeild_fact { get; set; }
        public decimal ekmp_yeild_percent { get; set; }
    }
}