﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Properties;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateVSSReportCollector
    {
        public List<ConsolidateVSS> Collect(string yymm)
        {
            List<ConsolidateVSS> result = new List<ConsolidateVSS>();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_VSSMonitoring_Svod");
                connect.AddSpParam("@yymm", yymm);
                var dt = connect.DataTable();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new ConsolidateVSS
                        {
                            RowNum = row["row_num"].ToString(),
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