using System.Collections.Generic;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class Cons140nTable1
    {
        public string Filial { get; set; }
        public Report140nDataDto Data { get; set; }
    }

    public class Cons140nTable2
    {
        public Report140nDataDto Data { get; set; }
    }
}