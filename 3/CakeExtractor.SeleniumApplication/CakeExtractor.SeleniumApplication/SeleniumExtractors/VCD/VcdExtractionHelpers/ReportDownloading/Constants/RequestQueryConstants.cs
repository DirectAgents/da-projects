using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.ReportDownloading.Constants
{
    internal class RequestQueryConstants
    {
        private const string tokenQueryParameter = "token";

        private const string vendorGroupIdParameter = "vgId";

        private const string mcIdQueryParameter = "mcId";

        private const string productQueryParameter = "product";

        private const string vendorCentralDataProductName = "ara";

        public static Dictionary<string, string> GetRequestQueryParameters(string tokenValue, string vendorGroupId, string mcId)
        {
            return new Dictionary<string, string>()
            {
                {tokenQueryParameter, tokenValue },
                {vendorGroupIdParameter, vendorGroupId },
                {mcIdQueryParameter, mcId },
                {productQueryParameter, vendorCentralDataProductName }
            };
        }
    }
}
