using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Cake;

namespace CakeExtracter.Etl.CakeMarketing.DALoaders
{
    public class DACampSumLoader : Loader<CampaignSummary>
    {
        private readonly DateTime date;
        public DACampSumLoader(DateTime date)
        {
            this.date = date;
            LoadCurrencies();
        }

        private Dictionary<string, int> currencyIdLookupByAbbr = new Dictionary<string, int>();

        //private HashSet<int> offerIdsSaved = new HashSet<int>();
        //public IEnumerable<int> OfferIdsSaved
        //{
        //    get { return offerIdsSaved; }
        //}
        //private HashSet<int> campIdsSaved = new HashSet<int>();
        //public IEnumerable<int> CampIdsSaved
        //{
        //    get { return campIdsSaved; }
        //}
        //private Dictionary<int, DirectAgents.Domain.Entities.Cake.Offer> offerLookupById = new Dictionary<int, DirectAgents.Domain.Entities.Cake.Offer>();
        //private Dictionary<int, Camp> campLookupById = new Dictionary<int, Camp>();

        protected override int Load(List<CampaignSummary> items)
        {
            Logger.Info("Loading {0} CampSums..", items.Count);
            AddMissingOffers(items);
            AddMissingCampaigns(items);
            var count = UpsertCampSums(items);
            return count;
        }

        // used to determine which items to "keep"
        private static Func<CampaignSummary, bool> KeepFunc =
            cs => (cs.Paid > 0 || cs.Revenue > 0 || cs.Cost > 0);

        private int UpsertCampSums(List<CampaignSummary> items)
        {
            var loaded = 0;
            var added = 0;
            var updated = 0;
            var deleted = 0;
            var alreadyDeleted = 0;
            using (var db = new DAContext())
            {
                bool toDelete;
                foreach (var item in items)
                {
                    toDelete = !KeepFunc(item);

                    var pk1 = item.Campaign.CampaignId;
                    var pk2 = this.date;
                    var target = db.CampSums.Find(pk1, pk2);

                    if (toDelete)
                    {
                        if (target == null)
                            alreadyDeleted++;
                        else
                        {
                            db.CampSums.Remove(target);
                            deleted++;
                        }
                    }
                    else //to add/update
                    {
                        if (target == null)
                        {
                            target = new CampSum
                            {
                                CampId = item.Campaign.CampaignId,
                                Date = this.date
                            };
                            item.CopyValuesTo(target);
                            db.CampSums.Add(target);
                            added++;
                        }
                        else
                        {   // update:
                            item.CopyValuesTo(target);
                            updated++;
                        }
                        //offerIdsSaved.Add(target.OfferId);
                        //campIdsSaved.Add(target.CampId);

                        //Update currencies...
                        var camp = db.Camps.Find(item.Campaign.CampaignId);
                        if (camp != null)
                        {
                            //CostCurr
                            if (camp.CurrencyAbbr == CurrencyAbbr.USD)
                                target.CostCurrId = CurrencyId.USD;
                            else
                            { // other than USD
                                // find currency match from currency table
                                if (currencyIdLookupByAbbr.ContainsKey(camp.CurrencyAbbr))
                                {
                                    target.CostCurrId = currencyIdLookupByAbbr[camp.CurrencyAbbr];
                                    target.Cost = camp.PayoutAmount * item.Paid;
                                    // recompute cost in foreign currency
                                }
                            }
                            var offerContract = db.OfferContracts.Find(camp.OfferContractId);
                            if (offerContract != null)
                            {
                                var offer = db.Offers.Find(offerContract.OfferId);
                                if (offer != null)
                                {
                                    //RevCurr
                                    if (offer.CurrencyAbbr == CurrencyAbbr.USD)
                                        target.RevCurrId = CurrencyId.USD;
                                    else
                                    { // other than USD
                                        // find currency match from currency table
                                        if (currencyIdLookupByAbbr.ContainsKey(offer.CurrencyAbbr))
                                        {
                                            target.RevCurrId = currencyIdLookupByAbbr[offer.CurrencyAbbr];
                                            target.Revenue = offerContract.ReceivedAmount * item.Paid;
                                            // recompute revenue in foreign currency
                                        }
                                    }
                                }
                            }
                        }
                    }
                    loaded++;
                }
                Logger.Info("Loading {0} CampSums ({1} updates, {2} additions, {3} deletions, {4} already-deleted)", loaded, updated, added, deleted, alreadyDeleted);
                db.SaveChanges();
            }
            return loaded;
        }

        private void LoadCurrencies()
        {
            using (var db = new DAContext())
            {
                currencyIdLookupByAbbr = db.Currencies.ToDictionary(c => c.Abbr, c => c.Id);
            }
        }

        public static void AddMissingOffers(List<CampaignSummary> items)
        {
            int[] existingOfferIds;
            using (var db = new DAContext())
            {
                existingOfferIds = db.Offers.Select(x => x.OfferId).ToArray();
            }
            var neededOfferIds = items.Where(KeepFunc).Select(cs => cs.SiteOffer.SiteOfferId).Distinct();
            var missingOfferIds = neededOfferIds.Where(id => !existingOfferIds.Contains(id));

            //NOTE: this _should_ be okay since the CampSum extracter just makes one call to Cake, so that's done by now

            var extracter = new OffersExtracter(offerIds: missingOfferIds);
            var loader = new DAOffersLoader(loadInactive: true);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

        //TODO: check if all affiliates are in db; save any that aren't
        //TODO: make camp.AffId a foreign key
        public static void AddMissingCampaigns(List<CampaignSummary> items)
        {
            int[] existingCampIds;
            using (var db = new DAContext())
            {
                existingCampIds = db.Camps.Select(x => x.CampaignId).ToArray();
            }
            var neededCampIds = items.Where(KeepFunc).Select(cs => cs.Campaign.CampaignId).Distinct();
            var missingCampIds = neededCampIds.Where(id => !existingCampIds.Contains(id));

            var extracter = new CampaignsExtracter(campaignIds: missingCampIds);
            var loader = new DACampLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
        }

    }
}
