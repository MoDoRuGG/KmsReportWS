using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.Constructor
{
    public class ReportDynamicDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        public bool IsUserRow { get; set; }
        public DateTime Date { get; set; }
        public int UserCreated { get; set; }
        

    }
}