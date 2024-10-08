using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Constructor
{
    public class DynamicReportDictionary
    {
        public long id { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }
        public string FileName { get; set; }

    }
}