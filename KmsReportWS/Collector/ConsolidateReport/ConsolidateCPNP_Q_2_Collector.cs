using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateCPNP_Q_2_Collector
    {
     
        public List<ConsolidateCPNP_Q_2> Collect(string yymm)
        {
            List<ConsolidateCPNP_Q_2> result = new List<ConsolidateCPNP_Q_2>();

            MsConnection connect = new MsConnection(Settings.Default.ConnStr);
            connect.NewSp("p_CPNP2_Q");
            connect.AddSpParam("@yymm", yymm);
            using (var dt = connect.DataTable())
            {
                foreach(DataRow row in dt.Rows)
                {
                    result.Add(new ConsolidateCPNP_Q_2 
                    {
                        Filial = row["Filial"].ToString(),
                        CountSporDoSuda = row.ToDecimal("CountSporDoSuda"),
                        CountObosnZhalob = row.ToDecimal("CountObosnZhalob")
                    });
                }
            }

           return result;

        }

    }
}