using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateTable7OldPolisCollector
    {
        public List<ConsolidateTable7OldPolis> Collect(string yymm)
        {
            List<ConsolidateTable7OldPolis> result = new List<ConsolidateTable7OldPolis>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_ConsolidateTable7OldPolis");
                connect.AddSpParam("@yymm", yymm);
                var dt = connect.DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateTable7OldPolis
                        {
                            RegionName = row["RegionName"].ToString(),
                            CountConstant2019 = row["CountConstant2019"] == DBNull.Value ? 0 : Convert.ToInt32(row["CountConstant2019"]),
                            CountYearStart = row["CountYearStart"] == DBNull.Value ? 0 : Convert.ToInt32(row["CountYearStart"]),
                            CurrentQuantity = row["CurrentQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(row["CurrentQuantity"]),
                            CountOldPolis = row["CountOldPolis"] == DBNull.Value ? 0 : Convert.ToInt32(row["CountOldPolis"]),
                            ShareFromQuantity = row["ShareFromQuantity"] == DBNull.Value ? 0 : Convert.ToDecimal(row["ShareFromQuantity"]),
                            YearlyDynamic = row["YearlyDynamic"] == DBNull.Value ? 0 : Convert.ToInt32(row["YearlyDynamic"]),
                            From2019Dynamic = row["From2019Dynamic"] == DBNull.Value ? 0 : Convert.ToInt32(row["From2019Dynamic"]),
                        });
                    }
                }
            }
            return result;
        }
    }
}