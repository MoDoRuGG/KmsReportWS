using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateOpedQ
    {
        public string Region { get; set; }

        public decimal MeePovtorPlan { get; set; }

        public decimal MeePovtorFact { get; set; }

        public decimal MeeOnkoPlan { get; set; }

        public decimal MeeOnkoFact { get; set; }

        public decimal EkmpLetalPlan { get; set; }

        public decimal EkmpLetalFact { get; set; }

        public string Notes { get; set; }
    }
}
