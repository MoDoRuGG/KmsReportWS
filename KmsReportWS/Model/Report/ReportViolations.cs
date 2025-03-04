using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class ReportViolations : AbstractReport
    {
        public List<ReportViolationsDto> ReportDataList { get; set; }
    }

    public class ReportViolationsDto
    {
        public string Theme { get; set; }
        public List<ReportViolationsDataDto> Data { get; set; }
    }

    public class ReportViolationsDataDto
    {
        public string Code { get; set; }
        public decimal Count { get; set; }

    }
}