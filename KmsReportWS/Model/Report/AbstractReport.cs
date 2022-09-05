using System;

namespace KmsReportWS.Model.Report
{
    public class AbstractReport
    {
        public int IdFlow { get; set; }
        public string IdType { get; set; }
        public string Yymm { get; set; }

        public DateTime Created { get; set; }
        public int IdEmployee { get; set; }
        public DateTime? Updated { get; set; }
        public int IdEmployeeUpd { get; set; }

        public DateTime? DateToCo { get; set; }
        public int UserToCo { get; set; }
        public DateTime? RefuseDate { get; set; }
        public int RefuseUser { get; set; }
        public DateTime? DateIsDone { get; set; }
        public int UserSubmit { get; set; }

        public int Version { get; set; }
        public string Scan { get; set; }
        public string Scan2 { get; set; }
        public string Scan3 { get; set; }

        public ReportStatus Status { get; set; }
        public DataSource DataSource { get; set; }
    }
}