using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class ReportDispReprodHealth : AbstractReport
    {
        public List<ReportDispReprodHealthDto> ReportDataList { get; set; }
    }

    public class ReportDispReprodHealthDto
    {
        public string Theme { get; set; }
        public List<ReportDispReprodHealthDataDto> Data { get; set; }
    }

    public class ReportDispReprodHealthDataDto
    {
        public string Code { get; set; }
        public int YearlySum { get; set; }
        public int ForPeriod { get; set; }
    }
}
