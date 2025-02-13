using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ViolationsOfAppeals
    {
        public string Filial { get; set; }
        public List<TreatmentVOA> Treatments { get; set; }
        public List<TreatmentVOA> Complaints { get; set; }
        public List<StatisticsVOA> Protections { get; set; }
        public List<ExpertiseVOA> Expertises { get; set; }
        public List<StatisticsVOA> Specialists { get; set; }
        public List<StatisticsVOA> Informations { get; set; }
    }

    public class TreatmentVOA
    {
        public string Row { get; set; }
        public int Oral { get; set; }
        public int Written { get; set; }
        public int Assignment { get; set; }
    }

    public class ExpertiseVOA
    {
        public string Row { get; set; }
        public int Target { get; set; }
        public int Plan { get; set; }
        public int Violation { get; set; }
    }

    public class StatisticsVOA
    {
        public string Row { get; set; }
        public decimal Count { get; set; }
    }
}