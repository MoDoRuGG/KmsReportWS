namespace KmsReportWS.Model.ConcolidateReport
{
    public class CReportZpz2023Full
    {
        public string Filial { get; set; }
        public ZpzExpertise2023Full Expertise1Q { get; set; }
        public ZpzExpertise2023Full Expertise2Q { get; set; }
        public ZpzExpertise2023Full Expertise3Q { get; set; }
        public ZpzExpertise2023Full Expertise4Q { get; set; }
        public ZpzFinance2023Full Finance1Q { get; set; }
        public ZpzFinance2023Full Finance2Q { get; set; }
        public ZpzFinance2023Full Finance3Q { get; set; }
        public ZpzFinance2023Full Finance4Q { get; set; }

    }

    public class ZpzExpertise2023Full
    {
        public decimal Bills { get; set; }
        public decimal CountMeeTarget { get; set; }
        public decimal CountMeePlan { get; set; }
        public decimal CountMeeComplaintTarget { get; set; }
        public decimal CountMeeComplaintPlan { get; set; }
        public decimal CountMeeRepeat { get; set; }
        public decimal CountMeeOnco { get; set; }
        public decimal CountMeeDs { get; set; }
        public decimal CountMeeLeth { get; set; }
        public decimal CountMeeInjured { get; set; }
        public decimal CountMeeDefectedCaseTarget { get; set; }
        public decimal CountMeeDefectedCasePlan { get; set; }
        public decimal CountMeeDefectsTarget { get; set; }
        public decimal CountMeeDefectsPlan { get; set; }
        public decimal CountMeeDefectsPeriod { get; set; }
        public decimal CountMeeDefectsCondition { get; set; }
        public decimal CountMeeDefectsRepeat { get; set; }
        public decimal CountMeeDefectsOutOfDocums { get; set; }
        public decimal CountMeeDefectsUnpayable { get; set; }
        public decimal CountMeeDefectsBuyMedicament { get; set; }
        public decimal CountMeeDefectsOutOfLeth { get; set; }
        public decimal CountMeeDefectsWithoutDocums { get; set; }
        public decimal CountMeeDefectsIncorrectDocums { get; set; }
        public decimal CountMeeDefectsBadDocums { get; set; }
        public decimal CountMeeDefectsBadDate { get; set; }
        public decimal CountMeeDefectsBadData { get; set; }
        public decimal CountMeeDefectsOutOfProtocol { get; set; }
        public decimal CountCaseEkmpTarget { get; set; }
        public decimal CountCaseEkmpPlan { get; set; }
        public decimal CountCaseEkmpComplaint { get; set; }
        public decimal CountCaseEkmpLeth { get; set; }
        public decimal CountCaseEkmpByMek { get; set; }
        public decimal CountCaseEkmpByMee { get; set; }
        public decimal CountCaseEkmpUTheme { get; set; }
        public decimal CountCaseEkmpMultiTarget { get; set; }
        public decimal CountCaseEkmpMultiPlan { get; set; }
        public decimal CountCaseEkmpMultiLeth { get; set; }
        public decimal CountCaseEkmpMultiUthemeTarget { get; set; }
        public decimal CountCaseEkmpMultiUthemePlan { get; set; }
        public decimal CountCaseDefectedBySmoTarget { get; set; }
        public decimal CountCaseDefectedBySmoPlan { get; set; }
        public decimal CountEkmpDefectedCaseTarget { get; set; }
        public decimal CountEkmpDefectedCasePlan { get; set; }
        public decimal CountEkmpBadDs { get; set; }
        public decimal CountEkmpBadDsNotAffected { get; set; }
        public decimal CountEkmpBadDsProlonger { get; set; }
        public decimal CountEkmpBadDsDecline { get; set; }
        public decimal CountEkmpBadDsInjured { get; set; }
        public decimal CountEkmpBadDsLeth { get; set; }
        public decimal CountEkmpBadMed { get; set; }
        public decimal CountEkmpUnreglamentedMed { get; set; }
        public decimal CountEkmpStopMed { get; set; }
        public decimal CountEkmpContinuity { get; set; }
        public decimal CountEkmpUnprofile { get; set; }
        public decimal CountEkmpUnfounded { get; set; }
        public decimal CountEkmpRepeat { get; set; }
        public decimal CountEkmpDifference { get; set; }
        public decimal CountEkmpUnfoundedMedicaments { get; set; }
        public decimal CountEkmpUnfoundedReject { get; set; }
        public decimal CountEkmpDisp { get; set; }
        public decimal CountEkmpRepeat2weeks { get; set; }
        public decimal CountEkmpOutOfResults { get; set; }
        public decimal CountEkmpDoubleHospital { get; set; }

    }

    public class ZpzFinance2023Full
    {
        public decimal SumPayment { get; set; }
        public decimal SumNotPayment { get; set; }
        public decimal SumMek { get; set; }
        public decimal SumMee { get; set; }
        public decimal SumEkmp { get; set; }
    }
}