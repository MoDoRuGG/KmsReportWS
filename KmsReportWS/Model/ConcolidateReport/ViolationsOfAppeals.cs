using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ViolationsOfAppeals
    {
        public string Filial { get; set; }
        public List<ForT1VOA> T1 { get; set; }
        public List<ForT2VOA> T2 { get; set; }
        public List<ForT3VOA> T3 { get; set; }
    }

    public class ForT1VOA
    {
        public string Row { get; set; }
        public int Oral { get; set; }
        public int Written { get; set; }
        public int Assignment { get; set; }
    }

    public class ForT2VOA
    {
        public string Row { get; set; }
        public int Target { get; set; }
        public int Plan { get; set; }
        public int Violation { get; set; }
    }

    public class ForT3VOA
    {
        public string Row { get; set; }
        public int Target { get; set; }
        public int Plan { get; set; }
        public int Violation { get; set; }
    }
}