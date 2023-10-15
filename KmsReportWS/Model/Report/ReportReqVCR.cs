using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class ReportReqVCR : AbstractReport
    {
        public List<ReportReqVCRDto> ReportDataList { get; set; }
    }

    public class ReportReqVCRDto
    {
        public string Theme { get; set; }
        public List<ReportReqVCRDataDto> Data { get; set; }
    }

    public class ReportReqVCRDataDto
    {
        public int Id { get; set; }
        public string RowNum { get; set; }
        public decimal y2019 { get; set;}
        public decimal y2020 { get; set; }
        public decimal y2021 { get; set;}
        public decimal y2022 { get; set;}
        public decimal y2023 { get; set;}

    }
}