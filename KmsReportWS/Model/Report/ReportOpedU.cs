using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportOpedU : AbstractReport
    {
        public List<ReportOpedUDto> ReportDataList { get; set; }
    }

    public class ReportOpedUDto
    {
        public string RowNum { get; set; }
        public decimal App { get; set; }
        public decimal Ks { get; set; }
        public decimal Ds { get; set; }
        public decimal Smp { get; set; }

        public decimal AppOnco { get; set; }
        public decimal KsOnco { get; set; }
        public decimal DsOnco { get; set; }
        public decimal SmpOnco { get; set; }

        public decimal AppLeth { get; set; }
        public decimal KsLeth { get; set; }
        public decimal DsLeth { get; set; }
        public decimal SmpLeth { get; set; }
        public string Notes { get; set; }
    }
}