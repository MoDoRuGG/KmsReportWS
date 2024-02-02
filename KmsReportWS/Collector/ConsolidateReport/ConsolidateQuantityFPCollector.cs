using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Collector.ConsolidateReport
{ 
        public class ConsolidateQuantityFPCollector
        {
            public List<ConsolidateQuantityFP> Collect(string year)
            {
                List<ConsolidateQuantityFP> result = new List<ConsolidateQuantityFP>();
                try
                {
                    using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
                    {
                        connect.NewSp("p_ConsolidateQuantityPlanCheck");
                        connect.AddSpParam("@year", year);
                        var dt = connect.DataTable();

                        foreach (DataRow row in dt.Rows)
                        {
                            result.Add(new ConsolidateQuantityFP
                            {
                                RegionName = row["RegionName"].ToString(),
                                IdRegion = row["IdRegion"].ToString(),
                                Yymm = row["Yymm"].ToString(),
                                Added = (int)row["Added"],
                                Fact = (int)row["Fact"],
                                Plan = (int)row["Plan"],
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