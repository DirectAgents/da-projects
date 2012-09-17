using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities;

namespace DirectAgents.Domain.Concrete
{
    public class AdminImpl : IAdmin
    {
        private EFDbContext daDomain;

        public void CreateDatabaseIfNotExists()
        {
            using (var context = new EFDbContext())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                }
            }
        }

        public void ReCreateDatabase()
        {
            using (var context = new EFDbContext())
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
                context.Database.Create();
            }
        }

        public void LoadCampaigns()
        {
            using (var cake = new Cake.Model.Staging.CakeStagingEntities())
            using (daDomain = new EFDbContext())
            {
                UpdateVerticals(cake);

                foreach (var item in StagedCampaigns(cake))
                {
                    var campaign = GetOrCreateCampaign(item);

                    campaign.Name = item.Offer.OfferName;

                    campaign.TrafficType = item.Offer.AllowedMediaTypeNames;

                    campaign.Vertical = daDomain.Verticals.Single(c => c.Name == item.Offer.VerticalName);

                    if (string.IsNullOrWhiteSpace(campaign.PayableAction))
                        campaign.PayableAction = "Not Specified";

                    var offer = (Cake.Data.Wsdl.ExportService.offer1)item.Offer;
                    if (offer != null)
                        ExtractFieldsFromWsdlOffer(campaign, offer);

                    AddCountries(item, campaign);

                    AddAccountManagers(item, campaign);

                    AddAdManagers(item, campaign);

                    daDomain.SaveChanges(); // save created entities so they are re-used
                }
            }
        }

        private void UpdateVerticals(Cake.Model.Staging.CakeStagingEntities cake)
        {
            var verticals = daDomain.Verticals.ToList();
            var staged = cake.CakeOffers.Select(c => c.VerticalName).Distinct();
            var current = verticals.Select(c => c.Name);

            foreach (var item in current.Except(staged).Select(c => verticals.Single(d => d.Name == c)))
                daDomain.Verticals.Remove(item);

            foreach (var item in staged.Except(current))
                daDomain.Verticals.Add(new Vertical { Name = item });

            daDomain.SaveChanges();
        }

        private class _OfferAndAdvertiser
        {
            public Cake.Model.Staging.CakeOffer Offer { get; set; }
            public Cake.Model.Staging.CakeAdvertiser Advertiser { get; set; }
        }

        private static IEnumerable<_OfferAndAdvertiser> StagedCampaigns(Cake.Model.Staging.CakeStagingEntities cake)
        {
            var query = from offer in cake.CakeOffers.ToList()
                        from advertiser in cake.CakeAdvertisers.ToList()
                        where advertiser.Advertiser_Id == int.Parse(offer.Advertiser_Id)
                        select new _OfferAndAdvertiser { Offer = offer, Advertiser = advertiser };

            return query;
        }

        Campaign GetOrCreateCampaign(_OfferAndAdvertiser item)
        {
            var campaign = daDomain.Campaigns.FirstOrDefault(c => c.Pid == item.Offer.Offer_Id);
            if (campaign == null)
                campaign = NewCampaign(item, campaign);
            return campaign;
        }

        private Campaign NewCampaign(_OfferAndAdvertiser item, Campaign campaign)
        {
            campaign = new Campaign { Pid = item.Offer.Offer_Id, };
            daDomain.Campaigns.Add(campaign);
            return campaign;
        }

        private static void ExtractFieldsFromWsdlOffer(Campaign campaign, Cake.Data.Wsdl.ExportService.offer1 offer)
        {
            campaign.Cost = offer.offer_contracts[0].payout.amount;
            campaign.ImageUrl = offer.offer_image_link;
            campaign.Description = string.IsNullOrWhiteSpace(offer.offer_description) ? "no description" : offer.offer_description;
            campaign.Link = offer.offer_contracts[0].offer_link;
            campaign.Cost = offer.offer_contracts[0].payout.amount;
            campaign.Revenue = offer.offer_contracts[0].received.amount;
        }

        // TODO: support multiple admangers
        private void AddAdManagers(_OfferAndAdvertiser item, Campaign campaign)
        {
            var ad = daDomain.People
                        .Where(p => p.Name == item.Advertiser.AdManagerName)
                        .FirstOrDefault();

            if (ad == null)
                ad = new Person { Name = item.Advertiser.AdManagerName };

            if (campaign.AdManagers != null)
            {
                campaign.AdManagers.Clear();
                campaign.AdManagers.Add(ad);
            }
        }

        // TODO: support multiple accountmangers
        private void AddAccountManagers(_OfferAndAdvertiser item, Campaign campaign)
        {
            var am = daDomain.People
                        .Where(p => p.Name == item.Advertiser.AccountManagerName)
                        .FirstOrDefault();

            if (am == null)
            {
                am = new Person { Name = item.Advertiser.AccountManagerName };
            }

            if (campaign.AccountManagers != null)
            {
                campaign.AccountManagers.Clear();
                campaign.AccountManagers.Add(am);
            }
        }

        private void AddCountries(_OfferAndAdvertiser item, Campaign campaign)
        {
            campaign.Countries.Clear();

            var countryCodes = item.Offer.AllowedCountries
                                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Distinct();

            foreach (var countryCode in countryCodes)
            {
                var country = daDomain.Countries.Where(c => c.CountryCode == countryCode)
                                .FirstOrDefault();

                if (country == null)
                    country = new Country
                    {
                        CountryCode = countryCode,
                        Name = countryCode + "."
                    };

                campaign.Countries.Add(country);
            }
        }
    }
}
