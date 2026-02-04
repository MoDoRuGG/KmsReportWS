using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Dictionary;
using KmsReportWS.Properties;

namespace KmsReportWS.Handler
{
    public class T7OldPolisYearlyDictionaryHandler
    {
        private static readonly string ConnStr = Settings.Default.ConnStr;
        public List<T7OldPolisYearlyDictionaryItem> GetList(string year)
        {
            List<T7OldPolisYearlyDictionaryItem> result = new List<T7OldPolisYearlyDictionaryItem>();
            var db = new LinqToSqlKmsReportDataContext(ConnStr);

            int yymm = Convert.ToInt32(year.Substring(2) + "01");

            var values = db.Report_T7OldPolisYearly.Where(x => Convert.ToInt32(Convert.ToInt32(x.Yymm)) == yymm);
            if (values != null)
            {
                foreach (var value in values)
                {
                    result.Add(new T7OldPolisYearlyDictionaryItem
                    {
                        IdReportT7OldPolisYearly = value.Id_Report_T7OldPolisYearly,
                        Yymm = value.Yymm,
                        IdRegion = value.Id_Region,
                        Value = value.Value
                    });
                }
            }

            return result;
        }

        public void Save(List<T7OldPolisYearlyDictionaryItem> values)
        {
            var db = new LinqToSqlKmsReportDataContext(ConnStr);
            if (values != null)
            {
                foreach (var value in values)
                {
                    var valueInDB = db.Report_T7OldPolisYearly.FirstOrDefault(x => x.Yymm == value.Yymm && x.Id_Region == value.IdRegion);
                    if (valueInDB == null) // Создание новой записи
                    {
                        valueInDB = new Report_T7OldPolisYearly
                        {
                            Id_Region = value.IdRegion,
                            Value = value.Value,
                            Yymm = value.Yymm
                        };

                        db.Report_T7OldPolisYearly.InsertOnSubmit(valueInDB);

                    }
                    else // Редактирование
                    {
                        valueInDB.Value = value.Value;
                    }

                    db.SubmitChanges();
                }
            }
        }
    }
}