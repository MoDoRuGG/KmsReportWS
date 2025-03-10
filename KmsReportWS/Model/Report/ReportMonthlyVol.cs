using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class ReportMonthlyVol : AbstractReport
    {
        public List<ReportMonthlyVolDto> ReportDataList { get; set; }
    }

    public class ReportMonthlyVolDto
    {
        public string Theme { get; set; }
        public List<ReportMonthlyVolDataDto> Data { get; set; }
    }

    public class ReportMonthlyVolDataDto
    {
        public string Code { get; set; }
        public int CountSluch { get; set; }
        public int CountAppliedSluch { get; set; }
        public int CountSluchMEE { get; set; }
        public int CountSluchEKMP { get; set; }

    }
}