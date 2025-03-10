using System.ComponentModel;

namespace KmsReportWS.Model.Report
{
    public enum ReportType
    {
        [Description("f262")] F262,
        [Description("f294")] F294,
        [Description("iizl")] Iizl,
        [Description("iizl2022")] Iizl2022,
        [Description("PG")] Pg,
        [Description("Zpz")] Zpz,
        [Description("PG_Q")] PgQ,
        [Description("Zpz_Q")] ZpzQ,
        [Description("foped")] Oped,
        [Description("fopedU")] OpedU,
        [Description("opedQ")] OpedQ,
        [Description("fcr")] IR,
        [Description("vac")] Vac,
        [Description("mfss")] MFSS,
        [Description("mvcr")] MVCR,
        [Description("proposal")] Proposal,
        [Description("oped_fin")] OpedFinance,
        [Description("oped_fin3")] OpedFinance3,
        [Description("cadre")] Cadre,
        [Description("infomat")] Infomaterial,
        [Description("Zpz10")] Zpz10,
        [Description("Effective")] Effective,
        [Description("ZpzLethal")] ZpzLethal,
        [Description("ReqVCR")] ReqVCR,
        [Description("Quantity")] Quantity,
        [Description("TarAllow")] TarAllow,
        [Description("pvpl")] PVPLoad,
        [Description("doff")] Doff,
        [Description("Zpz2025")] Zpz2025,
        [Description("Zpz_Q2025")] ZpzQ2025,
        [Description("Zpz10_2025")] Zpz10_2025,
        [Description("ZpzL2025")] ZpzL2025,
        [Description("ViolMEE")] ViolMEE,
        [Description("ViolEKMP")] ViolEKMP,
        [Description("VerifyPlan")] VerifyPlan,
        [Description("MonthlyVol")] MonthlyVol,
    }
}