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
    public class ConsolidateOpedFinance_1Collector
    {
        public List<ConsolidateOpedFinance_1> Collect(string year)
        {
            List<ConsolidateOpedFinance_1> result = new List<ConsolidateOpedFinance_1>();
            try
            {
                using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
                {
                    connect.NewSp("p_ConsolidateOpedFinance_1");
                    connect.AddSpParam("@year", year);
                    var dt = connect.DataTable();

                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateOpedFinance_1
                        {
                            RegionName = row["RegionName"].ToString(),
                            IdRegion = row["IdRegion"].ToString(),
                            Yymm = row["Yymm"].ToString(),
                            Fact = row.ToDecimal("Fact"),
                            PlanO = row.ToDecimal("PlanO"),
                            Mee = row.ToDecimal("Mee"),
                            Ekmp = row.ToDecimal("Ekmp"),
                            Penalty = row.ToDecimal("Penalty"),
                            CountRegularExpertMee = row.ToDecimal("CountRegularExpertMee"),
                            CountRegularExpertEkmp = row.ToDecimal("CountRegularExpertEkmp"),
                            CountFreelanceExpert = row.ToDecimal("CountFreelanceExpert"),
                            PaymentFreelanceExpert = row.ToDecimal("PaymentFreelanceExpert"),
                            PenaltyTfoms = row.ToDecimal("PenaltyTfoms")
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