using System;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Linq;
using DAgents.Common;
using EomApp1.Formss.Synch.Services.Cake;

namespace EomApp1.Formss.Synch.Models.Eom
{
    public partial class EomDatabaseEntities
    {
        public static EomDatabaseEntities Create()
        {
            var entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.Provider = "System.Data.SqlClient";
            // TODO: !!!switch this to use the configuration setting!!!
            entityBuilder.ProviderConnectionString = Properties.Settings.Default.DADatabaseR1ConnectionString + ";multipleactiveresultsets=True;App=EntityFramework";
            //entityBuilder.ProviderConnectionString = @"data source=biz2\da;initial catalog=zTestMay2012;integrated security=True;multipleactiveresultsets=True;App=EntityFramework";
            entityBuilder.Metadata = @"res://*/Formss.Synch.Models.Eom.EomModel.csdl|res://*/Formss.Synch.Models.Eom.EomModel.ssdl|res://*/Formss.Synch.Models.Eom.EomModel.msl";
            return new EomDatabaseEntities(entityBuilder.ConnectionString); 
        }
    }

    public static class EomDatabaseEntitiesExtensions
    {
        public static int GetOrCreate(this ObjectSet<Campaign> campaigns, int cakeOfferID, CakeWebService cakeService, ILogger logger)
        {
            var campaign = campaigns.Where(c => c.pid == cakeOfferID).FirstOrDefault();

            if (campaign == null)
            {
                campaign = new Campaign(campaigns.Context as EomDatabaseEntities, cakeOfferID, cakeService, logger);
            }

            return campaign.pid;
        }

        public static int GetOrCreate(this ObjectSet<Affiliate> affiliates, int cakeAffiliateID, CakeWebService cakeService, ILogger logger)
        {
            var affilaite = affiliates.Where(c => c.affid == cakeAffiliateID).FirstOrDefault();

            if (affilaite == null)
            {
                affilaite = new Affiliate(affiliates.Context as EomDatabaseEntities, cakeAffiliateID, cakeService, logger);
            }

            return affilaite.affid;
        }

        public static int GetOrCreate(this ObjectSet<AccountManager> accountManagers, int cakeOfferID, CakeWebService cakeService)
        {
            var advertiser = AdvertiserByOfferId(cakeOfferID, cakeService);

            string accountManagerName = advertiser.AccountManagerName;

            var accountManager = accountManagers.ToList().FirstOrDefault(c => c.NameIsEquivalentTo(accountManagerName));

            if (accountManager == null)
            {
                accountManager = new AccountManager(accountManagers.Context as EomDatabaseEntities, accountManagerName);
            }

            return accountManager.id;
        }

        public static int GetOrCreate(this ObjectSet<AdManager> adManagers, int cakeOfferID, CakeWebService cakeService)
        {
            var advertiser = AdvertiserByOfferId(cakeOfferID, cakeService);

            string adManagerName = advertiser.AdManagerName;

            var adManager = adManagers.ToList().FirstOrDefault(c => c.NameIsEquivalentTo(adManagerName));

            if (adManager == null)
            {
                adManager = new AdManager(adManagers.Context as EomDatabaseEntities, adManagerName);
            }

            return adManager.id;
        }

        public static int GetOrCreate(this ObjectSet<Advertiser> advertisers, int cakeOfferID, CakeWebService cakeService)
        {
            var advertiser = AdvertiserByOfferId(cakeOfferID, cakeService);

            string advertiserName = advertiser.AdManagerName;

            var newAdvertiser = advertisers.FirstOrDefault(c => c.name == advertiserName);

            if (newAdvertiser == null)
            {
                newAdvertiser = new Advertiser(advertisers.Context as EomDatabaseEntities, advertiserName);
            }

            return newAdvertiser.id;
        }

        public static int GetOrCreate(this ObjectSet<MediaBuyer> mediaBuyers, EomApp1.Cake.WebServices._4.Export.affiliate affiliate)
        {
            string mediaBuyerName = affiliate.AccountManagerName;
            var mediaBuyer = mediaBuyers.ToList().FirstOrDefault(c => c.NameIsEquivalentTo(mediaBuyerName));
            if (mediaBuyer == null)
            {
                mediaBuyer = new MediaBuyer(mediaBuyers.Context as EomDatabaseEntities, mediaBuyerName);
            }
            return mediaBuyer.id;
        }

        public static int IdByName(this ObjectSet<Currency> currencies, string currencyName)
        {
            var currency = currencies.FirstOrDefault(c => c.name == currencyName);

            if (currency == null)
            {
                throw new Exception("The currency named " + currencyName + " does not exist.");
            }

            return currency.id;
        }

        public static int IdByName(this ObjectSet<NetTermType> netTermTypes, string netTermTypeName)
        {
            var netTermType = netTermTypes.FirstOrDefault(c => c.name == netTermTypeName);

            if (netTermType == null)
            {
                throw new Exception("The net term type named " + netTermTypeName + " does not exist.");
            }

            return netTermType.id;
        }

        public static int IdByName(this ObjectSet<AffiliatePaymentMethod> affiliatePaymentMethods, string affiliatePaymentMethodName)
        {
            var affiliatePaymentMethod = affiliatePaymentMethods.FirstOrDefault(c => c.name == affiliatePaymentMethodName);

            if (affiliatePaymentMethod == null)
            {
                throw new Exception("The affiliate payment method named " + affiliatePaymentMethodName + " does not exist.");
            }

            return affiliatePaymentMethod.id;
        }

        private static EomApp1.Cake.WebServices._4.Export.advertiser1 AdvertiserByOfferId(int cakeOfferID, CakeWebService cakeService)
        {
            var offer = cakeService.OfferById(cakeOfferID);

            var advertiser = cakeService.AdvertiserById(offer.advertiser.advertiser_id);

            return advertiser;
        }
    }
}
