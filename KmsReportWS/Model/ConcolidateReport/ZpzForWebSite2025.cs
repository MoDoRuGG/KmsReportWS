using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ZpzForWebSite2025
    {
        public string Filial { get; set; }
        public List<ZpzTreatment2025> Treatments { get; set; }
        public List<ZpzTreatment2025> Complaints { get; set; }
        public List<ZpzStatistics2025> Protections { get; set; }
        public List<Expertise2025> Expertises { get; set; }
        public List<ZpzStatistics2025> Specialists { get; set; }
        public List<ZpzStatistics2025> Informations { get; set; }
    }

    public class ZpzTreatment2025
    {
        public string Row { get; set; }
        public int Oral { get; set; }
        public int Written { get; set; }
        public int Assignment { get; set; }
    }

    public class Expertise2025
    {
        public string Row { get; set; }
        public int Target { get; set; }
        public int Plan { get; set; }
        public int Violation { get; set; }
    }

    public class ZpzStatistics2025
    {
        public string Row { get; set; }
        public decimal Count { get; set; }
    }
}