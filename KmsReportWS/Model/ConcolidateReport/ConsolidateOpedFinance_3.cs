using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateOpedFinance_3
    {
        public string  RegionName { get; set; }
        public string  IdRegion { get; set; }
        public string   Yymm { get; set; }
        public decimal? Fact { get; set; }
        public decimal? PlanO { get; set; }
        public decimal? Percent { get; set; }
        public string Notes { get; set; }

    }
}