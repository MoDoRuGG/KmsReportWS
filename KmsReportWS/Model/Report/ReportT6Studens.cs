using System.Collections.Generic;


namespace KmsReportWS.Model.Report
{
    public class ReportT6Students : AbstractReport
    {
        public List<ReportT6StudentsDto> ReportDataList;
    }

    public class ReportT6StudentsDto
    {
        public string Theme { get; set; }
        public ReportT6StudentsDataDto Data { get; set; }
    }
    public class ReportT6StudentsDataDto
    {
        public int Id { get; set; }
        public int CountUniversity { get; set; }
        public int CountCollege { get; set; }
        public int CountInsured { get; set; }
        public string Comments { get; set; }
    }
}