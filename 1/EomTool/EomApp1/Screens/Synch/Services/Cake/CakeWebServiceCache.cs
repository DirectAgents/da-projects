using System.Collections.Generic;
using DAgents.Common;

namespace EomApp1.Screens.Synch.Services.Cake
{
    class CakeWebServiceCache
    {
        private readonly Dictionary<int, EomApp1.Cake.WebServices._3.Export.offer1> offersById;
        private readonly Dictionary<int, EomApp1.Cake.WebServices._4.Export.advertiser1> advertisersById;
        private readonly Dictionary<int, EomApp1.Cake.WebServices._4.Export.affiliate> affiliatesById;

        public CakeWebServiceCache()
        {
            this.offersById = new Dictionary<int, EomApp1.Cake.WebServices._3.Export.offer1>();
            this.advertisersById = new Dictionary<int, EomApp1.Cake.WebServices._4.Export.advertiser1>();
            this.affiliatesById = new Dictionary<int, EomApp1.Cake.WebServices._4.Export.affiliate>();
        }

        public bool ContainsOfferById(int offerID)
        {
            return this.offersById.ContainsKey(offerID);
        }

        public EomApp1.Cake.WebServices._3.Export.offer1 OfferById(int offerID)
        {
            return this.offersById[offerID];
        }

        public void OfferById(int offerID, EomApp1.Cake.WebServices._3.Export.offer1 offer)
        {
            this.offersById[offerID] = offer;
        }

        public bool ContainsAdvertiserById(int advertiserID)
        {
            return this.advertisersById.ContainsKey(advertiserID);
        }

        public EomApp1.Cake.WebServices._4.Export.advertiser1 AdvertiserById(int advertiserID)
        {
            return this.advertisersById[advertiserID];
        }

        public void AdvertiserById(int advertiserID, EomApp1.Cake.WebServices._4.Export.advertiser1 advertiser)
        {
            this.advertisersById[advertiserID] = advertiser;
        }

        public bool ContainsAffiliateById(int affiliateID)
        {
            return this.affiliatesById.ContainsKey(affiliateID);
        }

        public EomApp1.Cake.WebServices._4.Export.affiliate AffiliateById(int affiliateID)
        {
            return this.affiliatesById[affiliateID];
        }

        public void AffiliateById(int affiliateID, EomApp1.Cake.WebServices._4.Export.affiliate affiliate)
        {
            this.affiliatesById[affiliateID] = affiliate;
        }
    }
}
