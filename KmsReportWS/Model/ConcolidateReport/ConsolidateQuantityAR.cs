using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateQuantityAR
    {
        public string  RegionName { get; set; }
        public string  IdRegion { get; set; }
        public string   Yymm { get; set; }
        public int Added { get; set; }
        public int Removed { get; set; }
    }
}