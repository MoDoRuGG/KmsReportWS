using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsReportWS.Model.ConcolidateReport
{
    public class ConsolidateDisp
    {
        public string Filial { get; set; }
        public DispComplaint Complaint { get; set; }
        public DispProtection Protection { get; set; }
        public DispMek Mek { get; set; }
        public DispMee Mee { get; set; }
        public DispEkmp Ekmp { get; set; }
        public DispFinance Finance { get; set; }
    }


    /// <summary>
    /// Жалобы Гр 7 
    /// </summary>
    public class DispComplaint
    {
        public decimal Row361Gr7 { get; set; }
        public decimal Row365Gr7 { get; set; }
        public decimal Row37Gr7 { get; set; }
        public decimal Row371Gr7 { get; set; }
        public decimal Row372Gr7 { get; set; }
        public decimal Row3721Gr7 { get; set; }
        public decimal Row373Gr7 { get; set; }
        public decimal Row3731Gr7 { get; set; }
        public decimal Row462Gr7 { get; set; }
        public decimal Row47Gr7 { get; set; }
        public decimal Row471Gr7 { get; set; }
        public decimal Row472Gr7 { get; set; }
        public decimal Row4721Gr7 { get; set; }
        public decimal Row473Gr7 { get; set; }
        public decimal Row4731Gr7 { get; set; }
        public decimal Row49Gr7 { get; set; }


    }
    /// <summary>
    /// Защита 
    /// </summary>
    public class DispProtection
    {

        public decimal Row161Gr3 { get; set; }
        public decimal Row165Gr3 { get; set; }
        public decimal Row17Gr3 { get; set; }
        public decimal Row171Gr3 { get; set; }
        public decimal Row172Gr3 { get; set; }
        public decimal Row1721Gr3 { get; set; }
        public decimal Row173Gr3 { get; set; }
        public decimal Row1731Gr3 { get; set; }

        public decimal Row161Gr6 { get; set; }
        public decimal Row165Gr6 { get; set; }
        public decimal Row17Gr6 { get; set; }
        public decimal Row171Gr6 { get; set; }
        public decimal Row172Gr6 { get; set; }
        public decimal Row1721Gr6 { get; set; }
        public decimal Row173Gr6 { get; set; }
        public decimal Row1731Gr6 { get; set; }

    }

    /// <summary>
    /// МЭК гр 3
    /// </summary>
    public class DispMek
    {
        public decimal Row1Gr3 { get; set; }
        public decimal Row12Gr3 { get; set; }
        public decimal Row4Gr3 { get; set; }
        public decimal Row41Gr3 { get; set; }
        public decimal Row411Gr3 { get; set; }
        public decimal Row42Gr3 { get; set; }
        public decimal Row421Gr3 { get; set; }
        public decimal Row43Gr3 { get; set; }
        public decimal Row431Gr3 { get; set; }
        public decimal Row44Gr3 { get; set; }
        public decimal Row441Gr3 { get; set; }
        public decimal Row45Gr3 { get; set; }
        public decimal Row451Gr3 { get; set; }
        public decimal Row5Gr3 { get; set; }
        public decimal Row51Gr3 { get; set; }
        public decimal Row52Gr3 { get; set; }

    }

    /// <summary>
    /// Мээ
    /// </summary>
    public class DispMee
    {
        public decimal Row21Gr3 { get; set; }
        public decimal Row22Gr3 { get; set; }
        public decimal Row221Gr3 { get; set; }
        public decimal Row222Gr3 { get; set; }
        public decimal Row223Gr3 { get; set; }
        public decimal Row24Gr3 { get; set; }
        public decimal Row241Gr3 { get; set; }
        public decimal Row26Gr10 { get; set; }
        public decimal Row531Gr3 { get; set; }
        public decimal Row531Gr10 { get; set; }
        public decimal Row5311Gr3 { get; set; }
        public decimal Row5311Gr10 { get; set; }
        public decimal Row54Gr3 { get; set; }
        public decimal Row54Gr10 { get; set; }
        public decimal Row55Gr3 { get; set; }
        public decimal Row55Gr10 { get; set; }
        public decimal Row56Gr3 { get; set; }
        public decimal Row56Gr10 { get; set; }
        public decimal Row561Gr3 { get; set; }
        public decimal Row561Gr10 { get; set; }

    }

    /// <summary>
    ///  ЭКМП
    /// </summary>
    public class DispEkmp
    {
        public decimal Row21Gr3 { get; set; }
        public decimal Row223Gr3 { get; set; }
        public decimal Row25Gr3 { get; set; }
        public decimal Row25Gr10 { get; set; }
        public decimal Row251Gr3 { get; set; }
        public decimal Row251Gr10 { get; set; }
        public decimal Row611Gr3 { get; set; }
        public decimal Row611Gr10 { get; set; }
        public decimal Row6111Gr3 { get; set; }
        public decimal Row6111Gr10 { get; set; }
        public decimal Row62Gr3 { get; set; }
        public decimal Row62Gr10 { get; set; }
        public decimal Row621Gr3 { get; set; }
        public decimal Row621Gr10 { get; set; }
        public decimal Row63Gr3 { get; set; }
        public decimal Row63Gr10 { get; set; }
        public decimal Row631Gr3 { get; set; }
        public decimal Row631Gr10 { get; set; }
        public decimal Row632Gr3 { get; set; }
        public decimal Row632Gr10 { get; set; }
        public decimal Row633Gr3 { get; set; }
        public decimal Row633Gr10 { get; set; }
        public decimal Row64Gr3 { get; set; }
        public decimal Row64Gr10 { get; set; }
        public decimal Row641Gr3 { get; set; }
        public decimal Row641Gr10 { get; set; }
        public decimal Row642Gr3 { get; set; }
        public decimal Row642Gr10 { get; set; }
        public decimal Row643Gr3 { get; set; }
        public decimal Row643Gr10 { get; set; }
        public decimal Row644Gr3 { get; set; }
        public decimal Row644Gr10 { get; set; }
        public decimal Row645Gr3 { get; set; }
        public decimal Row645Gr10 { get; set; }
        public decimal Row651Gr3 { get; set; }
        public decimal Row651Gr10 { get; set; }
        public decimal Row652Gr3 { get; set; }
        public decimal Row652Gr10 { get; set; }
        public decimal Row653Gr3 { get; set; }
        public decimal Row653Gr10 { get; set; }

    }

    public class DispFinance
    {
        public decimal Row4Gr3 { get; set; }
        public decimal Row41Gr3 { get; set; }
        public decimal Row411Gr3 { get; set; }
        public decimal Row412Gr3 { get; set; }
        public decimal Row413Gr3 { get; set; }
        public decimal Row414Gr3 { get; set; }
        public decimal Row51Gr3 { get; set; }
        public decimal Row511Gr3 { get; set; }
        public decimal Row512Gr3 { get; set; }
        public decimal Row513Gr3 { get; set; }
        public decimal Row514Gr3 { get; set; }
        public decimal Row52Gr3 { get; set; }
        public decimal Row521Gr3 { get; set; }
        public decimal Row522Gr3 { get; set; }
        public decimal Row523Gr3 { get; set; }
        public decimal Row53Gr3 { get; set; }
        public decimal Row531Gr3 { get; set; }
        public decimal Row532Gr3 { get; set; }
        public decimal Row533Gr3 { get; set; }
        public decimal Row54Gr3 { get; set; }
        public decimal Row541Gr3 { get; set; }
        public decimal Row542Gr3 { get; set; }
        public decimal Row543Gr3 { get; set; }
        public decimal Row55Gr3 { get; set; }
        public decimal Row551Gr3 { get; set; }
        public decimal Row552Gr3 { get; set; }
        public decimal Row553Gr3 { get; set; }
        public decimal Row56Gr3 { get; set; }
        public decimal Row561Gr3 { get; set; }
        public decimal Row562Gr3 { get; set; }
        public decimal Row563Gr3 { get; set; }

    }

}