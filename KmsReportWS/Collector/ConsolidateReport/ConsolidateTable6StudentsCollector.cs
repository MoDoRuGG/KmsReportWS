using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateTable6StudentsCollector
    {
        public List<ConsolidateTable6Students> Collect(string yymm)
        {
            List<ConsolidateTable6Students> result = new List<ConsolidateTable6Students>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_ConsolidateTable6Students");
                connect.AddSpParam("@yymm", yymm);
                var dt = connect.DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateTable6Students
                        {
                            IdRegion = row["Id_Region"].ToString(),
                            RegionName = row["RegionName"].ToString(),
                            CountUniversity = Convert.ToDecimal(row["CountUniversity"]),
                            CountCollege = Convert.ToDecimal(row["CountCollege"]),
                            CountInsured = Convert.ToDecimal(row["CountInsured"]),
                            Comments = row["Comments"].ToString()
                           
                        });
                    }
                }
            }
            return result;
        }
    }
}