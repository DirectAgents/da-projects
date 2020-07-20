namespace RokuAPI.Constants
{
    public static class RokuConstants
    {
        public const string BaseApiUrl = "https://core.buysellads.com";

        public const string OrderListUrlFormat = "/lineitems?startingdate={0}T00:00:00.000Z&endingdate={1}T23:59:59.999Z";

        public const string OrdersStatsUrlFormat = "/stats/totals_by_order?order_id={0}&startingdate={1}T00:00:00.000Z&endingdate={2}T23:59:59.999Z&with_spend=on&breakdown=on&by_zone=";

        public const string ReferHeaderFormat = "https://selfserve.roku.com/orders?status=all&startDate={0}T00:00:00.000Z&endDate={1}T23:59:59.999Z";

        public const string CpmCompanyType = "CPM";

        public const string CpiCompanyType = "CPI";

        public const string CpcCompanyType = "CPC";
    }
}