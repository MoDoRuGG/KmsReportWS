using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSOncoCT
    {
        public string Filial { get; set; }
        public List<OncoCT_MEE> OncoCT_MEE { get; set; }
    }

    public class OncoCT_MEE
    {
        public string Row { get; set; }
        public int Target { get; set; }
    }
}