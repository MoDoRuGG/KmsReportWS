using System.Collections.Generic;

namespace KmsReportWS.Model.Report
{
    public class ReportPg : AbstractReport
    {
        public List<ReportPgDto> ReportDataList { get; set; }
    }

    public class ReportPgDto
    {
        public string Theme { get; set; }
        public List<ReportPgDataDto> Data { get; set; }
    }

    public class ReportPgDataDto
    {
        public string Code { get; set; }
        public decimal CountSmo { get; set; }
        public decimal CountSmoAnother { get; set; }

        public decimal CountInsured { get; set; }
        public decimal CountInsuredRepresentative { get; set; }
        public decimal CountTfoms { get; set; }
        public decimal CountProsecutor { get; set; }

        public decimal CountOutOfSmo { get; set; }
        public decimal CountAmbulatory { get; set; }
        public decimal CountDs { get; set; }
        public decimal CountDsVmp { get; set; }
        public decimal CountStac { get; set; }
        public decimal CountStacVmp { get; set; }
        public decimal CountOutOfSmoAnother { get; set; }
        public decimal CountAmbulatoryAnother { get; set; }
        public decimal CountDsAnother { get; set; }
        public decimal CountDsVmpAnother { get; set; }
        public decimal CountStacAnother { get; set; }
        public decimal CountStacVmpAnother { get; set; }
    }
}