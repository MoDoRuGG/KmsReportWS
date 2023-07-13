using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportOpedFinance3 : AbstractReport
    {
        public int IdReportData { get; set; }

        public List<ReportOpedFinance3Data> ReportDataList { get; set; }

    }


    public class ReportOpedFinance3Data
    {
        public string RowNum { get; set; }

        public decimal? ValueFact { get; set; }

        public string Notes { get; set; }
    }
}