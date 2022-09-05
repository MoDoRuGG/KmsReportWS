using System;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model
{
    public class ReportFlowDto
    {
        public string IdRegion { get; set; }
        public string IdReport { get; set; }
        public string Scan { get; set; }
        public string Yymm { get; set; }
        public DateTime? DateToCo { get; set; }
        public DateTime? DateEditCo { get; set; }
        public DateTime? DateIsDone { get; set; }
        public ReportStatus Status { get; set; }
        public DataSource DataSource{ get; set; }
    }
}