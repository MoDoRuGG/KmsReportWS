using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportInfomaterial : AbstractReport
    {
        public int IdReportData { get; set; }

        public List<ReportInfomaterialData> ReportDataList { get; set; }

    }


    public class ReportInfomaterialData
    {
        public string RowNum { get; set; }

        public decimal? CurrentCount { get; set; }

        public decimal? YearsAmount { get; set; }
    }
}