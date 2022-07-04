using System.Collections.Generic;
using KmsReportWS.Model.Constructor;

namespace KmsReportWS.Model.Service
{
    public class ClientContext
    {
        public List<KmsReportDictionary> Regions { get; set; }
        public List<KmsReportDictionary> ReportTypes { get; set; }
        public List<KmsReportDictionary> Users { get; set; }
        public List<KmsReportDictionary> Emails { get; set; }
        public List<HeadCompany> Heads { get; set; }
    }
}