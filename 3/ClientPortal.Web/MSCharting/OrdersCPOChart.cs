using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;

namespace MSCharting
{
    public class OrdersCPOChart : ChartBase
    {
        public OrdersCPOChart(IEnumerable stats) // IEnumerable<SearchStat>
        {
            string titleText = "Orders vs. CPO"; // make parameter/property?
            var builder = new TwoSeriesChartBuilder(titleText, "Orders", "CPO");

            builder.LeftSeries.ChartType = SeriesChartType.Column;
            builder.RightSeries.ChartType = SeriesChartType.Line;
            builder.RightSeries.BorderWidth = 4;
            builder.MainChartArea.AxisY2.LabelStyle.Format = "C";

            builder.LeftSeries.Points.DataBind(stats, "Title", "Orders", null);
            builder.RightSeries.Points.DataBind(stats, "Title", "CPO", null);

            this.ChartBuilder = builder;
            this.SetDefaults();
        }
    }
}
