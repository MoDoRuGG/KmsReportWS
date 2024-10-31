using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateOped
    {
        public string Filial { get; set; }
        public OpedData CountSl { get; set; }
        public OpedData CountMee { get; set; }
        public OpedData CountEkmp { get; set; }
        public OpedData Mee { get; set; }
        public OpedData Ekmp { get; set; }

    }


    public class SummaryOped
    {

        public string Filial { get; set; }
        public string rowNum { get; set; }
        public decimal app { get; set; }
        public decimal ks { get; set; }
        public decimal ds { get; set; }
        public decimal smp { get; set; }
    }


    public class ConsolidateOpedData
    {
        public string Filial { get; set; }
        public SummaryOped R1 { get; set; }
        public SummaryOped R4 { get; set; }
        public SummaryOped R5 { get; set; }
    }

    public class OpedData
    {
        public decimal app { get; set; }
        public decimal ks { get; set; }
        public decimal ds { get; set; }
        public decimal smp { get; set; }

        public OpedData()
        {

        }


        public OpedData(DataRow data)
        {
            decimal dTMp = 0.00m;

            if (Decimal.TryParse(data["app"].ToString(), out dTMp))
                app = dTMp;


            if (Decimal.TryParse(data["ks"].ToString(), out dTMp))
                ks = dTMp;


            if (Decimal.TryParse(data["ds"].ToString(), out dTMp))
                ds = dTMp;


            if (Decimal.TryParse(data["smp"].ToString(), out dTMp))
                smp = dTMp;

        }


    }
}
