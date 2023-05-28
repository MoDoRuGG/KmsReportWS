namespace KmsReportWS.Model.ConcolidateReport
{
    public class CReportZpz2023
    {
        public string Filial { get; set; }
        public ZpzExpertise2023 Expertise { get; set; }
        public ZpzNormative2023 Normative { get; set; }
        public ZpzFinance2023 Finance { get; set; }
        public ZpzPersonnel2023 Personnel { get; set; }
    }

    public class ZpzExpertise2023
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

    public class ZpzNormative2023
    {
        public decimal BillsOutMo { get; set; }
        public decimal MeeOutMoPlan { get; set; }
        public decimal MeeOutMoTarget { get; set; }
        public decimal BillsApp { get; set; }
        public decimal MeeAppPlan { get; set; }
        public decimal MeeAppTarget { get; set; }
        public decimal BillsDayHosp { get; set; }
        public decimal MeeDayHospPlan { get; set; }
        public decimal MeeDayHospPlanVmp { get; set; }
        public decimal MeeDayHospTarget { get; set; }
        public decimal MeeDayHospTargetVmp { get; set; }
        public decimal BillsHosp { get; set; }
        public decimal MeeHospPlan { get; set; }
        public decimal MeeHospPlanVmp { get; set; }
        public decimal MeeHospTarget { get; set; }
        public decimal MeeHospTargetVmp { get; set; }
        public decimal EkmpOutMoPlan { get; set; }
        public decimal EkmpOutMoTarget { get; set; }
        public decimal EkmpAppPlan { get; set; }
        public decimal EkmpAppTarget { get; set; }
        public decimal EkmpDayHospPlan { get; set; }
        public decimal EkmpDayHospTarget { get; set; }
        public decimal EkmpDayHospPlanVmp { get; set; }
        public decimal EkmpDayHospTargetVmp { get; set; }
        public decimal EkmpHospPlan { get; set; }
        public decimal EkmpHospTarget { get; set; }
        public decimal EkmpHospPlanVmp { get; set; }
        public decimal EkmpHospTargetVmp { get; set; }
    }

    public class ZpzFinance2023
    {
        public decimal SumPayment { get; set; }
        public decimal SumNotPayment { get; set; }
        public decimal SumMek { get; set; }
        public decimal SumMee { get; set; }
        public decimal SumEkmp { get; set; }
    }

    public class ZpzPersonnel2023
    {
        public decimal SpecialistFullTime { get; set; }
        public decimal SpecialistRemote { get; set; }
        public decimal FullTime { get; set; }
        public decimal Remote { get; set; }
        public decimal ExpertsFullTime { get; set; }
        public decimal ExpertsRemote { get; set; }
        public decimal ExpertsEkmpRegion { get; set; }
        public decimal ExpertsEkmpRemote { get; set; }
        public decimal ExpertisesFullTime { get; set; }
        public decimal ExpertisesRemote { get; set; }
        public decimal ExpertisesPlanFullTime { get; set; }
        public decimal ExpertisesPlanRemote { get; set; }
        public decimal ExpertisesPlanAppealFullTime { get; set; }
        public decimal ExpertisesPlanAppealRemote { get; set; }
        public decimal ExpertisesPlanUnfoundedFullTime { get; set; }
        public decimal ExpertisesPlanUnfoundedRemote { get; set; }
        public decimal ExpertisesUnplannedFullTime { get; set; }
        public decimal ExpertisesUnplannedRemote { get; set; }
        public decimal ExpertisesUnplannedAppealFullTime { get; set; }
        public decimal ExpertisesUnplannedAppealRemote { get; set; }
        public decimal ExpertisesUnplannedUnfoundedFullTime { get; set; }
        public decimal ExpertisesUnplannedUnfoundedRemote { get; set; }
        public decimal ExpertisesThemeFullTime { get; set; }
        public decimal ExpertisesThemeRemote { get; set; }
        public decimal ExpertisesThemeAppealFullTime { get; set; }
        public decimal ExpertisesThemeAppealRemote { get; set; }
        public decimal ExpertisesThemeUnfoundedFullTime { get; set; }
        public decimal ExpertisesThemeUnfoundedRemote { get; set; }
        public decimal ExpertisesMultiFullTime { get; set; }
        public decimal ExpertisesMultiRemote { get; set; }
        public decimal ExpertisesMultiAppealFullTime { get; set; }
        public decimal ExpertisesMultiAppealRemote { get; set; }
        public decimal ExpertisesMultiUnfoundedFullTime { get; set; }
        public decimal ExpertisesMultiUnfoundedRemote { get; set; }
        public decimal PreparedFullTime { get; set; }
        public decimal PreparedRemote { get; set; }
        public decimal QualFullTime { get; set; }
        public decimal QualRemote { get; set; }
        public decimal QualHigherFullTime { get; set; }
        public decimal QualHigherRemote { get; set; }
        public decimal Qual1stFullTime { get; set; }
        public decimal Qual1stRemote { get; set; }
        public decimal Qual2ndFullTime { get; set; }
        public decimal Qual2ndRemote { get; set; }
        public decimal DegreeFullTime { get; set; }
        public decimal DegreeRemote { get; set; }
        public decimal CandidateFullTime { get; set; }
        public decimal CandidateRemote { get; set; }
        public decimal DoctorFullTime { get; set; }
        public decimal DoctorRemote { get; set; }
        public decimal InsRepresFullTime { get; set; }
        public decimal InsRepres1FullTime { get; set; }
        public decimal InsRepres1spFullTime { get; set; }
        public decimal InsRepres2FullTime { get; set; }
        public decimal InsRepres2spFullTime { get; set; }
        public decimal InsRepres3FullTime { get; set; }
        public decimal InsRepres3spFullTime { get; set; }
    }
}