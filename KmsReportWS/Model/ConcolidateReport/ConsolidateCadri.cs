using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateCadri
    {
        public string Filial { get; set; }
        public ConsolidateCardiData data { get; set; }

    }

    public class ConsolidateCardiData
    {
      
        public ConsolidateCardiDataF r1 {get;set;}
        public ConsolidateCardiDataF r11 {get;set; }
        public ConsolidateCardiDataF r111 {get;set; }
        public ConsolidateCardiDataF r112 {get;set; }
        public ConsolidateCardiDataF r113 {get;set; }
        public ConsolidateCardiDataF r1131 {get;set; }
        public ConsolidateCardiDataF r11311 {get;set; }
        public ConsolidateCardiDataF r1132 {get;set; }
        public ConsolidateCardiDataF r11321 {get;set; }
        public ConsolidateCardiDataF r2 {get;set; }
        public ConsolidateCardiDataF r21 {get;set; }
        public ConsolidateCardiDataF r3{get;set; }
        public ConsolidateCardiDataF r31 {get;set; }
        public ConsolidateCardiDataF r32 {get;set; }
        public ConsolidateCardiDataF r33 {get;set; }
        public ConsolidateCardiDataF r4 {get;set; }
        public ConsolidateCardiDataF r41 {get;set; }
        public ConsolidateCardiDataF r42 {get;set; }

    }

    public class ConsolidateCardiDataF
    {
       
        public decimal IzNih1 { get; set; }
        public decimal IzNih2 { get; set; }
    }
}