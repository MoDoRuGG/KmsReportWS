using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Dictionary
{
    public class T7OldPolisYearlyDictionaryItem
    {
        public int IdReportT7OldPolisYearly { get; set; }
        public string IdRegion { get; set; }
        public string Yymm { get; set; }
        public int Value { get; set; } = 0;
    }
}