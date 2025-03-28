using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSOncoCT
    {
        public string Filial { get; set; }
        public FFOMSOncoCT_MEE OncoCT_MEE { get; set; }
    }

    public class FFOMSOncoCT_MEE
    {
        public string Row { get; set; }
        public decimal Target { get; set; }
    }
}