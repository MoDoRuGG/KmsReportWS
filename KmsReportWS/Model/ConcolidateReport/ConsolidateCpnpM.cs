using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateCpnpM
    {
        public string Filial { get; set; }
        public decimal? CountAll { get; set; } //Общее количество жалоб    
        public decimal? CountReason { get; set; } // Количество обоснованных жалоб     

    }
}