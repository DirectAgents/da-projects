using System;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Models
{
    /// <summary>
    /// Latests data for platform accounts.
    /// </summary>
    public class CustomSpecialPlatformLatestsSummary : SpecialPlatformLatestsSummary
    {
        public DateTime? EarliestDateNetPpmWeekly { get; set; }

        public DateTime? LatestDateNetPpmWeekly { get; set; }

        public DateTime? EarliestDateNetPpmMonthly { get; set; }

        public DateTime? LatestDateNetPpmMonthly { get; set; }

        public DateTime? EarliestDateNetPpmYearly { get; set; }

        public DateTime? LatestDateNetPpmYearly { get; set; }

        public DateTime? EarliestDateRepeatPurchaseMonthly { get; set; }

        public DateTime? LatestDateRepeatPurchaseMonthly { get; set; }

        public DateTime? EarliestDateRepeatPurchaseQuarterly { get; set; }

        public DateTime? LatestDateRepeatPurchaseQuarterly { get; set; }

        public DateTime? EarliestDateItemComp { get; set; }

        public DateTime? LatestDateItemComp { get; set; }

        public DateTime? EarliestDateAltPurchase { get; set; }

        public DateTime? LatestDateAltPurchase { get; set; }

        public DateTime? EarliestDateMarketBasket { get; set; }

        public DateTime? LatestDateMarketBasket { get; set; }

        public DateTime? EarliestDateGeoSales { get; set; }

        public DateTime? LatestDateGeoSales { get; set; }

    }
}
