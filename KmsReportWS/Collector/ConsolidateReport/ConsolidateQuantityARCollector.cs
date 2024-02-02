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
        public class ConsolidateQuantityARCollector
        {
            public List<ConsolidateQuantityAR> Collect(string year)
            {
                List<ConsolidateQuantityAR> result = new List<ConsolidateQuantityAR>();
                try
                {
                    using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
                    {
                        connect.NewSp("p_ConsolidateQuantityAddRemove");
                        connect.AddSpParam("@year", year);
                        var dt = connect.DataTable();

                        foreach (DataRow row in dt.Rows)
                        {
                            result.Add(new ConsolidateQuantityAR
                            {
                                RegionName = row["RegionName"].ToString(),
                                IdRegion = row["IdRegion"].ToString(),
                                Yymm = row["Yymm"].ToString(),
                                Added = (int)row["Added"],
                                Removed = (int)row["Removed"],
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