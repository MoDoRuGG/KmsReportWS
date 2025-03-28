using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSViolMEE
    {
        public string Filial { get; set; }
        public List<FFOMSViolMEEdata> DataViolMEE { get; set; }
    }

    public class FFOMSViolMEEdata
    {
        public string RowNum { get; set; }
        public int Count { get; set; }
    }
}