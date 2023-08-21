using System;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model
{
    public class ReportFlowDto
    {
        public string IdRegion { get; set; }
        public string IdReport { get; set; }
        public string Scan { get; set; }
        public string Scan2 { get; set; }
        public string Scan3 { get; set; }
        public string Scan4 { get; set; }
        public string Scan5 { get; set; }
        public string Scan6 { get; set; }
        public string Scan7 { get; set; }
        public string Scan8 { get; set; }
        public string Scan9 { get; set; }
        public string Scan10 { get; set; }
        public string Yymm { get; set; }
        public DateTime? DateToCo { get; set; }
        public DateTime? DateEditCo { get; set; }
        public DateTime? DateIsDone { get; set; }
        public ReportStatus Status { get; set; }
        public DataSource DataSource{ get; set; }
    }
}