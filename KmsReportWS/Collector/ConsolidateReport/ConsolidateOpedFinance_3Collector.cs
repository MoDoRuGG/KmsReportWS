using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateOpedFinance_3Collector
    {
        public List<ConsolidateOpedFinance_3> Collect(string year)
        {
            List<ConsolidateOpedFinance_3> result = new List<ConsolidateOpedFinance_3>();
            try
            {
                using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
                {
                    connect.NewSp("p_ConsolidateOpedFinance_3");
                    connect.AddSpParam("@year", year);
                    var dt = connect.DataTable();

                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateOpedFinance_3
                        {
                            RegionName = row["RegionName"].ToString(),
                            IdRegion = row["IdRegion"].ToString(),
                            Yymm = row["Yymm"].ToString(),
                            Fact = row.ToDecimal("Fact"),
                            PlanO = row.ToDecimal("PlanO"),
                            Notes = row["Notes"].ToString()
                        });
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}