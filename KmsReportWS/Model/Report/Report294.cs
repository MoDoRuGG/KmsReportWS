using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class Report294 : AbstractReport
    {
        public List<Report294Dto> ReportDataList { get; set; }
    }

    public class Report294Dto
    {
        public string Theme { get; set; }
        public List<Report294DataDto> Data { get; set; }
    }

    public class Report294DataDto
    {
        public string RowNum { get; set; }
        public int CountPpl { get; set; }
        public int CountSms { get; set; }
        public int CountPost { get; set; }
        public int CountPhone { get; set; }
        public int CountMessengers { get; set; }
        public int CountEmail { get; set; }
        public int CountAddress { get; set; }
        public int CountAnother { get; set; }
        public int CountOncologicalDisease { get; set; }
        public int CountEndocrineDisease { get; set; }
        public int CountBronchoDisease { get; set; }
        public int CountBloodDisease { get; set; }
        public int CountAnotherDisease { get; set; }
    }
}