﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Cake;

namespace CakeExtracter.Etl.CakeMarketing.DALoaders
{
    public class DACampSumLoader : Loader<CampaignSummary>
    {
        private readonly Func<CampaignSummary, bool> keepFunc;
        private readonly List<int> idsOfNotLoadedCampaigns = new List<int>();

        private Dictionary<string, int> currencyIdLookupByAbbr = new Dictionary<string, int>();

        public DACampSumLoader(bool keepAllNonZero = false)
        {
            if (keepAllNonZero)
            {
                keepFunc = cs =>
                    (cs.SourceAffiliate.SourceAffiliateId > 0 && cs.Campaign.CampaignId > 0) && !cs.AllZeros();
            }
            else
            {
                keepFunc = cs =>
                    (cs.SourceAffiliate.SourceAffiliateId > 0 && cs.Campaign.CampaignId > 0) &&
                    (cs.Paid > 0 || cs.Revenue > 0 || cs.Cost > 0);
            }

            LoadCurrencies();
        }

        public static void DoOffersEtl(IEnumerable<int> offerIds)
        {
            if (!offerIds.Any())
            {
                return;
            }

            var extractor = new OffersExtracter(offerIds: offerIds);
            var loader = new DAOffersLoader(loadInactive: true);
            CommandHelper.DoEtl(extractor, loader);
        }

        public static void DoAffiliatesEtl(IEnumerable<int> affIds)
        {
            if (!affIds.Any())
            {
                return;
            }

            var extractor = new AffiliatesExtracter(affiliateIds: affIds);
            var loader = new DAAffiliatesLoader();
            CommandHelper.DoEtl(extractor, loader);
        }

        public static IEnumerable<int> DoCampaignsEtl(IEnumerable<int> campIds)
        {
            if (!campIds.Any())
            {
                return new int[] { };
            }

            var extractor = new CampaignsExtracter(campaignIds: campIds);
            var loader = new DACampLoader();
            CommandHelper.DoEtl(extractor, loader);
            return extractor.CampIdsUnableToRetrieve;
        }

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

        // TODO: hashset for aff ids

        protected override int Load(List<CampaignSummary> items)
        {
            //TODO: make it so we don't have to query the db everytime through Load()
            //      ...to get existingOfferIds, existingAffIds and existingCampIds

            Logger.Info("Loading {0} CampSums..", items.Count);
            AddMissingOffers(items);
            AddMissingAffiliates(items);
            var idsOfMissingCampaigns = AddMissingCampaigns(items);
            idsOfNotLoadedCampaigns.AddRange(idsOfMissingCampaigns);
            var count = UpsertCampSums(items);
            return count;
        }

        private int UpsertCampSums(List<CampaignSummary> items)
        {
            var loaded = 0;
            var added = 0;
            var updated = 0;
            var deleted = 0;
            var alreadyDeleted = 0;
            using (var db = new DAContext())
            {
                foreach (var item in items)
                {
                    var toDelete = !keepFunc(item);
                    if (!toDelete && idsOfNotLoadedCampaigns.Contains(item.Campaign.CampaignId))
                    {
                        toDelete = true;
                        Logger.Info("Skipping CampSum for {0:d} because campaign {1} could not be loaded", item.Date, item.Campaign.CampaignId);
                    }

                    var pk1 = item.Campaign.CampaignId;
                    var pk2 = item.Date;
                    var target = db.CampSums.Find(pk1, pk2);

                    if (toDelete)
                    {
                        if (target == null)
                        {
                            alreadyDeleted++;
                        }
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
                                Date = item.Date,
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

        private void AddMissingOffers(IEnumerable<CampaignSummary> items)
        {
            int[] existingOfferIds;
            using (var db = new DAContext())
            {
                existingOfferIds = db.Offers.Select(x => x.OfferId).ToArray();
            }
            var neededOfferIds = items.Where(keepFunc).Select(cs => cs.SiteOffer.SiteOfferId).Distinct();
            var missingOfferIds = neededOfferIds.Where(id => !existingOfferIds.Contains(id));

            //NOTE: this _should_ be okay since the CampSum extracter just makes one call to Cake, so that's done by now
            DoOffersEtl(missingOfferIds);
        }

        private void AddMissingAffiliates(IEnumerable<CampaignSummary> items)
        {
            int[] existingAffIds;
            using (var db = new DAContext())
            {
                existingAffIds = db.Affiliates.Select(x => x.AffiliateId).ToArray();
            }
            var neededAffIds = items.Where(keepFunc).Select(cs => cs.SourceAffiliate.SourceAffiliateId).Distinct();
            var missingAffIds = neededAffIds.Where(id => !existingAffIds.Contains(id));

            // ?Could just use the SourceAffiliateId and SourceAffiliateName?
            // ?future: may be interested in other attributes?
            DoAffiliatesEtl(missingAffIds);
        }

        //TODO: make camp.AffId a foreign key

        // returns the campIds of any missing campaigns that couldn't be retrieved
        private IEnumerable<int> AddMissingCampaigns(IEnumerable<CampaignSummary> items)
        {
            int[] existingCampIds;
            using (var db = new DAContext())
            {
                existingCampIds = db.Camps.Select(x => x.CampaignId).ToArray();
            }
            var neededCampIds = items.Where(keepFunc).Select(cs => cs.Campaign.CampaignId).Distinct();
            var missingCampIds = neededCampIds.Where(id => !existingCampIds.Contains(id) && !idsOfNotLoadedCampaigns.Contains(id));

            //if (missingCampIds.Any())
            //{
            //    var itemsInQuestion = items.Where(KeepFunc).Where(cs => missingCampIds.Contains(cs.Campaign.CampaignId)).ToArray();
            //}

            //??? What to do if we fail to extract one of the needed campaigns? Will get a foreign-key violation error during the upsert
            //TODO: remember the campId and handle it when the related item gets processed.

            return DoCampaignsEtl(missingCampIds);
        }
    }
}
