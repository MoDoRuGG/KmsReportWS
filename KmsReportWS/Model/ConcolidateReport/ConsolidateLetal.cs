using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateLetal
    {
        public string Filial { get; set; }
        public LetalData Data { get; set; }
    }

    public class LetalData
    {
        public decimal r1 { get; set; }
        public decimal r1_1 { get; set; }
        public decimal r1_2 { get; set; }
        public decimal r121 { get; set; }
        public decimal r2 { get; set; }
        public decimal r3 { get; set; }
        public decimal r31 { get; set; }
        public decimal r311 { get; set; }
        public decimal r3111 { get; set; }
        public decimal r3112 { get; set; }
        public decimal r3113 { get; set; }
        public decimal r3114 { get; set; }
        public decimal r32 { get; set; }
        public decimal r33 { get; set; }
        public decimal r4 { get; set; }
        public decimal r5 { get; set; }
        public decimal r6 { get; set; }
        public decimal r7 { get; set; }
        public decimal r8 { get; set; }
        public decimal r9 { get; set; }
        public decimal r10 { get; set; }
        public decimal r11 { get; set; }
        public decimal r12 { get; set; }
        public decimal r13 { get; set; }
    }
}