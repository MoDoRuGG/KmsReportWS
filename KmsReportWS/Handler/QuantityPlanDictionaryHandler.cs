using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Dictionary;
using KmsReportWS.Properties;

namespace KmsReportWS.Handler
{
    public class QuantityPlanDictionaryHandler
    {
        private static readonly string ConnStr = Settings.Default.ConnStr;
        public List<QuantityPlanDictionaryItem> GetList(string year)
        {
            List<QuantityPlanDictionaryItem> result = new List<QuantityPlanDictionaryItem>();
            var db = new LinqToSqlKmsReportDataContext(ConnStr);

            int yymmStart = Convert.ToInt32(year.Substring(2) + "01");
            int yymmEnd = Convert.ToInt32(year.Substring(2) + "12");

            var plans = db.QuantityPlan.Where(x => Convert.ToInt32(Convert.ToInt32(x.Yymm)) >= yymmStart && Convert.ToInt32(x.Yymm) <= yymmEnd);
            if (plans != null)
            {
                foreach (var plan in plans)
                {
                    result.Add(new QuantityPlanDictionaryItem
                    {
                        IdQuantityPlan = plan.Id_QuantityPlan,
                        Yymm = plan.Yymm,
                        IdRegion = plan.Id_Region,
                        Value = plan.Value
                    });
                }
            }

            return result;

        }
        public void Save(List<QuantityPlanDictionaryItem> plans)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            if (plans != null)
            {
                foreach (var plan in plans)
                {
                    var planInDB = db.QuantityPlan.FirstOrDefault(x => x.Yymm == plan.Yymm && x.Id_Region == plan.IdRegion);
                    if (planInDB == null) // Создание новой записи
                    {
                        planInDB = new QuantityPlan
                        {
                            Id_Region = plan.IdRegion,
                            Value = plan.Value,
                            Yymm = plan.Yymm
                        };

                        db.QuantityPlan.InsertOnSubmit(planInDB);

                    }
                    else // Редактирование
                    {
                        planInDB.Value = plan.Value;
                    }

                    db.SubmitChanges();


                }
            }

        }
    }
}