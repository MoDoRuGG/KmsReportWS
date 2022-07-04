using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.ConcolidateReport;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;
using KmsReportWS.Support;
using Microsoft.SqlServer.Server;

namespace KmsReportWS.Collector.ConsolidateReport
{
    public class ConsolidateOpedCollector
    {    
        public List<ConsolidateOped> Collect(string yymmStart, string yymmEnd, List<string> regions)
        {
            var result = new List<ConsolidateOped>();
            DataTable dbData = new DataTable();          
            using var db = new LinqToSqlKmsReportDataContext(Settings.Default.ConnStr) { CommandTimeout = 120 };

         
            DataTable paramTable = new DataTable();
            paramTable.Columns.Add("str", typeof(string));

            foreach (string reg in regions)
            {
                var row = paramTable.NewRow();
                row["str"] = reg;
                paramTable.Rows.Add(row);
            }

            MsConnection connection = new MsConnection(Settings.Default.ConnStr);
            connection.NewSp("p_OpedConsolidateReport");
            connection.AddSpParam("@yymm_start", yymmStart);
            connection.AddSpParam("yymm_end", yymmEnd);
            connection.AddSpParam("@regions", paramTable);
            dbData = connection.DataTable();



            var dbAnumerable = dbData.AsEnumerable();
            var regUnique = dbAnumerable.Select(x => x["region"].ToString()).Distinct().ToList();

            foreach(var reg in regUnique)
            {
                var regData = dbAnumerable.Where(x => x["region"].ToString() == reg).ToList();
                var consolidateOped = new ConsolidateOped();
                var ekmp = new OpedData(regData[1]);
                var mee = new OpedData(regData[0]);
                consolidateOped.Filial = reg;
                consolidateOped.Ekmp = ekmp;
                consolidateOped.Mee = mee;

                result.Add(consolidateOped);

            }

            return result;

        }

    }

}