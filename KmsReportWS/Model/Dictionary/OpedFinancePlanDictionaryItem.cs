using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Dictionary
{
    public class OpedFinancePlanDictionaryItem
    {
        public int IdOpedFinancePlan { get; set; }
        public string IdRegion { get; set; }

        public string Yymm { get; set; }

        public decimal Value { get; set; }

    }
}