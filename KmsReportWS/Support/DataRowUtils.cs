using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KmsReportWS.Support
{
    public static class DataRowUtils
    {
        public static decimal? ToDecimalNullable(this DataRow row, string columnName)
        {
            decimal? result = null;

            if (row[columnName] != DBNull.Value)
                result = Convert.ToDecimal(row[columnName]);

            return result;
        }

        public static int? ToIntNullable(this DataRow row, string columnName)
        {
            int? result = null;

            if (row[columnName] != DBNull.Value)
                result = Convert.ToInt32(row[columnName]);

            return result;
        }

        public static decimal ToDecimal(this DataRow row, string columnName)
        {
            decimal result = 0.00m;

            if (row[columnName] != DBNull.Value)
               result = Convert.ToDecimal(row[columnName]); 

            return result;
        }
    }
}