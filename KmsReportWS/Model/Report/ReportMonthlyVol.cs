using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportMonthlyVol : AbstractReport
    {
        public List<ReportMonthlyVolDto> ReportDataList { get; set; }
    }

    public class ReportMonthlyVolDto
    {
        public string RowNum { get; set; }
        public int? CountSluch { get; set; }
        public int? CountAppliedSluch { get; set; }
        public int? CountSluchMEE { get; set; }
        public int? CountSluchEKMP { get; set; }
    }
}