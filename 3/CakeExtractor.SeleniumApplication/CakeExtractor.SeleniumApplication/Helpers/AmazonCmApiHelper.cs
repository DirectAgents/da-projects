using CakeExtractor.SeleniumApplication.Models.ConsoleManagerUtilityModels;
using System;
using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    internal class AmazonCmApiHelper
    {
        public const string AmazonAdvertisingPortalUrl = "https://advertising.amazon.com";
        public const string CampaignsApiRelativePath = "/cm/api/campaigns";
        public const string EntityIdArgName = "entityId";

        private const string CampaignNameField = "CAMPAIGN_NAME";
        private const string ImpressionsField = "IMPRESSIONS";
        private const string ClicksField = "CLICKS";
        private const string SpendField = "SPEND";
        private const string SalesField = "SALES";
        private const string OrdersField = "ORDERS";
        private const string CampaignEligibilityStatusField = "CAMPAIGN_ELIGIBILITY_STATUS";
        private const string CampaignSmartBiddingStrategyField = "CAMPAIGN_SMART_BIDDING_STRATEGY";
        private const string BidAdjustmentField = "BID_ADJUSTMENT_PERCENTAGE";
        private const string PortfolioNameField = "PORTFOLIO_NAME";
        private const string CtrField = "CTR";
        private const string CpcField = "CPC";
        private const string AcosField = "ACOS";

        private static readonly Filter CampaignStateFilter = new Filter
        {
            field = "CAMPAIGN_STATE",
            not = false,
            @operator = "EXACT",
            values = new[] {"ENABLED", "PAUSED", "ARCHIVED"}
        };

        private static readonly Filter CampaignPdaProgramTypeFilter = new Filter
        {
            field = "CAMPAIGN_PROGRAM_TYPE",
            not = false,
            @operator = "EXACT",
            values = new[] {"PDA", "SPARK"}
        };

        private static readonly string[] CampaignFieldsForRequest =
        {
            OrdersField
        };

        private static readonly Filter[] CampaignPdaFilters =
        {
            CampaignStateFilter, CampaignPdaProgramTypeFilter
        };

        public static Dictionary<string, string> GetCampaignsApiQueryParams(string accountEntityId)
        {
            var parameters = new Dictionary<string, string>
            {
                {EntityIdArgName, accountEntityId}
            };
            return parameters;
        }

        public static AmazonCmApiParams GetBasePdaCampaignsApiParams()
        {
            var parameters = new AmazonCmApiParams
            {
                period = "CUSTOM",
                interval = "SUMMARY",
                programType = "SP",
                queries = new string[0],
                fields = CampaignFieldsForRequest,
                filters = CampaignPdaFilters,
            };
            return parameters;
        }

        public static void SetCampaignApiSpecificInitParams(AmazonCmApiParams parameters, DateTime date, int pageSize)
        {
            parameters.startDateUTC = TimeHelper.ConvertToUnixTimestamp(date);
            var endDate = date.AddDays(1).AddMilliseconds(-1);
            parameters.endDateUTC = TimeHelper.ConvertToUnixTimestamp(endDate);
            parameters.pageSize = pageSize;
            parameters.pageOffset = 0;
        }

        public static dynamic GetDynamicCampaigns(dynamic data)
        {
            return data["campaigns"];
        }

        public static int GetNumberOfRecords(dynamic data)
        {
            return (int) data["summary"]["numberOfRecords"];
        }

        public static AmazonCmApiCampaignSummary ConvertDynamicCampaignInfoToModel(dynamic data)
        {
            var campaignInfo = new AmazonCmApiCampaignSummary
            {
                Id = data["id"],
                Orders = data["orders"]
            };
            return campaignInfo;
        }
    }
}
