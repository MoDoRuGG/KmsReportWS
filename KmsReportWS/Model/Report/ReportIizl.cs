using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class ReportIizl : AbstractReport
    {
        public List<ReportIizlDto> ReportDataList { get; set; }
    }

    public class ReportIizlDto
    {
        public string Theme { get; set; }
        public List<ReportIizlDataDto> Data { get; set; }
        public int TotalPersFirst { get; set; }
        public int TotalPersRepeat { get; set; }
    }

    public class ReportIizlDataDto
    {
        public string Code { get; set; }
        public int CountPersFirst { get; set; }
        public int CountPersRepeat { get; set; }
        public int CountMessages { get; set; }
        public decimal TotalCost { get; set; }

        public decimal AverageCostPerMessage { get; set; }
        public decimal AverageCostOfInforming1PL { get; set; }
        public string AccountingDocument { get; set; }

    }
}