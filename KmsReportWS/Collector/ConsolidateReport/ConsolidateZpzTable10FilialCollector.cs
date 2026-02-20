using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateZpzTable10FilialCollector
    {
        public List<ConsolidateZpzTable10Filial> Collect(string yymm)
        {
            List<ConsolidateZpzTable10Filial> result = new List<ConsolidateZpzTable10Filial>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_Zpz10_2025_Svod");
                connect.AddSpParam("@yymm", yymm);
                var dt = connect.DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateZpzTable10Filial
                        {
                            RowNum = row["RowNum"].ToString(),
                            Yearly = Convert.ToDecimal(row["Yearly"]),
                            ByMonth = Convert.ToDecimal(row["ByMonth"])
                        });
                    }
                }
            }
            return result;
        }
    }
}