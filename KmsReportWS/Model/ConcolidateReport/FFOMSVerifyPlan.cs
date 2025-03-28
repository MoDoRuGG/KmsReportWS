using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSVerifyPlan
    {
        public string Filial { get; set; }
        public List<FFOMSVerifyPlandata> DataVerifyPlan { get; set; }
    }

    public class FFOMSVerifyPlandata
    {
        public string RowNum { get; set; }
        public int Count { get; set; }
    }
}