using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportVaccination : AbstractReport
    {
        public int Id { get; set; }
        public int IdReportData { get; set; }
        public int M18_39 { get; set; }
        public int M40_59 { get; set; }
        public int M60_65 { get; set; }
        public int M66_74 { get; set; }
        public int M75_More { get; set; }
        public int W18_39 { get; set; }
        public int W40_54 { get; set; }    
        public int W55_65 { get; set; }
        public int W66_74 { get; set; }
        public int W75_More { get; set; }
       
    }

}



