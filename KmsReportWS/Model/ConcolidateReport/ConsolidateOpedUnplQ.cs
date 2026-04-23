using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateOpedUnplQ
    {
        public string Region { get; set; }

        public decimal LethalPlan { get; set; }

        public decimal LethalFact { get; set; }

        public decimal PovtorPlan { get; set; }

        public decimal PovtorFact { get; set; }

        public decimal OncoPlan { get; set; }

        public decimal OncoFact { get; set; }

        public decimal EcoPlan { get; set; }

        public decimal EcoFact { get; set; }

        public string Notes { get; set; }
    }
}
