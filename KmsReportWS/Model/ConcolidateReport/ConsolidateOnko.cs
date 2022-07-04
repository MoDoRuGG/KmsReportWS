namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateOnko
    {
        public string Filial { get; set; }
        public OnkoComplaint Complaint { get; set; }
        public OnkoProtection Protection { get; set; }
        public OnkoMek Mek { get; set; }
        public OnkoMee Mee { get; set; }
        public OnkoEkmp Ekmp { get; set; }
        public OnkoFinance Finance { get; set; }

    }

    public class OnkoComplaint
    {
        public decimal MedicalHelp { get; set; }
        public decimal Dedlines { get; set; }
        public decimal MedicineProvision { get; set; }
        public decimal DedlineDrugMedicine { get; set; }
        public decimal NoDrugMedicine { get; set; }
        public decimal AppealMedicalHelp { get; set; }
        public decimal AppealMedicineProvision { get; set; }
        public decimal AppealDrugsMedicine { get; set; }
    }

    public class OnkoProtection
    {
        public decimal PretrialMedicalHelp { get; set; }
        public decimal PretrialDeadline { get; set; }
        public decimal PretrialMedicineProvision { get; set; }
        public decimal PretrialDedlineDrugMedicine { get; set; }
        public decimal PretrialNoDrugMedicine { get; set; }
        public decimal JudicalMedicalHelp { get; set; }
        public decimal JudicalDeadline { get; set; }
        public decimal JudicalMedicineProvision { get; set; }
        public decimal JudicalDedlineDrugMedicine { get; set; }
        public decimal JudicalNoDrugMedicine { get; set; }
    }

    public class OnkoMek
    {
        public decimal PresentedBills { get; set; }
        public decimal AcceptedBills { get; set; }
        public decimal RegistrationMek { get; set; }
        public decimal NotInProgramMek { get; set; }
        public decimal TarifMek { get; set; }
        public decimal LicenceMek { get; set; }
        public decimal RepeatMek { get; set; }
    }

    public class OnkoMee
    {
        public decimal Complaint { get; set; }
        public decimal Antitumor { get; set; }
        public decimal PlanHosp { get; set; }
        public decimal ViolationCondition { get; set; }
        public decimal ViolationOnkoFirst { get; set; }
        public decimal ViolationHisto { get; set; }
        public decimal ViolationOnkoDiagnostic { get; set; }
    }

    public class OnkoEkmp
    {
        public decimal EkmpComplaint { get; set; }
        public decimal EkmpFromMee { get; set; }
        public decimal DeathEkmp { get; set; }
        public decimal ThematicEkmp { get; set; }
        public decimal CountOnko { get; set; }
        public decimal NoProfilOnko { get; set; }
        public decimal UnreasonEkmp { get; set; }
        public decimal DispEkmp { get; set; }
        public decimal RecommendationEkmp { get; set; }
        public decimal Premature { get; set; }
        public decimal ViolationMoEmkp { get; set; }
        public decimal Failure { get; set; }
        public decimal Payment { get; set; }
        public decimal OtherViolation { get; set; }
    }

    public class OnkoFinance
    {
        public decimal SumMek { get; set; }
        public decimal SumDispMee { get; set; }
        public decimal SumMee { get; set; }
        public decimal SumDispEkmp { get; set; }
        public decimal SumNoProfilEkmp { get; set; }
        public decimal SumUnreasonEkmp { get; set; }
        public decimal SumRecommendationEkmp { get; set; }
        public decimal SumPrematureEkmp { get; set; }
        public decimal SumFailureEkmp { get; set; }
        public decimal SumPaymentEkmp { get; set; }
        public decimal SumOtherEkmp { get; set; }
    }
}