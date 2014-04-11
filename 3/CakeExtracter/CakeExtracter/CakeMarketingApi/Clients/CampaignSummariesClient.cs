using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class CampaignSummariesClient : ApiClient
    {
        public CampaignSummariesClient()
            : base(2, "reports", "CampaignSummary")
        {
        }

        public CampaignSummaryResponse CampaignSummaries(CampaignSummariesRequest request)
        {
            var result = Execute<CampaignSummaryResponse>(request);
            return result;
        }
    }
}
