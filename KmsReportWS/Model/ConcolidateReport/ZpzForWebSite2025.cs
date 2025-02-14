using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ZpzForWebSite2025
    {
        public string Filial { get; set; }
        public List<WSData2025> WSData { get; set; }
    }

    public class WSData2025
    {
        public decimal Col1 {  get; set; }
        public decimal Col2 { get; set; }
        public decimal Col3 { get; set; }
        public decimal Col4 { get; set; }
        public decimal Col5 { get; set; }
        public decimal Col6 { get; set; }
        public decimal Col8 { get; set; }
        public decimal Col9 { get; set; }
        public decimal Col10 { get; set; }
        public decimal Col11 { get; set; }
        public decimal Col12 { get; set; }
        public decimal Col13 { get; set; }
        public decimal Col14 { get; set; }
    }
}