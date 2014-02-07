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

        public event LogEventHandler LogHandler;
        protected virtual void Log(string messageFormat, params object[] formatArgs)
        {
            if (LogHandler != null)
                LogHandler(this, messageFormat, formatArgs);
        }

        public void CreateDatabaseIfNotExists()
        {
            using (var context = new EFDbContext())
            {
                if (!context.Database.Exists())
                    context.Database.Create();
            }
        }

        public void ReCreateDatabase()
        {
            using (var context = new EFDbContext())
            {
                if (context.Database.Exists())
                    context.Database.Delete();
                context.Database.Create();
            }
        }

        public void LoadSummaries()
        {
            Log("start LoadSummaries");
            using (var cake = new Cake.Model.Staging.CakeStagingEntities())
            using (daDomain = new EFDbContext())
            {
                var pids = daDomain.Campaigns.Select(c => c.Pid).ToList();
                foreach (var pid in pids)
                {
                    Log("Pid {0}", pid);
                    var cakeSummaries = cake.DailySummaries.Where(ds => ds.offer_id == pid);
                    if (!cakeSummaries.Any())
                    {
                        Log("No cake summaries found; done");
                    }
                    else
                    {
                        var existingSummaries = daDomain.DailySummaries.Where(ds => ds.Pid == pid);
                        if (existingSummaries.Any())
                        {
                            // Try deleting today's and yesterday's summaries so they can be updated
                            var yesterday = DateTime.Today.AddDays(-1);
                            var recentSummaries = existingSummaries.Where(s => s.Date >= yesterday);
                            if (!recentSummaries.Any())
                            {   // Or... delete the most recent one if we have something to replace it with
                                // (if the updater stopped working for more than a day, the most recent one might not have complete stats)
                                var mostRecentExistingDate = existingSummaries.Max(es => es.Date);
                                if (cakeSummaries.Any(cs => cs.date == mostRecentExistingDate))
                                    recentSummaries = existingSummaries.Where(es => es.Date == mostRecentExistingDate);
                            }
                            if (recentSummaries.Any())
                            {
                                Log("Deleting most recent summaries");
                                foreach (var summary in recentSummaries)
                                {
                                    daDomain.DailySummaries.Remove(summary);
                                }
                                daDomain.SaveChanges();
                            }
                        }
                        // See if any existing summaries remain
                        if (!existingSummaries.Any())
                        {
                            Log("No existing summaries");
                        }
                        else
                        {
                            var maxDate = existingSummaries.Max(ds => ds.Date);
                            Log("Found existing summaries through {0}", maxDate);
                            cakeSummaries = cakeSummaries.Where(ds => ds.date > maxDate);
                        }
                        Log("Loading {0} new summaries", cakeSummaries.Count());
                        foreach (var cakeSummary in cakeSummaries)
                        {
                            var daSummary = new DirectAgents.Domain.Entities.Cake.DailySummary
                            {
                                Pid = cakeSummary.offer_id,
                                Date = cakeSummary.date,
                                Clicks = cakeSummary.clicks,
                                Conversions = cakeSummary.conversions,
                                Paid = cakeSummary.paid,
                                Sellable = cakeSummary.sellable,
                                Cost = cakeSummary.cost,
                                Revenue = cakeSummary.revenue
                            };
                            daDomain.DailySummaries.Add(daSummary);
                        }
                        daDomain.SaveChanges();
                        Log("done");
                    }
                }
            }
            Log("done LoadSummaries");
        }

        public void LoadCampaigns()
        {
            Log("start LoadCampaigns");
            using (var cake = new Cake.Model.Staging.CakeStagingEntities())
            using (daDomain = new EFDbContext())
            {
                Log("Updating verticals");
                UpdateVerticals(cake);
                Log("Updating traffic types");
                UpdateTrafficTypes(cake);

                var countryLookup = new CountryLookup(cake);

                Log("Going through cake staged campaigns...");
                var offerObjects = StagedOffers(cake);
                foreach (var item in offerObjects)
                {
                    var campaign = GetOrCreateCampaign(item);

                    campaign.Name = item.Offer.OfferName;
                    campaign.Vertical = daDomain.Verticals.Single(c => c.Name == item.Offer.VerticalName);

                    var offer = (Cake.Data.Wsdl.ExportService.offer1)item.Offer;

                    if (offer != null)
                    {
                        ExtractFieldsFromWsdlOffer(campaign, offer);
                        if (!daDomain.Statuses.Any(s => s.StatusId == offer.StatusId))
                        {
                            var status = new Status { StatusId = offer.StatusId, Name = offer.StatusName };
                            daDomain.Statuses.Add(status);
                        }
                    }
                    AddCountries(countryLookup[campaign.Pid], campaign);
                    AddTrafficTypes(item.Offer.AllowedMediaTypeNames.SplitCSV().Distinct(), campaign);
                    AddAccountManagers(item.Advertiser.AccountManagerName, campaign);
                    AddAdManagers(item.Advertiser.AdManagerName, campaign);
                    daDomain.SaveChanges();
                }
            }
            Log("done LoadCampaigns");
        }

        private static void ExtractFieldsFromWsdlOffer(Campaign campaign, offer1 offer)
        {
            campaign.ImageUrl = offer.offer_image_link;
            campaign.Description = string.IsNullOrWhiteSpace(offer.offer_description) ? "no description" : offer.offer_description;
            campaign.Link = offer.preview_link;
            campaign.Restrictions = offer.restrictions;
            campaign.Hidden = offer.hidden;
            campaign.StatusId = offer.StatusId;

            campaign.DefaultPriceFormat = offer.DefaultPriceFormat;

            offer_contract offer_contract = offer.offer_contracts.FirstOrDefault(oc => oc.offer_contract_id == offer.default_offer_contract_id);
            if (offer_contract != null)
            {
                campaign.RevenueIsPercentage = offer_contract.received.is_percentage;
                campaign.Revenue = offer_contract.received.amount;

                if (offer.DefaultPriceFormat == "RevShare")
                    campaign.Cost = offer_contract.payout.amount;
                else
                {   // compute cost as 2/3 of revenue
                    if (campaign.Revenue < 1.5m) // computed cost will be less than 1.00
                        campaign.Cost = decimal.Round(campaign.Revenue * (40m / 3m), 0) / 20; // round to nearest 0.05
                    //campaign.Cost = decimal.Round(campaign.Revenue * (2m / 3m), 2); // round to nearest 0.01
                    else
                        campaign.Cost = decimal.Round(campaign.Revenue * (8m / 3m), 0) / 4; // round to nearest 0.25
                }
            }
            campaign.CostCurrency = offer.currency.currency_symbol;
            campaign.RevenueCurrency = offer.currency.currency_symbol;
        }

        private void UpdateVerticals(CakeStagingEntities cake)
        {
            var verticals = daDomain.Verticals.ToList();
            var currentVerticals = verticals.Select(c => c.Name);

            var usedVerticalIds = daDomain.Campaigns.Select(c => c.Vertical.VerticalId).Distinct();
            var currentUnusedVerticals = daDomain.Verticals.Where(v => !usedVerticalIds.Contains(v.VerticalId)).Select(v => v.Name).ToList();

            var stagedVerticals = cake.CakeOffers.Select(o => o.VerticalName).Distinct();

            foreach (var item in currentUnusedVerticals.Except(stagedVerticals).Select(vn => verticals.Single(v => v.Name == vn)))
                daDomain.Verticals.Remove(item);

            foreach (var item in stagedVerticals.Except(currentVerticals))
                daDomain.Verticals.Add(new Vertical { Name = item });

            daDomain.SaveChanges();
        }

        private void UpdateTrafficTypes(CakeStagingEntities cake)
        {
            var trafficTypes = daDomain.TrafficTypes.ToList();
            var staged = cake.CakeOffers.ToList().SelectMany(c => c.AllowedMediaTypeNames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)).Distinct();
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

        private static IEnumerable<_OfferAndAdvertiser> StagedOffers(CakeStagingEntities cake)
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
            {
                campaign = new Campaign { Pid = item.Offer.Offer_Id, };
                daDomain.Campaigns.Add(campaign);
                Log("Pid {0} (adding)", campaign.Pid);
            }
            else
            {
                Log("Pid {0} (updating)", campaign.Pid);
            }

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
                var country = daDomain.Countries.Where(c => c.CountryCode == countryCode).FirstOrDefault();

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
