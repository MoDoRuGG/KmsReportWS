using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSTargetedExp
    {
        public string Filial { get; set; }
        public List<MEE> MEE { get; set; }
        public List<EKMP> EKMP { get; set; }
        public List<MD_EKMP> MD_EKMP { get; set; }
    }

    public class MEE
    {
        public string Row { get; set; }
        public int Target { get; set; }
    }

    public class EKMP
    {
        public string Row { get; set; }
        public int Target { get; set; }
    }

    public class MD_EKMP
    {
        public string Row { get; set; }
        public int Target { get; set; }
 
    }
}