using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model.Constructor
{
    public class ReportDynamicFlowDto
    {

        public int IdReport { get; set; }
        public int IdFlow { get; set; }
        public string IdRegion { get; set; }
        public ReportStatus Status { get; set; }
        public DateTime Created { get; set; }
        public int IdUser { get; set; }

    }
}