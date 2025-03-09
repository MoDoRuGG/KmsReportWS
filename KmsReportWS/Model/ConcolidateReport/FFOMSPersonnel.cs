using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSPersonnel
    {
        public string Filial { get; set; }
        public List<PersonnelT9> PersonnelT9 { get; set; }
    }

    public class PersonnelT9
    {
        public string Row { get; set; }
        public int FullTime { get; set; }
        public int Contract { get; set; }
    }
}