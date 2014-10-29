using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace DAGenerators.Spreadsheets
{
    public partial class SearchReportPPC
    {
        public void Setup(string templateName)
        {
            bool useDefaultTemplate = false;
            switch (templateName)
            {
                case "ScholasticTeacherExpress":
                    Metric_Revenue = new Metric(3, "Revenue");
                    Metric_Cost = new Metric(4, "Cost");
                    Metric_ROAS = new Metric(5, "ROAS");
                    Metric_Orders = new Metric(6, "Orders");
                    Metric_Impressions = new Metric(7, "Impressions");
                    Metric_Clicks = new Metric(8, "Clicks");
                    Metric_CTR = new Metric(9, "CTR");
                    Metric_OrderRate = new Metric(10, "Order Rate");
                    Metric_CPC = new Metric(11, "CPC");
                    Metric_CPO = new Metric(12, "CPO");
                    StartRow_Monthly = 12;
                    StartRow_Weekly = 13;
                    break;
                default:
                    useDefaultTemplate = true;
                    Metric_Clicks = new Metric(3, "Clicks");
                    Metric_Impressions = new Metric(4, "Impressions");
                    Metric_Orders = new Metric(5, "Orders");
                    Metric_Cost = new Metric(7, "Cost");
                    Metric_Revenue = new Metric(8, "Revenue");

                    Metric_OrderRate = new Metric(6, "Order Rate");
                    Metric_Net = new Metric(9, "Net");
                    Metric_RevPerOrder = new Metric(10, "Revenue/Order");
                    Metric_CTR = new Metric(11, "CTR");
                    Metric_CPC = new Metric(12, "CPC");
                    Metric_CPO = new Metric(13, "CPO");
                    Metric_ROAS = new Metric(14, "ROAS");
                    Metric_ROI = new Metric(15, "ROI");
                    break;
            }
            if (!useDefaultTemplate)
                this.TemplateFilename = "SearchTemplate_" + templateName + ".xlsx";
        }

        public void CreateWeeklyChart_RevenueVsClicks()
        {
            CreateWeeklyChart(NumWeeksAdded, Metric_Revenue.ColNum, Metric_Revenue.DisplayName, Metric_Clicks.ColNum, Metric_Clicks.DisplayName);
        }
        private void CreateWeeklyChart(int numWeeks, int series1column, string series1name, int series2column, string series2name)
        {
            var chart = WS.Drawings.AddChart("chartWeekly", eChartType.ColumnClustered);
            chart.SetPosition(Row_WeeklyChart + NumWeeksAdded + NumMonthsAdded, 0, 1, 0);
            chart.SetSize(1071, 217);

            chart.Title.Text = "Weekly Performance"; // TODO: add year; remember: handle when it's two years
            chart.Title.Font.Bold = true;
            //chart.Title.Anchor = eTextAnchoringType.Bottom;
            //chart.Title.AnchorCtr = false;

            int startRow_Weekly = this.StartRow_Weekly + (WeeklyFirst ? 0 : NumMonthsAdded);

            var series = chart.Series.Add(new ExcelAddress(startRow_Weekly, series1column, startRow_Weekly + numWeeks - 1, series1column).Address,
                                          new ExcelAddress(startRow_Weekly, Col_StatsTitle, startRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series.HeaderAddress = new ExcelAddress(Row_StatsHeader, column1, Row_StatsHeader, column1);
            series.Header = series1name;

            var chartType2 = chart.PlotArea.ChartTypes.Add(eChartType.LineMarkers);
            chartType2.UseSecondaryAxis = true;
            chartType2.XAxis.Deleted = true;
            var series2 = chartType2.Series.Add(new ExcelAddress(startRow_Weekly, series2column, startRow_Weekly + numWeeks - 1, series2column).Address,
                                                new ExcelAddress(startRow_Weekly, Col_StatsTitle, startRow_Weekly + numWeeks - 1, Col_StatsTitle).Address);
            //series2.HeaderAddress = new ExcelAddress(Row_StatsHeader, column2, Row_StatsHeader, column2);
            series2.Header = series2name;
        }

    }
}
