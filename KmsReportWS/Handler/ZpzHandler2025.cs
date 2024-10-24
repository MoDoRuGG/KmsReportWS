using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;

namespace KmsReportWS.Handler
{
    // Обработчик отчетов для типа Zpz2025
    public class Zpz2025Handler : BaseReportHandler
    {
        // Строка подключения к базе данных
        private readonly string _connStr = Settings.Default.ConnStr;

        // Конструктор, принимающий тип отчета
        public Zpz2025Handler(ReportType reportType) : base(reportType)
        {
        }

        // Метод для вставки нового отчета в базу данных (пока не реализован)
        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }

        // Получение данных за указанный год по теме и филиалу
        public ReportZpz2025DataDto GetYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            // Определяем начальную дату для выборки
            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_Zpz2025.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "Zpz10_2025"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportZpz2025DataDto
            {
                CountSmo = (decimal)x.Sum(g => g.CountSmo)

            }).FirstOrDefault();

            return result;
        }

        // Получение данных по летальным случаям за указанный год
        public ReportZpz2025DataDto GetLethalYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            // Определяем начальную дату для выборки
            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_Zpz2025.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "ZpzLethal_2025"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportZpz2025DataDto
            {
                CountSmo = (decimal)x.Sum(g => g.CountSmo)

            }).FirstOrDefault();

            return result;
        }

        // Метод для создания нового отчета в базе данных
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as ReportZpz2025 ?? throw new Exception("Error saving new report, because getting empty report");

            // Проход по всем формам отчетов
            foreach (var reportForms in report.ReportDataList)
            {
                // Создание записи темы отчета
                var themeData = new Report_Data
                {
                    Id_Flow = flow.Id,
                    Id_Report = flow.Id_Report_Type,
                    Theme = reportForms.Theme
                };
                db.Report_Data.InsertOnSubmit(themeData);
                db.SubmitChanges();

                // Подготовка данных для вставки
                var zpzDataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (zpzDataList.Any())
                {
                    db.Report_Zpz2025.InsertAllOnSubmit(zpzDataList);
                }
                db.SubmitChanges();
            }
        }

        // Метод для обновления существующего отчета в базе данных
        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportZpz2025 ?? throw new Exception("Error update report, because getting empty report");

            // Проход по всем формам отчетов
            foreach (var reportForms in report.ReportDataList)
            {
                // Получение ID темы отчета из базы данных
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id;
                if (idTheme != null)
                {
                    // Удаление существующих данных отчета
                    var dataReport = db.Report_Zpz2025.Where(x => x.Id_Report_Data == idTheme);
                    db.Report_Zpz2025.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    // Подготовка новых данных для вставки
                    var zpzDataList = reportForms.Data.Select(data => MapThemeToPersist(idTheme.Value, data)).ToList();
                    if (zpzDataList.Any())
                    {
                        db.Report_Zpz2025.InsertAllOnSubmit(zpzDataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        // Метод для маппинга данных отчета из базы данных в объект отчета
        protected override AbstractReport MapReportFromPersist(Report_Flow rep_flow)
        {
            var outReport = new ReportZpz2025 { ReportDataList = new List<ReportZpz2025Dto>() };
            MapFromReportFlow(rep_flow, outReport);

            // Проход по всем темам отчета
            foreach (var themeData in rep_flow.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportZpz2025Dto { Theme = theme, Data = new List<ReportZpz2025DataDto>() };

                // Маппинг данных отчета
                var dataList = themeData.Report_Zpz2025.Select(MapThemeToPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        // Метод для преобразования данных темы отчета в объект DTO
        private ReportZpz2025DataDto MapThemeToPersist(Report_Zpz2025 data) =>
            new ReportZpz2025DataDto
            {
                Code = data.RowNum,
                CountSmo = data.CountSmo ?? 0,
                CountSmoAnother = data.CountSmoAnother ?? 0,
                CountAssignment = data.CountAssignment ?? 0,
                CountInsured = data.CountInsured ?? 0,
                CountInsuredRepresentative = data.CountInsuredRepresentative ?? 0,
                CountTfoms = data.CountTfoms ?? 0,
                CountProsecutor = data.CountProsecutor ?? 0,
                CountOutOfSmo = data.CountOutOfSmo ?? 0,
                CountAmbulatory = data.CountAmbulatory ?? 0,
                CountDs = data.CountDs ?? 0,
                CountDsVmp = data.CountDsVmp ?? 0,
                CountStac = data.CountStac ?? 0,
                CountStacVmp = data.CountStacVmp ?? 0,
                CountOutOfSmoAnother = data.CountOutOfSmoAnother ?? 0,
                CountAmbulatoryAnother = data.CountAmbulatoryAnother ?? 0,
                CountDsAnother = data.CountDsAnother ?? 0,
                CountDsVmpAnother = data.CountDsVmpAnother ?? 0,
                CountStacAnother = data.CountStacAnother ?? 0,
                CountStacVmpAnother = data.CountStacVmpAnother ?? 0
            };

        // Метод для преобразования данных темы отчета в объект базы данных
        private Report_Zpz2025 MapThemeToPersist(int idThemeData, ReportZpz2025DataDto data) =>
            new Report_Zpz2025
            {
                Id_Report_Data = idThemeData,
                RowNum = data.Code,
                CountSmo = data.CountSmo,
                CountSmoAnother = data.CountSmoAnother,
                CountAssignment = data.CountAssignment,
                CountInsured = data.CountInsured,
                CountInsuredRepresentative = data.CountInsuredRepresentative,
                CountTfoms = data.CountTfoms,
                CountProsecutor = data.CountProsecutor,
                CountOutOfSmo = data.CountOutOfSmo,
                CountAmbulatory = data.CountAmbulatory,
                CountDs = data.CountDs,
                CountDsVmp = data.CountDsVmp,
                CountStac = data.CountStac,
                CountStacVmp = data.CountStacVmp,
                CountOutOfSmoAnother = data.CountOutOfSmoAnother,
                CountAmbulatoryAnother = data.CountAmbulatoryAnother,
                CountDsAnother = data.CountDsAnother,
                CountDsVmpAnother = data.CountDsVmpAnother,
                CountStacAnother = data.CountStacAnother,
                CountStacVmpAnother = data.CountStacVmpAnother
            };
    }
}
