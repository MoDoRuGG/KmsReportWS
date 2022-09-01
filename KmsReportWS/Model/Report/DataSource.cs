using System.ComponentModel;

namespace KmsReportWS.Model.Report
{
    public enum DataSource
    {
        [Description("New")] New,
        [Description("Excel")] Excel,
        [Description("Handle")] Handle,
    }
}