using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateZpzTable5Collector
    {
        public List<ConsolidateZpzTable5> Collect(string yymm)
        {
            List<ConsolidateZpzTable5> result = new List<ConsolidateZpzTable5>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_ZpzQTable5");
                connect.AddSpParam("@yymm", yymm);
                var dt = connect.DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateZpzTable5
                        {
                            Filial = row["name"].ToString(),
                            RowNum = row["RowNum"].ToString(),
                            CountSmo = Convert.ToDecimal(row["CountSmo"]),
                            CountSmoAnother = Convert.ToDecimal(row["CountSmoAnother"]),
                            CountInsured = Convert.ToDecimal(row["CountInsured"]),
                            CountInsuredRepresentative = Convert.ToDecimal(row["CountInsuredRepresentative"]),
                            CountTfoms = Convert.ToDecimal(row["CountTfoms"]),
                            CountProsecutor = Convert.ToDecimal(row["CountProsecutor"]),
                            CountOutOfSmo = Convert.ToDecimal(row["CountOutOfSmo"]),
                        });
                    }
                }

            }

            return result;
        }
    }
}