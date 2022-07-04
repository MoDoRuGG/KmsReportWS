namespace KmsReportWS.Model.ConcolidateReport
{
    public class SummaryPg
    {
        public string Filial { get; set; }
        public string Theme { get; set; }
        public string RowNum { get; set; }

        public decimal SumSmo { get; set; }
        public decimal SumSmoAnother { get; set; }

        public decimal SumOutOfSmo { get; set; }
        public decimal SumAmbulatory { get; set; }
        public decimal SumDs { get; set; }
        public decimal SumStac { get; set; }

        public decimal SumOutOfSmoAnother { get; set; }
        public decimal SumAmbulatoryAnother { get; set; }
        public decimal SumDsAnother { get; set; }
        public decimal SumStacAnother { get; set; }

        public decimal SumInsured { get; set; }
        public decimal SumInsuredRepresentative { get; set; }
        public decimal SumTfoms { get; set; }
        public decimal SumProsecutor { get; set; }
        public decimal SumVmp { get; set; }


        public decimal SumVidpom { 
            get => SumOutOfSmo + SumAmbulatory + SumDs + SumStac;
        }

        public decimal SumVidpomGr3 {
            get => SumOutOfSmo + SumAmbulatory + SumDs + SumStac + SumVmp;
        }
        public decimal SumVidpomAnother {
            get => SumOutOfSmoAnother + SumAmbulatoryAnother + SumDsAnother + SumStacAnother;
        }

        public decimal SumConflict {
            get => SumInsured + SumInsuredRepresentative + SumTfoms + SumProsecutor;
        }
      
    }
}