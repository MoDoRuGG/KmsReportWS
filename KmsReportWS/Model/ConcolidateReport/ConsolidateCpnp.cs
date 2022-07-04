using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateCpnp
    {
        public string Filial { get; set; }

        public decimal? CountPretrial { get; set; } // Досудебный порядок

        public decimal? CountAll { get; set; } //Общее количество жалоб      

        public double NormativRegionCpnp { get; set; } // Региональный норматив ЦПНП 
            
        public double NormativFederalCpnp { get; set; } // Отклонение от регионального норматива ЦПНП, абс.
        


    }

   
}