using System.Collections.Generic;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class CReportQuantityFilial
    {
        public string Filial { get; set; }
        public string Theme {  get; set; }
        public ReportQuantity Data { get; set; }
    }
}