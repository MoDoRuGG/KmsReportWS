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
    public class ConsolidateProposalCollector
    {
        public List<ConsolidateProposal> Collect(string yymm)
        {
            List<ConsolidateProposal> result = new List<ConsolidateProposal>();
            using (MsConnection connection = new MsConnection(Settings.Default.ConnStr))
            {
                connection.NewSp("p_ConsolidateProposal");
                connection.AddSpParam("@yymm", yymm);
                using (var dt = connection.DataTable())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateProposal
                        {
                            RegionId = row["id_region"].ToString(),
                            RegionName = row["name_region"].ToString(),
                            CountMoCheck = row.ToIntNullable("Count_MoCheck"),
                            CountMoCheckWithDefect = row.ToIntNullable("Count_MoCheckWithDefect"),
                            CountProporsals = row.ToIntNullable("Count_Proporsals"),
                            CountProporsalsWithDefect = row.ToIntNullable("Count_ProporsalsWithDefect"),
                            Notes = row["Notes"].ToString()
                        });
                    }
                }
            }

            return result;
        }
    }
}