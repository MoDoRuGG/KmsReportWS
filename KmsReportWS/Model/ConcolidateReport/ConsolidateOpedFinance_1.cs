using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateOpedFinance_1
    {
        public string  RegionName { get; set; }
        public string  IdRegion { get; set; }
        public string   Yymm { get; set; }
        public decimal? Fact { get; set; }
        public decimal? PlanO { get; set; }
        public decimal? Mee { get; set; }
        public decimal? Ekmp { get; set; }
        public decimal? Penalty { get; set; }
        public decimal? CountRegularExpertMee { get; set; }
        public decimal? CountRegularExpertEkmp { get; set; }
        public decimal? CountFreelanceExpert { get; set; }
        public decimal? PaymentFreelanceExpert { get; set; }
        public decimal? PenaltyTfoms { get; set; }
    }
}