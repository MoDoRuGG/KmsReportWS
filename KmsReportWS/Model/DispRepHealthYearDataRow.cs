using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model
{
    [DataContract]
    public class DispRepHealthYearDataRow
    {
        [DataMember]
        public string RowNum { get; set; }

        [DataMember]
        public ReportDispReprodHealthDataDto Data { get; set; }
    }
}