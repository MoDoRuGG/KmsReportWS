using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model
{
    public class ReportDynamicScanModel
    {
        public int IdReportDynamicScan { get; set; }

        public int idFlow { get; set; }

        public string FileName { get; set; }
    }
}