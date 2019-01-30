using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DSP.Models
{
    internal class DspReportMetricItem
    {
        public string Name { get; set; }

        public decimal TotalCost { get; set; }

        public decimal Impressions { get; set; }

        public decimal ClickThroughs { get; set; }

        public decimal TotalPixelEvents { get; set; }

        public decimal TotalPixelEventsViews { get; set; }

        public decimal TotalPixelEventsClicks { get; set; }

        public decimal DPV { get; set; }

        public decimal ATC { get; set; }

        public decimal Purchase { get; set; }

        public decimal PurchaseViews { get; set; }

        public decimal PurchaseClicks { get; set; }
    }
}
