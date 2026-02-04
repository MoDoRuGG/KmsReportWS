using System.Collections.Generic;


namespace KmsReportWS.Model.Report
{
    public class ReportT7OldPolis : AbstractReport
    {
        public List<ReportT7OldPolisDto> ReportDataList;
    }

    public class ReportT7OldPolisDto
    {
        public string Theme { get; set; }
        public ReportT7OldPolisDataDto Data { get; set; }
    }
    public class ReportT7OldPolisDataDto
    {
        public int Id { get; set; }
        public int CurrentQuantity { get; set; }
        public int CountOldPolis { get; set; }
    }
}