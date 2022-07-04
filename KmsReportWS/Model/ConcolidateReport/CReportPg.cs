namespace KmsReportWS.Model.ConcolidateReport
{
    public class CReportPg
    {
        public string Filial { get; set; }
        public PgExpertise Expertise { get; set; }
        public PgNormative Normative { get; set; }
        public PgFinance Finance { get; set; }
        public PgPersonnel Personnel { get; set; }
    }

    public class PgExpertise
    {
        public decimal Bills { get; set; }
        public decimal BillsOnco { get; set; }
        public decimal BillsVioletion { get; set; }
        public decimal PaymentBills { get; set; }
        public decimal PaymentBillsOnco { get; set; }
        public decimal MeeTarget { get; set; }
        public decimal MeePlan { get; set; }
        public decimal CaseMeeTarget { get; set; }
        public decimal CaseMeePlan { get; set; }
        public decimal DefectMeeTarget { get; set; }
        public decimal DefectMeePlan { get; set; }
        public decimal EkmpTarget { get; set; }
        public decimal EkmpPlan { get; set; }
        public decimal ThemeCaseEkmpPlan { get; set; }
        public decimal CaseEkmpTarget { get; set; }
        public decimal CaseEkmpPlan { get; set; }
        public decimal DefectEkmpTarget { get; set; }
        public decimal DefectEkmpPlan { get; set; }
    }

    public class PgNormative
    {
        public decimal BillsOutMo { get; set; }
        public decimal MeeOutMoPlan { get; set; }
        public decimal MeeOutMoTarget { get; set; }
        public decimal BillsApp { get; set; }
        public decimal MeeAppPlan { get; set; }
        public decimal MeeAppTarget { get; set; }
        public decimal BillsDayHosp { get; set; }
        public decimal MeeDayHospPlan { get; set; }
        public decimal MeeDayHospTarget { get; set; }
        public decimal BillsHosp { get; set; }
        public decimal MeeHospPlan { get; set; }
        public decimal MeeHospTarget { get; set; }
        public decimal EkmpOutMoPlan { get; set; }
        public decimal EkmpOutMoTarget { get; set; }
        public decimal EkmpAppPlan { get; set; }
        public decimal EkmpAppTarget { get; set; }
        public decimal EkmpDayHospPlan { get; set; }
        public decimal EkmpDayHospTarget { get; set; }
        public decimal EkmpHospPlan { get; set; }
        public decimal EkmpHospTarget { get; set; }
    }

    public class PgFinance
    {
        public decimal SumPayment { get; set; }
        public decimal SumNotPayment { get; set; }
        public decimal SumMek { get; set; }
        public decimal SumMee { get; set; }
        public decimal SumEkmp { get; set; }
    }

    public class PgPersonnel
    {
        public decimal Specialist { get; set; }
        public decimal MekFullTime { get; set; }
        public decimal MekRemote { get; set; }
        public decimal ExpertsFullTime { get; set; }
        public decimal ExpertsRemote { get; set; }
        public decimal ExpertsEkmpRegion { get; set; }
        public decimal ExpertsEkmpRemote { get; set; }
        public decimal ExpertsEkmpRegionOnko { get; set; }
        public decimal ExpertsEkmpRemoteOnko { get; set; }
        public decimal ExpertsEkmpRegister { get; set; }
        public decimal ExpertsEkmpRegisterRemote { get; set; }
        public decimal ExpertsEkmpRegisterOnko { get; set; }
        public decimal ExpertsEkmpRegisterRemoteOnko { get; set; }
        public decimal ExpertsOmsFullTime { get; set; }
        public decimal ExpertsOmsRemote { get; set; }
        public decimal ExpertsOmsEkmpFullTime { get; set; }
        public decimal ExpertsOmsEkmpRemote { get; set; }
    }
}