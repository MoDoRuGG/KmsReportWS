using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateTable5NewbornCollector
    {
        public List<ConsolidateTable5Newborn> Collect(string yymm)
        {
            List<ConsolidateTable5Newborn> result = new List<ConsolidateTable5Newborn>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_ConsolidateTable5Newborn");
                connect.AddSpParam("@yymm", yymm);
                var dt = connect.DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateTable5Newborn
                        {
                            RegionName = row["RegionName"] != DBNull.Value ? row["RegionName"].ToString() : string.Empty,
                            MarketShare = GetDecimal(row, "MarketShare"),
                            CountNewborn = GetDecimal(row, "CountNewborn"),
                            CountMaterinityBills = GetDecimal(row, "CountMaterinityBills"),
                            ShareFromRegister = GetDecimal(row, "ShareFromRegister"),
                            DeviationFromRegister = GetDecimal(row, "DeviationFromRegister")
                        });
                    }
                }
            }
            return result;
        }

        // Вспомогательный метод для безопасного приведения
        private decimal GetDecimal(DataRow row, string columnName)
        {
            if (row[columnName] == DBNull.Value) return 0m;

            if (decimal.TryParse(row[columnName].ToString(), out var result))
            {
                return result;
            }

            return 0m; // Или выбросить исключение, если ошибка недопустима
        }
    }
}