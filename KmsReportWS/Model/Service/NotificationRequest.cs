namespace KmsReportWS.Model.Service
{
    public class NotificationRequest
    {
        public string[] Filials { get; set; }
        public string ReportType { get; set; }
        public string Yymm { get; set; }
        public bool IsRefuse { get; set; }
        public string Theme { get; set; }
        public string Text { get; set; }
        public string CurrentEmail { get; set; }
    }
}