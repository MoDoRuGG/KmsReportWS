using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportOpedU : AbstractReport
    {
        public List<ReportOpedUDto> ReportDataList;
    }


    public class ReportOpedUDto
    {
        public string Theme { get; set; }
        public ReportOpedUDataDto Data { get; set; }
    }

    public class ReportOpedUDataDto
    {
        public string RowNum { get; set; }
        public decimal App { get; set; }
        public decimal Ks { get; set; }
        public decimal Ds { get; set; }
        public decimal Smp { get; set; }
        public string Notes { get; set; }
    }
}