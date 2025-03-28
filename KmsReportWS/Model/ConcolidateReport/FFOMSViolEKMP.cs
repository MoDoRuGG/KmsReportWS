using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSViolEKMP
    {
        public string Filial { get; set; }
        public List<FFOMSViolEKMPdata> DataViolEKMP { get; set; }
    }

    public class FFOMSViolEKMPdata
    {
        public string RowNum { get; set; }
        public int Count { get; set; }
    }
}