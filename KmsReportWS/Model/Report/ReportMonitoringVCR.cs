using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportMonitoringVCR : AbstractReport
    {

        public int IdReportData { get; set; }

        public List<MonitoringVCRData> Data { get; set; }

        public ReportMonitoringVCR()
        {
            Data = new List<MonitoringVCRData>();
        }

    }

    public class MonitoringVCRData
    {
        public int IdMonitoringVCR { get; set; }
        public string RowNum { get; set; }

        public decimal? ExpertWithEducation { get; set; }

        public decimal? ExpertWithoutEducation { get; set; }

        public decimal? Total { get; set; }
    }

}