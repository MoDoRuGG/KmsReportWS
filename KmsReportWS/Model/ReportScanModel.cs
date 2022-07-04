using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model
{
    public class ReportScanModel
    {
        public int IdScan { get; set; }
        public string FileName { get; set; }

        public string UserAdded { get; set; }

        public DateTime DateAdded { get; set; }
        public string UserUpdate { get; set; }

        public DateTime? DateUpdated { get; set; }


    }
}