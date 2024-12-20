﻿using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.Cake;
using DirectAgents.Domain.Entities.Screen;
using Z.EntityFramework.Plus;

namespace DirectAgents.Domain.Concrete
{
    public class MainRepository : IMainRepository, IDisposable
    {
        private DAContext context;

        public MainRepository(DAContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public IQueryable<Variable> GetVariables()
        {
            return context.Variables;
        }

        public Variable GetVariable(string name)
        {
            return context.Variables.Find(name);
        }

        public void SaveVariable(Variable variable)
        {
            var existing = GetVariable(variable.Name);
            if (existing != null)
            {
                existing.StringVal = variable.StringVal;
                existing.IntVal = variable.IntVal;
                existing.DecVal = variable.DecVal;
            }
            else
            {
                context.Variables.Add(variable);
            }
            SaveChanges();
        }

        #region Screens

        public IQueryable<Salesperson> Salespeople()
        {
            return context.Salespeople;
        }

        public IQueryable<SalespersonStat> SalespersonStats(DateTime? minDate = null)
        {
            var stats = context.SalespersonStats.AsQueryable();
            if (minDate.HasValue)
                stats = stats.Where(s => s.Date >= minDate.Value);
            return stats;
        }
        public IQueryable<SalespersonStat> SalespersonStats(int? salespersonId, DateTime? date)
        {
            var stats = context.SalespersonStats.AsQueryable();
            if (salespersonId.HasValue)
                stats = stats.Where(s => s.SalespersonId == salespersonId.Value);
            if (date.HasValue)
                stats = stats.Where(s => s.Date == date.Value);
            return stats;
        }

        public SalespersonStat GetSalespersonStat(int salespersonId, DateTime date)
        {
            return context.SalespersonStats.Find(date, salespersonId);
        }
        public void SaveSalespersonStat(SalespersonStat stat)
        {
            var existing = GetSalespersonStat(stat.SalespersonId, stat.Date);
            if (existing != null)
            {
                existing.EmailSent = stat.EmailSent;
                existing.EmailTracked = stat.EmailTracked;
                existing.EmailOpened = stat.EmailOpened;
                existing.EmailReplied = stat.EmailReplied;
            }
            else
            {
                context.SalespersonStats.Add(stat);
            }
        }

        public void DeleteSalespersonStats(DateTime date)
        {
            var stats = context.SalespersonStats.Where(s => s.Date == date);
            context.SalespersonStats.RemoveRange(stats);
        }

        #endregion
        #region Cake

        public IDepartmentRepository Create_Cake_DeptRepository()
        {
            return new Cake_DeptRepository(this);
        }

        public Contact GetContact(int contactId)
        {
            return context.Contacts.Find(contactId);
        }

        public IQueryable<Contact> GetAccountManagers()
        {
            var accountManagers = context.Advertisers.Where(a => a.AccountManagerId.HasValue).Select(a => a.AccountManager).Distinct();
            return accountManagers;
        }

        public IQueryable<Advertiser> GetAdvertisers(int? acctMgrId = null, bool? withBudgetedOffers = null, int? ABClientId = null)
        {
            var advertisers = context.Advertisers.AsQueryable();
            if (acctMgrId.HasValue)
                advertisers = advertisers.Where(a => a.AccountManagerId == acctMgrId.Value);
            if (ABClientId.HasValue)
                advertisers = advertisers.Where(a => a.ABClientId == ABClientId.Value);

            if (withBudgetedOffers.HasValue && withBudgetedOffers.Value)
                advertisers = advertisers.Where(a => a.Offers.Any(o => o.OfferBudgets.Any()));
            if (withBudgetedOffers.HasValue && !withBudgetedOffers.Value)
                advertisers = advertisers.Where(a => !a.Offers.Any() || a.Offers.Any(o => !o.Budget.HasValue));

            return advertisers;
        }

        public Advertiser GetAdvertiser(int advertiserId)
        {
            return context.Advertisers.Find(advertiserId);
        }

        public IQueryable<Offer> GetOffers(bool includeExtended = false, int? acctMgrId = null, int? advertiserId = null, bool? withBudget = null, bool includeInactive = true, bool? hidden = null)
        {
            IQueryable<Offer> offers;
            if (includeExtended)
                offers = context.Offers.Include("Advertiser.AccountManager").Include("Advertiser.AdManager").AsQueryable();
            else
                offers = context.Offers.AsQueryable();

            if (acctMgrId.HasValue)
                offers = offers.Where(o => o.Advertiser.AccountManagerId == acctMgrId.Value);
            if (advertiserId.HasValue)
                offers = offers.Where(o => o.AdvertiserId == advertiserId);

            // TODO: count OfferBudgets where Budget==0 as "doesn't have budget" ??
            if (withBudget.HasValue && withBudget.Value)
                offers = offers.Where(o => o.OfferBudgets.Count > 0);
            if (withBudget.HasValue && !withBudget.Value)
                offers = offers.Where(o => o.OfferBudgets.Count == 0);

            if (!includeInactive)
                offers = offers.Where(o => o.OfferStatusId != OfferStatus.Inactive);

            if (hidden.HasValue && hidden.Value)
                offers = offers.Where(o => o.Hidden);
            if (hidden.HasValue && !hidden.Value)
                offers = offers.Where(o => !o.Hidden);

            return offers;
        }

        // the withBudget and offerIds criteria are UNIONed
        public IQueryable<Offer> GetOffersUnion(bool includeExtendedInfo, bool excludeInactive, bool? withBudget, int[] offerIds)
        {
            var offers = GetOffers(includeExtendedInfo, null, null, null, !excludeInactive, null);
            if (withBudget.HasValue)
            {
                if (withBudget.Value)
                    offers = offers.Where(o => offerIds.Contains(o.OfferId) || o.OfferBudgets.Count > 0);
                else
                    offers = offers.Where(o => offerIds.Contains(o.OfferId) || o.OfferBudgets.Count == 0);
            }
            else
            {
                offers = offers.Where(o => offerIds.Contains(o.OfferId));
            }
            return offers;
        }

        public Offer GetOffer(int offerId, bool includeExtended, bool fillBudgetStats)
        {
            Offer offer;
            if (includeExtended)
                offer = context.Offers.Include("Advertiser.AccountManager").Include("Advertiser.AdManager").SingleOrDefault(o => o.OfferId == offerId);
            else
                offer = context.Offers.Find(offerId);

            if (offer != null)
                FillOfferBudgetStats(offer);
            return offer;
        }

        public IEnumerable<int> OfferIds(int? advId = null)
        {
            var offers = GetOffers(advertiserId: advId);
            var offerIds = offers.Select(o => o.OfferId).ToList();
            return offerIds;
        }

        public void FillOfferBudgetStats(Offer offer)
        {
            if (offer.Budget == null || offer.Budget <= 0)
                return;

            var ods = GetOfferDailySummariesForBudget(offer);
            if (ods.Any())
            {
                offer.BudgetUsed = ods.Sum(o => o.Revenue);
                offer.EarliestStatDate = ods.Min(o => o.Date);
                offer.LatestStatDate = ods.Max(o => o.Date);
            }
            else
                offer.BudgetUsed = 0;
        }

        public IEnumerable<CampaignSummary> TopOffers(int num, TopCampaignsBy by) //, string trafficType)  //maybe: offerIds
        {
            int minClicks = 50;
            var minDate = DateTime.Now.AddDays(-16);
            var offerDailySummaries = GetOfferDailySummaries(null, minDate, null);
            var offers = context.Offers.Where(o => !o.OfferName.Contains("pause") && !o.OfferName.Contains("not live yet")
                                                && !o.OfferName.Contains("cpm"));

            //if (trafficType != null)
            //    offers = offers.Where(

            //if (trafficType != null)
            //    campaigns = campaigns.Where(c => c.TrafficTypes.Select(t => t.Name).Contains(trafficType));

            var query = from ods in offerDailySummaries
                        join o in offers on ods.OfferId equals o.OfferId
                        group ods by new { o.OfferId, o.OfferName } into g
                        select new CampaignSummary
                        {
                            Pid = g.Key.OfferId,
                            CampaignName = g.Key.OfferName,
                            Revenue = g.Sum(ds => ds.Revenue),
                            Cost = g.Sum(ds => ds.Cost),
                            Clicks = g.Sum(ds => ds.Clicks)
                        };

            switch (by)
            {
                case TopCampaignsBy.Revenue:
                    return query.OrderByDescending(c => c.Revenue).Take(num).ToList();
                case TopCampaignsBy.Cost:
                    return query.OrderByDescending(c => c.Cost).Take(num).ToList();
                case TopCampaignsBy.EPC:
                    return query.Where(ds => ds.Clicks >= minClicks).ToList().OrderByDescending(c => c.EPC).Take(num);
                default:
                    throw new Exception("Invalid TopCampaignsBy: " + by.ToString());
            }
        }

        //Note: Starting 7/1/15, includes clicks, conversions and sellable from offerid -1 (Global Redirect) and clicks from -2 (404)
        public IQueryable<OfferDailySummary> GetOfferDailySummaries(int? offerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            var ods = context.OfferDailySummaries.AsQueryable();
            if (offerId.HasValue)
                ods = ods.Where(x => x.OfferId == offerId.Value);
            if (startDate.HasValue)
                ods = ods.Where(x => x.Date >= startDate.Value);
            if (endDate.HasValue)
                ods = ods.Where(x => x.Date <= endDate.Value);
            return ods;
        }

        // get OfferDailySummaries used to compute budget spent for the specified offer
        public IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(int offerId)
        {
            var offer = GetOffer(offerId, false, false);
            return GetOfferDailySummariesForBudget(offer);
        }
        public IQueryable<OfferDailySummary> GetOfferDailySummariesForBudget(Offer offer)
        {
            if (offer == null)
                return new List<OfferDailySummary>().AsQueryable();

            DateTime? start = null, end = null;
            if (offer.HasBudget)
            {
                start = offer.BudgetStart;
                end = offer.BudgetEnd;
            }
            return GetOfferDailySummaries(offer.OfferId, start, end);
        }

        public StatsSummary GetStatsSummary(int? offerId, DateTime? startDate, DateTime? endDate)
        {
            var ods = GetOfferDailySummaries(offerId, startDate, endDate);
            StatsSummary stats;
            if (ods.Any())
            {
                stats = new StatsSummary
                {
                    Views = ods.Sum(o => o.Views),
                    Clicks = ods.Sum(o => o.Clicks),
                    Conversions = ods.Sum(o => o.Conversions),
                    Paid = ods.Sum(o => o.Paid),
                    Sellable = ods.Sum(o => o.Sellable),
                    Revenue = ods.Sum(o => o.Revenue),
                    Cost = ods.Sum(o => o.Cost)
                };
            }
            else
            {
                stats = new StatsSummary();
            }
            return stats;
        }

        // --- Camps, CampSums, etc ---

        public IQueryable<Camp> GetCamps(int? offerId = null, int? affiliateId = null)
        {
            var camps = context.Camps.AsQueryable();
            if (offerId.HasValue)
                camps = camps.Where(x => x.OfferId == offerId.Value);
            if (affiliateId.HasValue)
                camps = camps.Where(x => x.AffiliateId == affiliateId.Value);
            return camps;
        }

        public IQueryable<CampSum> GetCampSums(int? advertiserId = null, int? offerId = null, int? campId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var cs = context.CampSums.AsQueryable();

            if (advertiserId.HasValue) // ?should check cs.Camp.Offer.AdvertiserId?
            {
                var offerIds = OfferIds(advertiserId.Value);
                cs = cs.Where(x => offerIds.Contains(x.OfferId));
            }
            if (offerId.HasValue) // ?should check cs.Camp.OfferId?
                cs = cs.Where(x => x.OfferId == offerId.Value);

            if (campId.HasValue)
                cs = cs.Where(x => x.CampId == campId.Value);
            if (startDate.HasValue)
                cs = cs.Where(x => x.Date >= startDate.Value);
            if (endDate.HasValue)
                cs = cs.Where(x => x.Date <= endDate.Value);
            return cs;
        }
        public void DeleteCampSums(IQueryable<CampSum> campSums)
        {
            campSums.Delete();
            context.SaveChanges();
        }

        public IQueryable<AffSubSummary> GetAffSubSummaries(int? advertiserId = null, int? offerId = null, int? affiliateId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var afss = context.AffSubSummaries.AsQueryable();
            if (advertiserId.HasValue)
                afss = afss.Where(x => x.Offer.AdvertiserId == advertiserId.Value);
            if (offerId.HasValue)
                afss = afss.Where(x => x.OfferId == offerId.Value);
            if (affiliateId.HasValue)
                afss = afss.Where(x => x.AffSub.AffiliateId == affiliateId.Value);
            if (startDate.HasValue)
                afss = afss.Where(x => x.Date >= startDate.Value);
            if (endDate.HasValue)
                afss = afss.Where(x => x.Date <= endDate.Value);
            return afss;
        }
        public void DeleteAffSubSummaries(IQueryable<AffSubSummary> affSubSums)
        {
            affSubSums.Delete();
            context.SaveChanges();
        }

        //TODO: Have these call the corresponding static methods in MainHelper...?
        public IQueryable<EventConversion> GetEventConversions(int? advertiserId = null, int? offerId = null, int? affiliateId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var ec = context.EventConversions.AsQueryable();
            if (advertiserId.HasValue)
                ec = ec.Where(x => x.Offer.AdvertiserId == advertiserId.Value);
            if (offerId.HasValue)
                ec = ec.Where(x => x.OfferId == offerId.Value);
            if (affiliateId.HasValue)
                ec = ec.Where(x => x.AffiliateId == affiliateId.Value);
            if (startDate.HasValue)
                ec = ec.Where(x => x.ConvDate >= startDate.Value);
            if (endDate.HasValue)
            {
                var endDatePlusOne = endDate.Value.AddDays(1);
                ec = ec.Where(x => x.ConvDate < endDatePlusOne);
            }
            return ec;
        }
        public void DeleteEventConversions(IQueryable<EventConversion> eventConvs)
        {
            eventConvs.Delete();
            context.SaveChanges();
        }

        // --- Stats Gauges ---

        public CakeGauge GetGaugeForAllAdvertisers(DateTime? startDateForStats = null)
        {
            var allOffers = GetOffers();
            var allCampSums = GetCampSums();
            var campSumsForStats = startDateForStats.HasValue ? allCampSums.Where(cs => cs.Date >= startDateForStats.Value) : allCampSums;
            var allAffSubSums = GetAffSubSummaries();
            var affSubSumsForStats = startDateForStats.HasValue ? allAffSubSums.Where(afss => afss.Date >= startDateForStats.Value) : allAffSubSums;
            var allConvs = GetEventConversions();
            var convsForStats = startDateForStats.HasValue ? allConvs.Where(ec => ec.ConvDate >= startDateForStats.Value) : allConvs;
            var gauge = new CakeGauge
            {
                Advertiser = new Advertiser { AdvertiserId = 0, AdvertiserName = "All Advertisers" },
                NumOffers = allOffers.Count(),
                CampSums = new CakeGauge.Range {
                    Earliest = allCampSums.Min(cs => (DateTime?)cs.Date),
                    Latest = allCampSums.Max(cs => (DateTime?)cs.Date),
                    NumConvs = campSumsForStats.Sum(cs => (decimal?)cs.Conversions) ?? 0,
                    NumConvsPaid = campSumsForStats.Sum(cs => (decimal?)cs.Paid) ?? 0
                },
                AffSubSums = new CakeGauge.Range {
                    Earliest = allAffSubSums.Min(afss => (DateTime?)afss.Date),
                    Latest = allAffSubSums.Max(afss => (DateTime?)afss.Date),
                    NumConvs = affSubSumsForStats.Sum(afss => (decimal?)afss.Conversions) ?? 0,
                    NumConvsPaid = affSubSumsForStats.Sum(afss => (decimal?)afss.Paid) ?? 0
                },
                EventConvs = new CakeGauge.Range {
                    Earliest = allConvs.Min(ec => (DateTime?)ec.ConvDate),
                    Latest = allConvs.Max(ec => (DateTime?)ec.ConvDate),
                    NumConvs = convsForStats.Count(),
                    NumConvsPaid = 0
                }
            };
            return gauge;
        }

        public IQueryable<CakeGauge> GetGaugesByAdvertiser(bool includeThoseWithNoStats = false, DateTime? startDateForStats = null)
        {
            var advs = GetAdvertisers();
            var ews = advs.Select(adv => new EntityWithStats
            {
                Advertiser = adv,
                CampSums = adv.Offers.AsQueryable().SelectMany(o => o.Camps).SelectMany(c => c.CampSums),
                AffSubSums = adv.Offers.AsQueryable().SelectMany(o => o.AffSubSummaries),
                EventConvs = adv.Offers.AsQueryable().SelectMany(o => o.EventConversions)
            });
            if (!includeThoseWithNoStats)
                ews = ews.Where(x => x.CampSums.Any() || x.AffSubSums.Any() || x.EventConvs.Any());

            IQueryable<CakeGauge> gauges;
            if (startDateForStats.HasValue) // often startDate is the first of the month...
            {
                gauges = ews.Select(x => new CakeGauge()
                {
                    Advertiser = x.Advertiser,
                    NumOffers = x.Advertiser.Offers.Count(),
                    CampSums = new CakeGauge.Range() {
                        Earliest = x.CampSums.Min(cs => (DateTime?)cs.Date),
                        Latest = x.CampSums.Max(cs => (DateTime?)cs.Date),
                        NumConvs = x.CampSums.Where(cs => cs.Date >= startDateForStats.Value).Sum(cs => (decimal?)cs.Conversions) ?? 0,
                        NumConvsPaid = x.CampSums.Where(cs => cs.Date >= startDateForStats.Value).Sum(cs => (decimal?)cs.Paid) ?? 0
                    },
                    AffSubSums = new CakeGauge.Range() {
                        Earliest = x.AffSubSums.Min(afss => (DateTime?)afss.Date),
                        Latest = x.AffSubSums.Max(afss => (DateTime?)afss.Date),
                        NumConvs = x.AffSubSums.Where(afss => afss.Date >= startDateForStats.Value).Sum(afss => (decimal?)afss.Conversions) ?? 0,
                        NumConvsPaid = x.AffSubSums.Where(afss => afss.Date >= startDateForStats.Value).Sum(afss => (decimal?)afss.Paid) ?? 0
                    },
                    EventConvs = new CakeGauge.Range() {
                        Earliest = x.EventConvs.Min(ec => (DateTime?)ec.ConvDate),
                        Latest = x.EventConvs.Max(ec => (DateTime?)ec.ConvDate),
                        NumConvs = x.EventConvs.Where(ec => ec.ConvDate >= startDateForStats.Value).Count(),
                        NumConvsPaid = 0
                    },
                });
            }
            else // compute NumConvs, etc, for everything in the db...
            {
                gauges = ews.Select(x => new CakeGauge()
                {
                    Advertiser = x.Advertiser,
                    CampSums = new CakeGauge.Range() {
                        Earliest = x.CampSums.Min(cs => (DateTime?)cs.Date),
                        Latest = x.CampSums.Max(cs => (DateTime?)cs.Date),
                        NumConvs = x.CampSums.Sum(cs => (decimal?)cs.Conversions) ?? 0,
                        NumConvsPaid = x.CampSums.Sum(cs => (decimal?)cs.Paid) ?? 0
                    },
                    AffSubSums = new CakeGauge.Range()
                    {
                        Earliest = x.AffSubSums.Min(afss => (DateTime?)afss.Date),
                        Latest = x.AffSubSums.Max(afss => (DateTime?)afss.Date),
                        NumConvs = x.AffSubSums.Sum(afss => (decimal?)afss.Conversions) ?? 0,
                        NumConvsPaid = x.AffSubSums.Sum(afss => (decimal?)afss.Paid) ?? 0
                    },
                    EventConvs = new CakeGauge.Range()
                    {
                        Earliest = x.EventConvs.Min(ec => (DateTime?)ec.ConvDate),
                        Latest = x.EventConvs.Max(ec => (DateTime?)ec.ConvDate),
                        NumConvs = x.EventConvs.Count(),
                        NumConvsPaid = 0
                    }
                });
            }
            return gauges;
        }

        //This one does an inner join, so only includes adv with campsums and eventconvs...
        public IQueryable<CakeGauge> GetGaugesByAdvertiserX()
        {
            var advertisers = context.Advertisers.AsQueryable();
            var advCampSumGroups = context.CampSums.Where(x => x.Camp.Offer.AdvertiserId.HasValue).GroupBy(x => x.Camp.Offer.AdvertiserId.Value);
            var advEventConvGroups = context.EventConversions.Where(x => x.Offer.AdvertiserId.HasValue).GroupBy(x => x.Offer.AdvertiserId.Value);
            var gauges = from adv in advertisers
                         join csGrp in advCampSumGroups on adv.AdvertiserId equals csGrp.Key
                         join ecGrp in advEventConvGroups on adv.AdvertiserId equals ecGrp.Key
                         select new CakeGauge()
                         {
                             Advertiser = adv,
                             CampSums = new CakeGauge.Range() { Earliest = csGrp.Min(x => x.Date), Latest = csGrp.Max(x => x.Date) },
                             EventConvs = new CakeGauge.Range() { Earliest = ecGrp.Min(x => x.ConvDate), Latest = ecGrp.Max(x => x.ConvDate) }
                         };
            return gauges;
        }

        #endregion
        // ---

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
