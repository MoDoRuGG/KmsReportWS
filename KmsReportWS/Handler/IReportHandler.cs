using KmsReportWS.Model.Report;

namespace KmsReportWS.Handler
{
    public interface IReportHandler
    {
        AbstractReport GetReport(string filialCode, string yymm);
        AbstractReport SaveReportToDb(AbstractReport report, string yymm, int idUser, string filialCode);
        ReportOpedDto[] GetYearOpedData(string yymm, string filiall);     
    }
}