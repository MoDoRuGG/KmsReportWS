using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ZpzForWebSite2023
    {
        public string Filial { get; set; }
        public List<ZpzTreatment2023> Treatments { get; set; }
        public List<ZpzTreatment2023> Complaints { get; set; }
        public List<ZpzStatistics2023> Protections { get; set; }
        public List<Expertise2023> Expertises { get; set; }
        public List<ZpzStatistics2023> Specialists { get; set; }
        public List<ZpzStatistics2023> Informations { get; set; }
    }

    public class ZpzTreatment2023
    {
        public string Row { get; set; }
        public int Oral { get; set; }
        public int Written { get; set; }
    }

    public class Expertise2023
    {
        public string Row { get; set; }
        public int Target { get; set; }
        public int Plan { get; set; }
        public int Violation { get; set; }
    }

    public class ZpzStatistics2023
    {
        public string Row { get; set; }
        public decimal Count { get; set; }
    }
}