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
        public class ConsolidateQuantityQCollector
        {
            public List<ConsolidateQuantityQ> Collect(string yymm)
            {
                List<ConsolidateQuantityQ> result = new List<ConsolidateQuantityQ>();
                try
                {
                    using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
                    {
                        connect.NewSp("p_QuantityConsQ");
                        connect.AddSpParam("@yymm", yymm);
                        var dt = connect.DataTable();

                        foreach (DataRow row in dt.Rows)
                        {
                            result.Add(new ConsolidateQuantityQ
                            {
                                RegionName = row["RegionName"].ToString(),
                                IdRegion = row["IdRegion"].ToString(),
                                Yymm = row["Yymm"].ToString(),
                                c2 = (int)row["c2"],
                                c3 = (int)row["c3"],
                                c4 = (int)row["c4"],
                                c5 = (int)row["c5"],
                                c6 = (int)row["c6"],
                                c7 = (int)row["c7"],
                                c8 = (int)row["c8"],
                                c9 = (int)row["c9"]

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