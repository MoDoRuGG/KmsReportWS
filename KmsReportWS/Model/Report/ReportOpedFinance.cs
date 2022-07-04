using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportOpedFinance : AbstractReport
    {
        public int IdReportData { get; set; }

        public List<ReportOpedFinanceData> ReportDataList { get; set; }

    }


    public class ReportOpedFinanceData
    {
        public string RowNum { get; set; }

        public decimal? ValueFact { get; set; }

        public string Notes { get; set; }
    }
}