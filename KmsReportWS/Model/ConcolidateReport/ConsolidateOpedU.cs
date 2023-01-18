using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateOpedU
    {
        public string Filial { get; set; }
        public OpedUData Mee { get; set; }
        public OpedUData Ekmp { get; set; }

    }





    public class ConsolidateOpedUData
    {
        public string Filial { get; set; }
        public CReportOpedU R1 { get; set; }
        public CReportOpedU R4 { get; set; }
        public CReportOpedU R5 { get; set; }
    }

    public class OpedUData
    {
        public decimal app { get; set; }
        public decimal ks { get; set; }
        public decimal ds { get; set; }
        public decimal smp { get; set; }
        public string notes { get; set; }

        public OpedUData()
        {

        }


        public OpedUData(DataRow data)
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

            notes = notes.ToString();


        }


    }
}
