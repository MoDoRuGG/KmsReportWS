using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Dictionary
{
    public class QuantityPlanDictionaryItem
    {
        public int IdQuantityPlan { get; set; }
        public string IdRegion { get; set; }

        public string Yymm { get; set; }

        public int Value { get; set; }

    }
}