using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateCardio
    {
        public string Filial { get; set; }

        public CardioComplaint Complaint { get; set; }
        public CardioProtection Protection { get; set; }

        public CardioMeeEkmp MeeEkmp {get;set;}

        public CardioFinance Finance { get; set; }

    }

    /// <summary>
    /// Таблица 1. ЖАЛОБЫ
    /// </summary>
    public class CardioComplaint
    {
        /// <summary>
        /// т.1. с 3.6.3, гр7.Поступило жалоб на оказание медицинской помощи (за исключением оказания медицинской помощи несовершеннолетним)
        /// </summary>
        public decimal MedicalHelp { get; set; }

        /// <summary>
        ///т.1. с 3.6.4, гр7 несовершеннолетние
        /// </summary>
        public decimal Underage { get; set; }

        /// <summary>
        /// Т.1. с.4.6.4., гр.7 Обратились за разъяснениямина по вопросам оказание медицинской помощи
        /// </summary>
        public decimal AskMedicalHelp { get; set; }

        /// <summary>
        /// Т.1. с.4.6.5., гр.7 Обратились за разъяснениямина по вопросам оказание медицинской помощи Несовершенолетние
        /// </summary>
        public decimal UnderageAskMedicalHelp { get; set; }

    }

    /// <summary>
    /// Таблица 3. Защита
    /// </summary>
    public class CardioProtection
    {
        /// <summary>
        /// Т.3., с.1.6.3, гр.3 Количество спорных случаевв  связи с оказанием медицинской помощи, разрешенных в досудебном порядке
        /// </summary>

        public decimal PretrialMedicalHelp { get; set; }

        /// <summary>
        /// Т.3., с.1.6.4, гр.3 Несовершенолетние досудебные порядок
        /// </summary>
        public decimal PretrialUnderage { get; set; }

        /// <summary>
        /// Т.3., с.1.6.3, гр.6 Количество спорных случаев в связи с оказанием медицинской помощи, разрешенных в судебном порядке
        /// </summary>
        public decimal JudicalMedicalHelp { get; set; }

        /// <summary>
        /// Т.3., с.1.6.4, гр.6 Несовершенолетние разрешенных в судебном порядке
        /// </summary>
        public decimal JudicalUnderage { get; set; }

    }



    /// <summary>
    /// Таблицы 6 МЭЭ и 8 ЭКМП
    /// </summary>
    public class CardioMeeEkmp
    {
        /// <summary>
        /// т.6.,с.2.2.2.,гр.3 в связи с получением жалоб от застрахованных лиц или их законных представителей  МЭЭ
        /// </summary>
        public decimal ComplaintsMEE { get; set; }

        /// <summary>
        /// т.6., с.2.5.2.,гр.3 в связи с госпитализацией застрахованного лица, медицинская помощь которому должна быть оказана в плановой форме в стационаре (структурном подразделении стационара) другого профиля в соответствии с принятыми в субъекте РФ (далее - непрофильная госпитализация)
        /// </summary>
        public decimal HospitalizationMEE { get; set; }

        /// <summary>
        /// т 6., с.5.3.1.3.,гр.3 в результате целевой экспертизы  условий оказания медицинской помощи, включая нарушение сроков ее ожидания
        /// </summary>
        public decimal CelExpertiseMEE { get; set; }

        /// <summary>
        /// т 6., с.5.3.1.3.,гр.10 в результате плановой экспертизы   условий оказания медицинской помощи, включая нарушение сроков ее ожидания
        /// </summary>
        public decimal PlanExpertiseMEE { get; set; }


        /// <summary>
        /// т.8., с.2.2.2.,гр.3 в связи с получением жалоб от застрахованных лиц или их законных представителей ЭКМП
        /// </summary>
        public decimal ComplaintsEKMP { get; set; }

        /// <summary>
        /// т.8 , с. 2.4.1,гр.3 целевые  летальный исход в связи с коронарным синдромом
        /// </summary>
        public decimal CelLetalKoronar { get; set; }

        /// <summary>
        /// т8.,с 2.4.2.,гр.3  целевые летальный исход в связи с ОНМК
        /// </summary>
        public decimal CelLetalOnmk { get; set; }

        /// <summary>
        /// т.8.,с.6.1.1.3.,гр.3 целевые всего по профилю «сердечно-сосудистые заболевания» :
        /// </summary>
        public decimal CelAllCardio { get; set; }

        /// <summary>
        /// т.8.,с.6.1.1.4.,гр.3 несовершеннолетние
        /// </summary>
        public decimal CelUnderage { get; set; }

        /// <summary>
        /// т.8.,с.6.1.1.3.,гр.10 плановые всего по профилю «сердечно-сосудистые заболевания»
        /// </summary>

        public decimal PlanAllCardio { get; set; }
        /// <summary>
        /// т.8.,с.6.1.1.4.,гр.10 несовершеннолетние 
        /// </summary>
        public decimal PlanCardioUnderage { get; set; }

        /// <summary>
        /// т.8.,с.6.2.2.,гр.3  целевая  непрофильная  госпитализация 
        /// </summary>
        public decimal CelNeprofilGospital { get; set; }

        /// <summary>
        /// т.8.,с.6.2.3.,гр.3 несовершеннолетние
        /// </summary>
        public decimal CelNeprofilGospitalUnderage { get; set; }

        /// <summary>
        /// т.8.,с.6.2.2.,гр.10 плановая непрофильная  госпитализация  
        /// </summary>
        public decimal PlanNeprofilGospital { get; set; }

        /// <summary>
        /// т.8.,с.6.2.3.,гр.10 несовершенолетние
        /// </summary>
        public decimal PlanNeprofilGospitalUnderage { get; set; }

        /// <summary>
        /// т.8.,с.6.3.2.,гр.3
        /// </summary>
        public decimal CelNevipolnenie { get; set; }

        /// <summary>
        /// т.8.,с.6.3.3.,гр.3
        /// </summary>
        public decimal CelNevipolnenieUnderage { get; set; }

        /// <summary>
        /// т.8.,с.6.3.2.,гр.10
        /// </summary>
        public decimal PlanNevipolnenie { get; set; }

        /// <summary>
        /// т.8.,с.6.3.3.,гр.10
        /// </summary>
        public decimal PlanNevipolnenieUnderage { get; set; }

        /// <summary>
        /// т.8., с.6.4.3.,гр.3
        /// </summary>
        public decimal CelNotAddDispNab { get; set; }

        /// <summary>
        /// т.8., с.6.4.4.,гр.3
        /// </summary>
        public decimal CelNotAddDispNabUnderage { get; set; }

        /// <summary>
        /// т.8., с.6.4.3.,гр.10
        /// </summary>
        public decimal PlanNotAddDispNab { get; set; }

        /// <summary>
        /// т.8., с.6.4.4.,гр.10
        /// </summary>
        public decimal PlanNotAddDispNabUnderage { get; set; }

        /// <summary>
        /// т.8, с.6.5.2.,г.3
        /// </summary>
        public decimal CelNotSobludClinicRecomendation { get; set; }

        /// <summary>
        /// т.8, с.6.5.3.,г.3
        /// </summary>
        public decimal CelNotSobludClinicRecomendationUnderage { get; set; }


        /// <summary>
        /// т.8, с.6.5.2.,г.10
        /// </summary>
        public decimal PlanNotSobludClinicRecomendation { get; set; }

        /// <summary>
        /// т.8, с.6.5.3.,г.10
        /// </summary>
        public decimal PlanNotSobludClinicRecomendationUnderage { get; set; }

        /// <summary>
        /// т.8, с.6.6.2.,гр.3
        /// </summary>
        public decimal CelPrematureCloseHelpMerop { get; set; }

        /// <summary>
        /// т.8, с.6.6.3.,гр.3
        /// </summary>
        public decimal CelPrematureCloseHelpMeropUnderage { get; set; }

        /// <summary>
        /// т.8, с.6.6.2.,гр.10
        /// </summary>
        public decimal PlanPrematureCloseHelpMerop { get; set; }

        /// <summary>
        /// т.8, с.6.6.3.,гр.10
        /// </summary>
        public decimal PlanPrematureCloseHelpMeropUnderage { get; set; }

        /// <summary>
        /// т.8, с.6.7.2.,гр.3
        /// </summary>
        public decimal CelViolationHospital { get; set; }

        /// <summary>
        /// т.8, с.6.7.3.,гр.3
        /// </summary>
        public decimal CelViolationHospitalUnderage { get; set; }

        /// <summary>
        /// т.8, с.6.7.2.,гр.10
        /// </summary>
        public decimal PlanViolationHospital { get; set; }

        /// <summary>
        /// т.8, с.6.7.3.,гр.10
        /// </summary>
        public decimal PlanViolationHospitalUnderage { get; set; }

        /// <summary>
        /// т.8., с.6.8.2.,гр.3
        /// </summary>
        public decimal CelNeobosOtkaz { get; set; }

        /// <summary>
        /// т.8., с.6.8.3.,гр.3
        /// </summary>
        public decimal CelNeobosOtkazUnderage { get; set; }

        /// <summary>
        /// т.8., с.6.8.2.,гр.10
        /// </summary>
        public decimal PlanNeobosOtkaz { get; set; }

        /// <summary>
        /// т.8., с.6.8.3.,гр.10
        /// </summary>
        public decimal PlanNeobosOtkazUnderage { get; set; }

    }

    /// <summary>
    /// Таблица 10. Финансы
    /// </summary>
    public class CardioFinance
    {
        /// <summary>
        /// т.10 , с.4.1.3.гр.3
        /// </summary>
        public decimal SumMeeNotTimeDispan { get; set; }


        /// <summary>
        /// т.10 , с.4.1.4.гр.3
        /// </summary>
        public decimal SumMeeNotTimeDispanUnderage { get; set; }

        /// <summary>
        /// т.10., с 5.1.3.гр.3
        /// </summary>
        public decimal SumEkmpNotTimeDispan {get;set;}

        /// <summary>
        /// т.10., с 5.1.4.гр.3
        /// </summary>
        public decimal SumEkmpNotTimeDispanUnderage { get;set;}

        /// <summary>
        /// т.10., с 5.2.2.гр.3
        /// </summary>
        public decimal SumNeprofilHelp { get; set; }

        /// <summary>
        /// т.10., с 5.2.3.гр.3
        /// </summary>
        public decimal SumNeprofilHelpUnderage { get; set; }

        /// <summary>
        /// т.10., с 5.3.2.гр.3
        /// </summary>
        public decimal SumNevipolnenie { get; set; }

        /// <summary>
        /// т.10., с 5.3.3.гр.3
        /// </summary>
        public decimal SumNevipolnenieUnderage { get; set; }

        /// <summary>
        /// т.10., с 5.4.2.гр.3
        /// </summary>
        public decimal SumNesobludRecomedation { get; set; }

        /// <summary>
        /// т.10., с 5.4.3.гр.3
        /// </summary>
        public decimal SumNesobludRecomedationUnderage { get; set; }

        /// <summary>
        /// т.10., с 5.5.2.гр.3
        /// </summary>
        
        public decimal SumCloseHelp { get; set; }

        /// <summary>
        /// т.10., с 5.5.3.гр.3
        /// </summary>
        public decimal SumCloseHelpUnderage { get; set; }

        /// <summary>
        /// т.10., с 5.6.2.гр.3
        /// </summary>
        public decimal SumViolation { get; set; }

        /// <summary>
        /// т.10., с 5.6.3.гр.3
        /// </summary>
        public decimal SumViolationUnderage { get; set; }


    }






}