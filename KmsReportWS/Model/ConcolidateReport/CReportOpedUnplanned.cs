using System.Collections.Generic;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class CReportOpedUnplanned    {
        public string Filial { get; set; }
        public string RowNum { get; set; }
        public decimal App { get; set; }
        public decimal Ks { get; set; }
        public decimal Ds { get; set; }
        public decimal Smp { get; set; }
        public string Notes { get; set; }
        public string NotesGoodReason { get; set; }
        //public ReportOpedUDto Data { get; set; }
    }

}