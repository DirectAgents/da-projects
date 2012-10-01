using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Data.Wsdl.ExportService;
using Cake.Model.Staging;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Cake;

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

        public void LoadSummaries()
        {
            List<MonthlySummary> summaries;
            using (var cake = new Cake.Model.Staging.CakeStagingEntities())
            {
                var query = from d in cake.DailySummaries
                            group d by d.offer_id into g
                            select new MonthlySummary
                            {
                                pid = g.Key,
                                date = g.Max(s => s.date),
                                clicks = g.Sum(s => s.clicks),
                                conversions = g.Sum(s => s.conversions),
                                paid = g.Sum(s => s.paid),
                                sellable = g.Sum(s => s.sellable),
                                cost = g.Sum(s => s.cost),
                                revenue = g.Sum(s => s.revenue)
                            };
                summaries = query.ToList();
            }
            using (daDomain = new EFDbContext())
            {
                foreach (var monthlySummary in summaries)
                {
                    daDomain.MonthlySummaries.Add(monthlySummary);
                }
                daDomain.SaveChanges();
            }
        }

        public void LoadCampaigns()
        {
            using (var cake = new Cake.Model.Staging.CakeStagingEntities())
            using (daDomain = new EFDbContext())
            {
                UpdateVerticals(cake);

                UpdateTrafficTypes(cake);

                foreach (var item in StagedCampaigns(cake))
                {
                    var campaign = GetOrCreateCampaign(item);

                    campaign.Name = item.Offer.OfferName;

                    campaign.Vertical = daDomain.Verticals.Single(c => c.Name == item.Offer.VerticalName);

                    if (string.IsNullOrWhiteSpace(campaign.PayableAction))
                        campaign.PayableAction = "Not Specified";

                    var offer = (Cake.Data.Wsdl.ExportService.offer1)item.Offer;
                    if (offer != null)
                        ExtractFieldsFromWsdlOffer(campaign, offer);

                    AddCountries(item.Offer.AllowedCountries.SplitCSV().Distinct(), campaign);

                    AddTrafficTypes(item.Offer.AllowedMediaTypeNames.SplitCSV().Distinct(), campaign);

                    AddAccountManagers(item.Advertiser.AccountManagerName, campaign);

                    AddAdManagers(item.Advertiser.AdManagerName, campaign);

                    daDomain.SaveChanges(); // save created entities so they are re-used
                }
            }
        }

        private static void ExtractFieldsFromWsdlOffer(Campaign campaign, offer1 offer)
        {
            campaign.ImageUrl = offer.offer_image_link;
            campaign.Description = string.IsNullOrWhiteSpace(offer.offer_description) ? "no description" : offer.offer_description;
            campaign.Link = offer.offer_contracts[0].offer_link;
            campaign.Cost = offer.offer_contracts[0].payout.amount;
            campaign.Revenue = offer.offer_contracts[0].received.amount;
            campaign.CostCurrency = offer.currency.currency_symbol;
            campaign.RevenueCurrency = offer.currency.currency_symbol;
            campaign.ImportantDetails = offer.restrictions;
        }

        private void UpdateVerticals(CakeStagingEntities cake)
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

        private void UpdateTrafficTypes(CakeStagingEntities cake)
        {
            var trafficTypes = daDomain.TrafficTypes.ToList();
            var staged = cake.CakeOffers.ToList().SelectMany(c => c.AllowedMediaTypeNames
                                                                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                                                .Distinct();
            var current = trafficTypes.Select(c => c.Name);
            foreach (var item in current.Except(staged).Select(c => trafficTypes.Single(d => d.Name == c)))
                daDomain.TrafficTypes.Remove(item);
            foreach (var item in staged.Except(current))
                daDomain.TrafficTypes.Add(new TrafficType { Name = item });
            daDomain.SaveChanges();
        }

        private class _OfferAndAdvertiser
        {
            public CakeOffer Offer { get; set; }
            public CakeAdvertiser Advertiser { get; set; }
        }

        private static IEnumerable<_OfferAndAdvertiser> StagedCampaigns(CakeStagingEntities cake)
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
        
        // TODO: support multiple accountmangers
        private void AddAccountManagers(string accountManagerName, Campaign campaign)
        {
            var am = daDomain.People.Where(p => p.Name == accountManagerName).FirstOrDefault();
            if (am == null)
                am = new Person { Name = accountManagerName };
            campaign.AccountManagers.Clear();
            campaign.AccountManagers.Add(am);
        }

        // TODO: support multiple admangers
        private void AddAdManagers(string adManagerName, Campaign campaign)
        {
            var ad = daDomain.People.Where(p => p.Name == adManagerName).FirstOrDefault();
            if (ad == null)
                ad = new Person { Name = adManagerName };
            if (campaign.AdManagers != null)
            {
                campaign.AdManagers.Clear();
                campaign.AdManagers.Add(ad);
            }
        }

        private void AddCountries(IEnumerable<string> countryCodes, Campaign campaign)
        {
            campaign.Countries.Clear();
            foreach (var countryCode in countryCodes)
            {
                var country = daDomain.Countries.Where(c => c.CountryCode == countryCode) .FirstOrDefault();
                if (country == null)
                    country = new Country { CountryCode = countryCode, Name = countryCode + "." };
                campaign.Countries.Add(country);
            }
        }

        private void AddTrafficTypes(IEnumerable<string> trafficTypeNames, Campaign campaign)
        {
            campaign.TrafficTypes.Clear();
            foreach (var trafficTypeName in trafficTypeNames)
            {
                var trafficType = daDomain.TrafficTypes.Where(c => c.Name == trafficTypeName).FirstOrDefault();
                if (trafficType == null)
                    trafficType = new TrafficType { Name = trafficTypeName };
                campaign.TrafficTypes.Add(trafficType);
            }
        }
    }
}
