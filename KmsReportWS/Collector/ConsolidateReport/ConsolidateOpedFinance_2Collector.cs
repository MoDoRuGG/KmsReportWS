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
    public class ConsolidateOpedFinance_2Collector
    {

        public List<ConsolidateOpedFinance_2> Collect(string year)
        {
            List<ConsolidateOpedFinance_2> result = new List<ConsolidateOpedFinance_2>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_ConsolidateOpedFinance_2");
                connect.AddSpParam("@year", year);
                using (var dt = connect.DataTable())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateOpedFinance_2 
                        {
                            IdRegion = row["IdRegion"].ToString(),
                            RegionName = row["RegionName"].ToString(),

                            Fact1 = row.ToDecimalNullable("Fact1"),
                            Plan1 = row.ToDecimalNullable("Plan1"),

                            Fact2 = row.ToDecimalNullable("Fact2"),
                            Plan2 = row.ToDecimalNullable("Plan2"),

                            Fact3 = row.ToDecimalNullable("Fact3"),
                            Plan3 = row.ToDecimalNullable("Plan3"),

                            Fact4 = row.ToDecimalNullable("Fact4"),
                            Plan4 = row.ToDecimalNullable("Plan4"),

                            Fact5 = row.ToDecimalNullable("Fact5"),
                            Plan5 = row.ToDecimalNullable("Plan5"),

                            Fact6 = row.ToDecimalNullable("Fact6"),
                            Plan6 = row.ToDecimalNullable("Plan6"),

                            Fact7 = row.ToDecimalNullable("Fact7"),
                            Plan7 = row.ToDecimalNullable("Plan7"),

                            Fact8 = row.ToDecimalNullable("Fact8"),
                            Plan8 = row.ToDecimalNullable("Plan8"),

                            Fact9 = row.ToDecimalNullable("Fact9"),
                            Plan9 = row.ToDecimalNullable("Plan9"),

                            Fact10 = row.ToDecimalNullable("Fact10"),
                            Plan10 = row.ToDecimalNullable("Plan10"),

                            Fact11 = row.ToDecimalNullable("Fact11"),
                            Plan11 = row.ToDecimalNullable("Plan11"),

                            Fact12 = row.ToDecimalNullable("Fact12"),
                            Plan12 = row.ToDecimalNullable("Plan12")

                        });
                    }
                }
            }

            return result;
        }
    }
}