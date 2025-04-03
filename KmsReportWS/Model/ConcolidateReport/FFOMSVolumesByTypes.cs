using System.Collections.Generic;
using KmsReportWS.Model.Report;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class FFOMSVolumesByTypes
    {
        public List<FFOMSVolumesByTypesFull> VolFull { get; set; }
        public List<FFOMSVolumesByTypesByFilials> VolFil { get; set; }
    }
        public class FFOMSVolumesByTypesFull
    {
        public decimal mee_unpl { get; set; }
        public decimal mee_pl { get; set; }
        public decimal ekmp_unpl { get; set; }
        public decimal ekmp_pl { get; set; }
        public decimal mek_app { get; set; }
        public decimal mee_app_unpl { get; set; }
        public decimal mee_app_pl { get; set; }
        public decimal ekmp_app_unpl { get; set; }
        public decimal ekmp_app_pl { get; set; }
        public decimal mek_skp { get; set; }
        public decimal mee_skp_unpl { get; set; }
        public decimal mee_skp_pl { get; set; }
        public decimal ekmp_skp_unpl { get; set; }
        public decimal ekmp_skp_pl { get; set; }
        public decimal mek_smp { get; set; }
        public decimal mee_smp_unpl { get; set; }
        public decimal mee_smp_pl { get; set; }
        public decimal ekmp_smp_unpl { get; set; }
        public decimal ekmp_smp_pl { get; set; }
        public decimal mek_sdp { get; set; }
        public decimal mee_sdp_unpl { get; set; }
        public decimal mee_sdp_pl { get; set; }
        public decimal ekmp_sdp_unpl { get; set; }
        public decimal ekmp_sdp_pl { get; set; }
    }

    public class FFOMSVolumesByTypesByFilials
    {
        public string Code { get; set; }
        public string Filial { get; set; }
        public decimal mek_app { get; set; }
        public decimal mee_app { get; set; }
        public decimal ekmp_app { get; set; }
        public decimal mek_skp { get; set; }
        public decimal mee_skp { get; set; }
        public decimal ekmp_skp { get; set; }
        public decimal mek_smp { get; set; }
        public decimal mee_smp { get; set; }
        public decimal ekmp_smp { get; set; }
        public decimal mek_sdp { get; set; }
        public decimal mee_sdp { get; set; }
        public decimal ekmp_sdp { get; set; }
    }
}