using System.Collections.Generic;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSMonthlyVol
    {
        public string Filial { get; set; }
        public List<FFOMSMonthlyVol_SKP> FFOMSMonthlyVol_SKP { get; set; }
        public List<FFOMSMonthlyVol_SDP> FFOMSMonthlyVol_SDP { get; set; }
        public List<FFOMSMonthlyVol_APP> FFOMSMonthlyVol_APP { get; set; }
        public List<FFOMSMonthlyVol_SMP> FFOMSMonthlyVol_SMP { get; set; }
    }

    public class FFOMSMonthlyVol_SKP
    {
        public int? RowNum { get; set; }
        public int CountSluch { get; set; }
        public int CountAppliedSluch { get; set; }
        public int CountSluchMEE { get; set; }
        public int CountSluchEKMP { get; set; }
    }

    public class FFOMSMonthlyVol_SDP
    {
        public int? RowNum { get; set; }
        public int CountSluch { get; set; }
        public int CountAppliedSluch { get; set; }
        public int CountSluchMEE { get; set; }
        public int CountSluchEKMP { get; set; }
    }

    public class FFOMSMonthlyVol_APP
    {
        public int? RowNum { get; set; }
        public int CountSluch { get; set; }
        public int CountAppliedSluch { get; set; }
        public int CountSluchMEE { get; set; }
        public int CountSluchEKMP { get; set; }
    }

    public class FFOMSMonthlyVol_SMP
    {
        public int? RowNum { get; set; }
        public int CountSluch { get; set; }
        public int CountAppliedSluch { get; set; }
        public int CountSluchMEE { get; set; }
        public int CountSluchEKMP { get; set; }
    }
}