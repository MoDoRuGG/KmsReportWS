using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.Model;
using KmsReportWS.Properties;
using KmsReportWS.Support;

namespace KmsReportWS.Handler
{
    /// <summary>
    ///Общий класс для получения информации, которая нужна в запросах
    /// </summary>
    public class DynamicReportCommonHandler
    {
        public CheckFFOMS2022CommonData GetCheckFFOMS2022CommonData(string year, string idRegion)
        {
            CheckFFOMS2022CommonData result = new CheckFFOMS2022CommonData();
            using (MsConnection connect = new MsConnection(Settings.Default.ConnStr))
            {
                connect.NewSp("p_DynamicReport_GetBaseReportData");
                connect.AddSpParam("@mode", 1);
                connect.AddSpParam("@year", year);
                connect.AddSpParam("@id_region", idRegion);
                using(var dt  = connect.DataTable())
                {
                    if(dt.Rows.Count > 0)
                    {
                        var row = dt.Rows[0];
                        result.CountLetalAll = row.ToDecimalNullable("CountLetalAll");
                        result.CountEkmp = row.ToDecimalNullable("CountEkmp");
                        result.CountNarush = row.ToDecimalNullable("CountNarush");
                        result.CountNeProvedenaOb = row.ToDecimalNullable("CountNeProvedenaOb");
                    }
                }
            }

            return result;
        }
    }
}