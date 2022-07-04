using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportInfrormationResponse : AbstractReport
    {
        public List<ReportInfrormationResponseDto> ReportDataList;
    }

    public class ReportInfrormationResponseDto
    {
        public string Theme { get; set; }

        public ReportInfrormationResponseDataDto Data { get; set; }
    }

    public class ReportInfrormationResponseDataDto
    {
        public int Id { get; set; }

        public int Plan { get; set; }

        public int Informed { get; set; }

        public int CountRegistry { get; set; }

        public int CountPast { get; set; }


    }


}