using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{

    [Table("Report_140n")]
    public class Report140n : AbstractReport
    {
        public List<Report140nDto> ReportDataList;
    }

    public class Report140nDto
    {
        public string Theme { get; set; }
        public Report140nDataDto Data { get; set; }
    }

    public class Report140nDataDto
    {
        [Key]
        public int Id { get; set; }

        public int Id_Report_Data { get; set; }

        public decimal? CZLdost { get; set; }
        public decimal? CZLsmo { get; set; }
        public decimal? KSErez { get; set; }
        public decimal? KSE { get; set; }
        public decimal? PPMinadvn { get; set; }
        public decimal? Iidvn { get; set; }
        public decimal? PPMinfdn { get; set; }
        public decimal? Iidn { get; set; }
        public decimal? KOJdosud { get; set; }
        public decimal? KOJsud { get; set; }
        public decimal? KOJzl { get; set; }
        public decimal? KOJzlsmo { get; set; }
        public decimal? KZAsobl { get; set; }
        public decimal? KZAvsego { get; set; }
        public decimal? DT { get; set; }
        public decimal? Scpo { get; set; }
        public decimal? KEKMPpodtv { get; set; }
        public decimal? KEKMPtfoms { get; set; }
        public decimal? KZSMOpodtv { get; set; }
        public decimal? KPMOtfoms { get; set; }
    }
}