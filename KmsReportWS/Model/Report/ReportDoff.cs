using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportDoff: AbstractReport
    {
        public List<ReportDoffDto> ReportDataList { get; set; }
    }
    public class ReportDoffDto
    {
        public string Theme { get; set; }
        public List<ReportDoffDataDto> Data { get; set; }
    }

    public class ReportDoffDataDto
    {
        public string RowNum { get; set; }
        public string Column1 { get; set; }
        public string Column2 { get; set; }
        public string Column3 { get; set; }
    }
}