using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateProposal
    {
        public string RegionId { get; set; }

        public string RegionName { get; set; }
        public int? CountMoCheck { get; set; }
        public int? CountMoCheckWithDefect { get; set; }

        public int? CountProporsals { get; set; }

        public int? CountProporsalsWithDefect { get; set; }

        public string Notes { get; set; }

    }
}