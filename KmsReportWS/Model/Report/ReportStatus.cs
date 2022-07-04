using System.ComponentModel;

namespace KmsReportWS.Model.Report
{
    public enum ReportStatus
    {
        [Description("New")] New,
        [Description("Saved")] Saved,
        [Description("Scan")] Scan,
        [Description("Submit")] Submit,
        [Description("Refuse")] Refuse,
        [Description("Done")] Done
    }
}