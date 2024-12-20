﻿using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class CreativesClient : ApiClient
    {
        public CreativesClient()
            : base(3, "export", "Creatives")
        {
        }

        public CreativeExportResponse Creatives(CreativesRequest request)
        {
            var result = TryGetResponse<CreativeExportResponse>(request);
            return result;
        }
    }
}
