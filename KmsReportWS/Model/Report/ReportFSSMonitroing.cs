using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportFSSMonitroing : AbstractReport
    {

        public int IdReportData { get; set; }

        public List<FSSMonitroingData> Data { get; set; }

        public ReportFSSMonitroing()
        {
            Data = new List<FSSMonitroingData>();
        }

    }

    public class FSSMonitroingData
    {
        public int IdFssMonitoring { get; set; }
        public string RowNum { get; set; }

        public decimal? ExpertWithEducation { get; set; }

        public decimal? ExpertWithoutEducation { get; set; }

        public decimal? Total { get; set; }
    }

}