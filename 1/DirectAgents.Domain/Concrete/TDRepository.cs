using System;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.DBM;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Concrete
{
    public class TDRepository : ITDRepository, IDisposable
    {
        private DATDContext context;

        public TDRepository(DATDContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        #region TD

        public IQueryable<Account> Accounts(string platformCode = null)
        {
            var accounts = context.Accounts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(platformCode))
                accounts = accounts.Where(a => a.Platform.Code == platformCode);
            return accounts;
        }

        public IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? accountId = null)
        {
            var dSums = context.DailySummaries.AsQueryable();
            if (startDate.HasValue)
                dSums = dSums.Where(ds => ds.Date >= startDate.Value);
            if (endDate.HasValue)
                dSums = dSums.Where(ds => ds.Date <= endDate.Value);
            if (accountId.HasValue)
                dSums = dSums.Where(ds => ds.AccountId == accountId.Value);
            return dSums;
        }

        public TDStatWithAccount GetTDStatWithAccount(DateTime? startDate, DateTime? endDate, Account account = null)
        {
            var stat = new TDStatWithAccount
            {
                Account = account
            };
            int? accountId = (account != null) ? account.Id : (int?)null;
            var dSums = DailySummaries(startDate, endDate, accountId: accountId);
            //NOTE: This will sum stats for ALL accounts if none specified.
            // Later, will we have other params, like campaign?

            if (dSums.Any())
            {
                stat.Impressions = dSums.Sum(ds => ds.Impressions);
                stat.Clicks = dSums.Sum(ds => ds.Clicks);
                stat.Conversions = dSums.Sum(ds => ds.Conversions);
                stat.Cost = Math.Round(dSums.Sum(ds => ds.Cost), 2);
            }
            return stat;
        }

        #endregion
        #region AdRoll

        public IQueryable<Advertisable> Advertisables()
        {
            return context.Advertisables;
        }

        public IQueryable<Ad> AdRoll_Ads(int? advId = null, string advEid = null)
        {
            var ads = context.AdRollAds.AsQueryable();
            if (advId.HasValue)
                ads = ads.Where(a => a.AdvertisableId == advId.Value);
            if (!string.IsNullOrWhiteSpace(advEid))
                ads = ads.Where(a => a.Advertisable.Eid == advEid);
            return ads;
        }

        public IQueryable<AdDailySummary> AdRoll_AdDailySummaries(int? advertisableId, int? adId, DateTime? startDate, DateTime? endDate)
        {
            var ads = context.AdRollAdDailySummaries.AsQueryable();
            if (advertisableId.HasValue) // TODO: check what query this translates to...
                ads = ads.Where(a => a.Ad.AdvertisableId == advertisableId.Value);
            if (adId.HasValue)
                ads = ads.Where(a => a.AdId == adId.Value);
            if (startDate.HasValue)
                ads = ads.Where(a => a.Date >= startDate.Value);
            if (endDate.HasValue)
                ads = ads.Where(a => a.Date <= endDate.Value);
            return ads;
        }

        public AdRollStat GetAdRollStat(Ad ad, DateTime? startDate, DateTime? endDate)
        {
            var stat = new AdRollStat
            {
                Name = ad.Name
            };
            var ads = AdRoll_AdDailySummaries(null, ad.Id, startDate, endDate);
            if (ads.Any())
            {
                stat.Impressions = ads.Sum(a => a.Impressions);
                stat.Clicks = ads.Sum(a => a.Clicks);
                stat.ClickThruConv = ads.Sum(a => a.CTC);
                stat.ViewThruConv = ads.Sum(a => a.VTC);
                stat.Spend = Math.Round(ads.Sum(a => a.Cost), 2);
                stat.Prospects = ads.Sum(a => a.Prospects);
            }
            return stat;
        }

        #endregion
        #region DBM

        public InsertionOrder InsertionOrder(int ioID)
        {
            return context.InsertionOrders.Find(ioID);
        }
        public IQueryable<InsertionOrder> InsertionOrders()
        {
            return context.InsertionOrders;
        }

        public IQueryable<Creative> DBM_Creatives(int? ioID)
        {
            var creatives = context.Creatives.AsQueryable();
            if (ioID.HasValue)
                creatives = creatives.Where(c => c.InsertionOrderID == ioID);
            return creatives;
        }

        public IQueryable<CreativeDailySummary> DBM_CreativeDailySummaries(DateTime? startDate, DateTime? endDate, int? ioID = null, int? creativeID = null)
        {
            var cds = context.DBMCreativeDailySummaries.AsQueryable();
            if (ioID.HasValue)
                cds = cds.Where(c => c.Creative.InsertionOrderID == ioID.Value);
            if (creativeID.HasValue)
                cds = cds.Where(c => c.CreativeID == creativeID.Value);
            if (startDate.HasValue)
                cds = cds.Where(c => c.Date >= startDate.Value);
            if (endDate.HasValue)
                cds = cds.Where(c => c.Date <= endDate.Value);
            return cds;
        }

        public TDStat GetDBMStat(Creative creative, DateTime? startDate, DateTime? endDate)
        {
            var stat = new TDStat
            {
                Name = creative.Name
            };
            var cds = DBM_CreativeDailySummaries(startDate, endDate, creativeID: creative.ID);
            if (cds.Any())
            {
                stat.Impressions = cds.Sum(c => c.Impressions);
                stat.Clicks = cds.Sum(c => c.Clicks);
                //stat.Conversions = cds.Sum(c => c.Conversions);
                stat.Conversions = cds.Sum(c => c.PostClickConv) + cds.Sum(c => c.PostViewConv);
                stat.Cost = cds.Sum(c => c.Revenue);
            }
            return stat;
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
