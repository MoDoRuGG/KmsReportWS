using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class Report262 : AbstractReport
    {
        public List<Report262Dto> ReportDataList { get; set; }
    }

    public class Report262Dto
    {
        public string Theme { get; set; }
        public List<Report262DataDto> Data { get; set; }
        public List<Report262Table3Data> Table3 { get; set; }
    }

    public class Report262DataDto
    {
        public int RowNum { get; set; }
        public int CountPpl { get; set; }
        public int CountPplFull { get; set; }
        public int CountSms { get; set; }
        public int CountPost { get; set; }
        public int CountPhone { get; set; }
        public int CountMessengers { get; set; }
        public int CountEmail { get; set; }
        public int CountAddress { get; set; }
        public int CountAnother { get; set; }
    }

    public class Report262Table3Data
    {
        public string Mo { get; set; }
        public int CountUnit { get; set; }
        public int CountUnitChild { get; set; }
        public int CountUnitWithSp { get; set; }
        public int CountUnitWithSpChild { get; set; }
        public int CountChannelSp { get; set; }
        public int CountChannelSpChild { get; set; }
        public int CountChannelPhone { get; set; }
        public int CountChannelPhoneChild { get; set; }
        public int CountChannelTerminal { get; set; }
        public int CountChannelTerminalChild { get; set; }
        public int CountChannelAnother { get; set; }
        public int CountChannelAnotherChild { get; set; }
    }
}