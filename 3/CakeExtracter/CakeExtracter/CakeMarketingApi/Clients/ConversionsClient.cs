using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class ConversionsClient : ApiClient
    {
        public ConversionsClient()
            : base(8, "reports", "Conversions")
        {
        }

        public ConversionReportResponse Conversions(ConversionsRequest request)
        {
            var result = Execute<ConversionReportResponse>(request);
            return result;
        }
    }
}