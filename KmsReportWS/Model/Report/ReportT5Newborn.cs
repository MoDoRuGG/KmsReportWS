using System.Collections.Generic;


namespace KmsReportWS.Model.Report
{
    public class ReportT5Newborn : AbstractReport
    {
        public List<ReportT5NewbornDto> ReportDataList;
    }

    public class ReportT5NewbornDto
    {
        public string Theme { get; set; }
        public ReportT5NewbornDataDto Data { get; set; }
    }
    public class ReportT5NewbornDataDto
    {
        public int Id { get; set; }
        public decimal MarketShare { get; set; }
        public decimal CountNewborn { get; set; }
        public decimal CountMaterinityBills { get; set; }
    }
}