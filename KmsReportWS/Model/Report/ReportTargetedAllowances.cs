using System.Collections.Generic;
using System.Web.UI;

namespace KmsReportWS.Model.Report
{
    public class ReportTargetedAllowances : AbstractReport
    {

        public int Id_Report_Data { get; set; }
        public List<TargetedAllowancesData> Data { get; set; }

        public ReportTargetedAllowances()
        {
            Data = new List<TargetedAllowancesData>();
        }
    }
    public class TargetedAllowancesData
    {
        public int Id { get; set; }
        public int RowNumID { get; set; }
        public string FIO { get; set; }
        public string Speciality { get; set; }
        public string Period { get; set; }
        public int CountEKMP { get; set; }
        public decimal AmountSank { get; set; }
        public decimal AmountPayment { get; set; }
        public string ProvidedBy { get; set; }
        public string Comments { get; set; }
    }
}