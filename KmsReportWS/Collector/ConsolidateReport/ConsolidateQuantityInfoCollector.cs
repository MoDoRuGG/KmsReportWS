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
        public class ConsolidateQuantityInfoCollector
        {
            public List<ConsolidateQuantityInfo> Collect(string year)
            {
                List<ConsolidateQuantityInfo> result = new List<ConsolidateQuantityInfo>();
                try
                {
                    using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
                    {
                        connect.NewSp("p_ConsolidateQuantityInformation");
                        connect.AddSpParam("@year", year);
                        var dt = connect.DataTable();

                        foreach (DataRow row in dt.Rows)
                        {
                            result.Add(new ConsolidateQuantityInfo
                            {
                                RegionName = row["RegionName"].ToString(),
                                IdRegion = row["IdRegion"].ToString(),
                                Yymm = row["Yymm"].ToString(),
                                Added = (int)row["Added"],
                                Fact = (int)row["Fact"],
                                Plan = (int)row["Plan"],
                                Col_1 = (int)row["Col_1"],
                                Col_3 = (int)row["Col_3"],
                                Col_7 = (int)row["Col_7"],
                                Col_8 = (int)row["Col_8"],
                                Col_10 = (int)row["Col_10"],
                                Col_12 = (int)row["Col_12"],
                                Col_15 = (int)row["Col_15"]
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