using System.Collections.Generic;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class CReport262Table1
    {
        public string Filial { get; set; }
        public List<CReport262Table1Row> ListOfCountPpl { get; set; }
        public List<CReport262Table1Row> ListOfCountInform { get; set; }

    }

    public class CReport262Table1Row
    {
        public int Value { get; set; }

        public string yymm { get; set; }
    }

    public class CReport262Table2
    {
        public string Filial { get; set; }
        public Report262DataDto Data { get; set; }
    }

    public class CReport262Table3
    {
        public string Filial { get; set; }
        public Report262Table3Data Data { get; set; }
    }
}