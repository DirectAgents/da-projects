using System.Windows.Forms.DataVisualization.Charting;

namespace MSCharting
{
    public interface IChartBuilder
    {
        Chart Chart { get; }
        ChartArea MainChartArea { get; }
    }
}
