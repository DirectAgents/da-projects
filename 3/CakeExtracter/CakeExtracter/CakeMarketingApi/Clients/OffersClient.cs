﻿using CakeExtracter.CakeMarketingApi.Entities;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class OffersClient : ApiClient
    {
        public OffersClient()
            : base(5, "export", "Offers")
        {
        }

        public OfferExportResponse Offers(OffersRequest request)
        {
            var result = TryGetResponse<OfferExportResponse>(request);
            return result;
        }
    }
}