using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model
{
    [DataContract]
    public class ZpzYearDataRow
    {
        [DataMember]
        public string RowNum { get; set; }

        [DataMember]
        public ReportZpz2025DataDto Data { get; set; }
    }
}