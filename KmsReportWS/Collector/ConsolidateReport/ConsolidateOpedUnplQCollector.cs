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
    public class ConsolidateOpedUnplQCollector
    {
        public List<ConsolidateOpedUnplQ> Collect(string yymm)
        {
            List<ConsolidateOpedUnplQ> result = new List<ConsolidateOpedUnplQ>();
            MsConnection connection = new MsConnection(Settings.Default.ConnStr);
            connection.NewSp("p_OpedUnplCons");
            connection.AddSpParam("@yymm", yymm);
            var dt = connection.DataTable();

            if(dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    result.Add(new ConsolidateOpedUnplQ
                    {
                        Region = row["Region"].ToString(),

                        LethalPlan = row.ToDecimal("LethalPlan"),
                        LethalFact = row.ToDecimal("LethalFact"),

                        PovtorPlan = row.ToDecimal("PovtorPlan"),
                        PovtorFact = row.ToDecimal("PovtorFact"),

                        OncoPlan = row.ToDecimal("OncoPlan"),
                        OncoFact = row.ToDecimal("OncoFact"),

                        EcoPlan = row.ToDecimal("EcoPlan"),
                        EcoFact = row.ToDecimal("EcoFact"),

                        Notes = row["Notes"].ToString().Replace(';',' ')

                    });
                }
            }

            return result;
        }
    }
}