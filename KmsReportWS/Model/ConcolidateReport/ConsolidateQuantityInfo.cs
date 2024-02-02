using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateQuantityInfo
    {
        public string  RegionName { get; set; }
        public string  IdRegion { get; set; }
        public string   Yymm { get; set; }
        public int Added {  get; set; }
        public int Fact { get; set; }
        public int Plan { get; set; }
        public int Col_1 { get; set; }
        public int Col_3 { get; set; }
        public int Col_7 { get; set; }
        public int Col_8 { get; set; }
        public int Col_10 { get; set; }
        public int Col_12 { get; set; }
        public int Col_15 { get; set; }
    }
}