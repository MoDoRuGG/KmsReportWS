using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class Consolidate294
    {
        public List<Dispanserization> Disp13List { get; set; }
        public List<Dispanserization> Disp12List { get; set; }
        public List<Dispanserization> Disp2List { get; set; }
        public List<DispensaryObservation> DispensaryObservationList { get; set; }
        public List<PhoneQuestionary> PhoneQuestionaryList { get; set; }
        public List<InsuranceRepresentative> InsuranceRepresentativeList { get; set; }
        public List<Treatment> TreatmentList { get; set; }
        public List<Efficiency> EfficiencyList { get; set; }
    }

    public class Dispanserization
    {
        public string Filial { get; set; }
        public string Month { get; set; }
        public int CountPeoplelMo { get; set; }
        public int CountPeopleInform { get; set; }
        public int CountPeopleRepeatInform { get; set; }
        public int Sms { get; set; }
        public int Post { get; set; }
        public int Phone { get; set; }
        public int Messangers { get; set; }
        public int EMail { get; set; }
        public int Address { get; set; }
        public int AnotherType { get; set; }
    }

    public class DispensaryObservation
    {
        public string Filial { get; set; }
        public string Month { get; set; }
        public int CountPeopleInform { get; set; }
        public int Onco { get; set; }
        public int Endo { get; set; }
        public int Broncho { get; set; }
        public int BloodCirculatory { get; set; }
        public int NotInfection { get; set; }
    }

    public class PhoneQuestionary
    {
        public string Filial { get; set; }
        public string Month { get; set; }
        public int Prof { get; set; }
        public int Disp { get; set; }
    }

    public class InsuranceRepresentative
    {
        public string Filial { get; set; }
        public string Month { get; set; }
        public int FirstLevel { get; set; }
        public int FirstLevelTraining { get; set; }
        public int SecondLevel { get; set; }
        public int SecondLevelTraining { get; set; }
        public int ThirdLevel { get; set; }
        public int ThirdLevelTraining { get; set; }
    }

    public class Treatment
    {
        public string Filial { get; set; }
        public string Month { get; set; }
        public int Oral { get; set; }
        public int OralSecond { get; set; }
        public int OralThird { get; set; }
        public int SecondLevelTraining { get; set; }
        public int ThirdLevel { get; set; }
        public int ThirdLevelTraining { get; set; }
        public int Written { get; set; }
        public int WrittenSecond { get; set; }
        public int WrittenThird { get; set; }
        public int WrittenDoctor { get; set; }
    }

    public class Efficiency
    {
        public string Filial { get; set; }
        public string Month { get; set; }
        public int Disp13Inform { get; set; }
        public int Disp12Inform { get; set; }
        public int AimedDisp2 { get; set; }
        public int Disp2 { get; set; }
        public int SubjectToDisp { get; set; }
    }

}