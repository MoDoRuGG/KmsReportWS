using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateVCRReportCollector
    {
        public List<ConsolidateVCR> Collect(string yymm)
        {
            List<ConsolidateVCR> result = new List<ConsolidateVCR>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_VCRMonitoring_Svod");
                connect.AddSpParam("@yymm", yymm);
                var dt = connect.DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateVCR
                        {
                            RowNum = row["RowNum"].ToString(),
                            ExpertWithEducation = Convert.ToDecimal(row["ExpertWithEducation"]),
                            ExpertWithoutEducation = Convert.ToDecimal(row["ExpertWithoutEducation"]),
                            // Тут тянутся итоги по форме ПГ, но оказывается что так теперь не надо, поэтому не ломая ничего я просто комменчу//
                            // Total = Convert.ToDecimal(row["Total"])
                            // Вместо этого делаю //
                            Total = Convert.ToDecimal(row["ExpertWithEducation"]) + Convert.ToDecimal(row["ExpertWithoutEducation"]),
                        });
                    }
                }

            }

            return result;
        }
    }
}