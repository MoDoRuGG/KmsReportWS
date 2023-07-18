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
    public class ConsolidateOpedQCollector
    {
        public List<ConsolidateOpedQ> Collect(string yymm)
        {
            List<ConsolidateOpedQ> result = new List<ConsolidateOpedQ>();
            MsConnection connection = new MsConnection(Settings.Default.ConnStr);
            connection.NewSp("p_OpedQCons");
            connection.AddSpParam("@yymm", yymm);
            var dt = connection.DataTable();

            if(dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    result.Add(new ConsolidateOpedQ 
                    {
                        Region = row["Region"].ToString(),

                        MeePovtorPlan = row.ToDecimal("MeePovtorPlan"),
                        MeePovtorFact = row.ToDecimal("MeePovtorFact"),

                        MeeOnkoPlan = row.ToDecimal("MeeOnkoPlan"),
                        MeeOnkoFact = row.ToDecimal("MeeOnkoFact"),

                        EkmpLetalPlan = row.ToDecimal("EkmpLetalPlan"),
                        EkmpLetalFact = row.ToDecimal("EkmpLetalFact"),

                        Notes = row["Notes"].ToString(),
                        NotesGoodReason = row["NotesGoodReason"].ToString()

                    });
                }
            }

            return result;
        }
    }
}