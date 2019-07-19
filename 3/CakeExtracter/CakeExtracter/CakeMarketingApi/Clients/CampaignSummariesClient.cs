using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class CampaignSummariesClient : ApiClient
    {
        public CampaignSummariesClient()
            : base(5, "reports", "CampaignSummary")
        {
        }

        public CampaignSummaryResponse CampaignSummaries(CampaignSummariesRequest request)
        {
            var result = TryGetResponse<CampaignSummaryResponse>(request);
            return result;
        }
    }
}
