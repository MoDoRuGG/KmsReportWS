using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportOped : AbstractReport
    {
        public List<ReportOpedDto> ReportDataList { get; set; }
    }

    public class ReportOpedDto
    {
        public string RowNum { get; set; }
        public decimal App { get; set; }
        public decimal Ks { get; set; }
        public decimal Ds { get; set; }
        public decimal Smp { get; set; }
        public string Notes { get; set; }
    }
}