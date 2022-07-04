using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ZpzForWebSite
    {
        public string Filial { get; set; }
        public List<ZpzTreatment> Treatments { get; set; }
        public List<ZpzTreatment> Complaints { get; set; }
        public List<ZpzStatistics> Protections { get; set; }
        public List<Expertise> Expertises { get; set; }
        public List<ZpzStatistics> Specialists { get; set; }
        public List<ZpzStatistics> Complacence { get; set; }
        public List<ZpzStatistics> Informations { get; set; }
    }

    public class ZpzTreatment
    {
        public string Row { get; set; }
        public int Oral { get; set; }
        public int Written { get; set; }
    }

    public class Expertise
    {
        public string Row { get; set; }
        public int Target { get; set; }
        public int Plan { get; set; }
        public int Violation { get; set; }
    }

    public class ZpzStatistics
    {
        public string Row { get; set; }
        public decimal Count { get; set; }
    }
}