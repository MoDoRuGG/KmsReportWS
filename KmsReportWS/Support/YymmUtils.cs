using System;
using System.Collections.Generic;

namespace KmsReportWebApi.Utils
{
    public static class YymmUtils
    {
        private static readonly Dictionary<string, string> Months = new Dictionary<string, string>
        {
             {"01","январь" },
             {"02","февраль" },
             {"03","март" },
             {"04","апрель" },
             {"05","май" },
             {"06","июнь" },
             {"07","июль" },
             {"08","август" },
             {"09","сентябрь" },
             {"10","октябрь" },
             {"11","ноябрь" },
             {"12","декабрь" },
        };

        public static string ConvertYymmToMonth(string yymm) => Months[yymm.Substring(2, 2)];
        public static int ConvertYymmToMonthInt(string yymm)
        {
            string monthString = yymm.Substring(2, 2);
            string result = monthString;          
            if (monthString[0].Equals("0"))
            {
                result = monthString.Substring(1, 1);
            }

            return Convert.ToInt32(result);
           
        }
        
        public static string ConvertYymmToText(string yymm)
        {
            string month = Months[yymm.Substring(2, 2)];
            string year = "20" + yymm.Substring(0, 2);
            
            return $"{month} {year}";
        }

        public static string GetYymmFromInt(object year, object month)
        {
            int convertedYear = Convert.ToInt32(year);
            int convertedMonth = Convert.ToInt32(month);

            string yymm = Convert.ToString(convertedYear % 2000);
            yymm += convertedMonth < 10 ? $"0{convertedMonth}" : convertedMonth.ToString();
            return yymm;
        }


        public static int ConvertYymmToYear(string yymm)
        {
                    
            int year = Convert.ToInt32("20" + yymm.Substring(0, 2));

            return year;
        }
    }
}