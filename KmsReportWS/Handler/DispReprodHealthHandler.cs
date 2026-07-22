using System;
using System.Collections.Generic;
using System.Linq;
using KmsReportWS.LinqToSql;
using KmsReportWS.Model;
using KmsReportWS.Model.Report;
using KmsReportWS.Properties;

namespace KmsReportWS.Handler
{
    // Обработчик отчетов для типа DispReprodHealthHandler
    public class DispReprodHealthHandler : BaseReportHandler
    {
        // Строка подключения к базе данных
        private readonly string _connStr = Settings.Default.ConnStr;

        // Конструктор, принимающий тип отчета
        public DispReprodHealthHandler(ReportType reportType) : base(reportType)
        {
        }

        // Метод для вставки нового отчета в базу данных (пока не реализован)
        protected override void InsertReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        { }

        // Получение данных за указанный год по теме и филиалу
        public ReportDispReprodHealthDataDto GetYearData(string yymm, string theme, string fillial, string rowNum)
        {
            var db = new LinqToSqlKmsReportDataContext(_connStr);

            // Определяем начальную дату для выборки
            string start = yymm.Substring(0, 2) + "01";
            var result = db.Report_DispReprodHealth.Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
            && x.Report_Data.Theme == theme
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
            && x.Report_Data.Report_Flow.Id_Report_Type == "dRepHeal"
            && x.RowNum == rowNum
            ).GroupBy(x => x.Report_Data.Theme).
            Select(x => new ReportDispReprodHealthDataDto
            {
                ForPeriod = (int)x.Sum(g => g.ForPeriod)

            }).FirstOrDefault();

            return result;
        }

        public List<DispRepHealthYearDataRow> GetYearDataBatch(string yymm, string theme, string fillial, string[] rowNumbers)
        {
            if (rowNumbers == null || rowNumbers.Length == 0)
                return new List<DispRepHealthYearDataRow>();

            var db = new LinqToSqlKmsReportDataContext(_connStr);
            string start = yymm.Substring(0, 2) + "01";
            var rowNumbersArray = rowNumbers;

            var dbResults = db.Report_DispReprodHealth
               .Where(x => x.Report_Data.Report_Flow.Id_Region == fillial
                            && x.Report_Data.Theme == theme
                            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) >= Convert.ToInt32(start)
                            && Convert.ToInt32(x.Report_Data.Report_Flow.Yymm) <= Convert.ToInt32(yymm)
                            && x.Report_Data.Report_Flow.Id_Report_Type == "dRepHeal"
                            && rowNumbersArray.Contains(x.RowNum)) // ← Фильтрация по списку
        .GroupBy(x => x.RowNum)
        .ToDictionary(g => g.Key, g => (int)g.Sum(x => x.ForPeriod));

            var fullList = new List<DispRepHealthYearDataRow>();
            foreach (var rn in rowNumbers)
            {
                var count = dbResults.TryGetValue(rn, out var sum) ? sum : 0;
                fullList.Add(new DispRepHealthYearDataRow
                {
                    RowNum = rn,
                    Data = new ReportDispReprodHealthDataDto { ForPeriod = count }
                });
            }

            return fullList;
        }


        // Метод для создания нового отчета в базе данных
        protected override void CreateNewReport(LinqToSqlKmsReportDataContext db, Report_Flow flow,
            AbstractReport inReport)
        {
            var report = inReport as ReportDispReprodHealth ?? throw new Exception("Error saving new report, because getting empty report");

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
                var DataList = reportForms.Data.Select(data => MapThemeToPersist(themeData.Id, data)).ToList();
                if (DataList.Any())
                {
                    db.Report_DispReprodHealth.InsertAllOnSubmit(DataList);
                }
                db.SubmitChanges();
            }
        }

        // Метод для обновления существующего отчета в базе данных
        protected override void UpdateReport(LinqToSqlKmsReportDataContext db, AbstractReport inReport)
        {
            var report = inReport as ReportDispReprodHealth ?? throw new Exception("Error update report, because getting empty report");

            // Проход по всем формам отчетов
            foreach (var reportForms in report.ReportDataList)
            {
                // Получение ID темы отчета из базы данных
                var idTheme = db.Report_Data
                    .SingleOrDefault(x => x.Id_Flow == inReport.IdFlow && x.Theme == reportForms.Theme)?.Id;
                if (idTheme != null)
                {
                    // Удаление существующих данных отчета
                    var dataReport = db.Report_DispReprodHealth.Where(x => x.Id_Report_Data == idTheme);
                    db.Report_DispReprodHealth.DeleteAllOnSubmit(dataReport);
                    db.SubmitChanges();

                    // Подготовка новых данных для вставки
                    var DataList = reportForms.Data.Select(data => MapThemeToPersist(idTheme.Value, data)).ToList();
                    if (DataList.Any())
                    {
                        db.Report_DispReprodHealth.InsertAllOnSubmit(DataList);
                    }

                    db.SubmitChanges();
                }
            }
        }

        // Метод для маппинга данных отчета из базы данных в объект отчета
        protected override AbstractReport MapReportFromPersist(Report_Flow rep_flow)
        {
            var outReport = new ReportDispReprodHealth { ReportDataList = new List<ReportDispReprodHealthDto>() };
            MapFromReportFlow(rep_flow, outReport);

            // Проход по всем темам отчета
            foreach (var themeData in rep_flow.Report_Data)
            {
                var theme = themeData.Theme.Trim();
                var dto = new ReportDispReprodHealthDto { Theme = theme, Data = new List<ReportDispReprodHealthDataDto>() };

                // Маппинг данных отчета
                var dataList = themeData.Report_DispReprodHealth.Select(MapThemeToPersist);
                dto.Data.AddRange(dataList);

                outReport.ReportDataList.Add(dto);
            }

            return outReport;
        }

        // Метод для преобразования данных темы отчета в объект DTO
        private ReportDispReprodHealthDataDto MapThemeToPersist(Report_DispReprodHealth data) =>
            new ReportDispReprodHealthDataDto
            {
                Code = data.RowNum,
                YearlySum = data.YearlySum ?? 0,
                ForPeriod = data.ForPeriod ?? 0
                
            };

        // Метод для преобразования данных темы отчета в объект базы данных
        private Report_DispReprodHealth MapThemeToPersist(int idThemeData, ReportDispReprodHealthDataDto data) =>
            new Report_DispReprodHealth
            {
                Id_Report_Data = idThemeData,
                RowNum = data.Code,
                YearlySum = data.YearlySum,
                ForPeriod = data.ForPeriod
            };
    }
}
