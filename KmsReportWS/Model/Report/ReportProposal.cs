using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Report
{
    public class ReportProposal : AbstractReport
    {
        public int IdReportData { get; set; }

        public int? CountMoCheck { get; set; }
        public int? CountMoCheckWithDefect { get; set; }

        public int? CountProporsals { get; set; }

        public int? CountProporsalsWithDefect { get; set; }

        public string Notes { get; set; }


    }
}