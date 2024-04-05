using System.Collections.Generic;
using System.Web.Security;
using System.Web.UI;

namespace KmsReportWS.Model.Report
{
    public class ReportPVPLoad : AbstractReport
    {

        public int Id_Report_Data { get; set; }
        public List<PVPload> Data { get; set; }

        public ReportPVPLoad()
        {
            Data = new List<PVPload>();
        }
    }
    public class PVPload
    {
        public int RowNumID { get; set; }
        public string PVP_name { get; set; }
        public string location_of_the_office { get; set; }
        public int number_of_insured_by_beginning_of_year { get; set; }
        public int number_of_insured_by_reporting_date { get; set; }
        public int population_dynamics { get; set; }
        public string specialist { get; set; }
        public decimal conditions_of_employment { get; set; }
        public int PVP_plan { get; set; }
        public int registered_total_citizens { get; set; }
        public int newly_insured { get; set; }
        public int attracted_by_agents { get; set; }
        public int issued_by_PEO_and_extracts_from_ERZL { get; set; }   
        public decimal workload_per_day_for_specialist { get; set; }
        public int appeals_through_EPGU { get; set; }
        public string notes { get; set; }

    }
}