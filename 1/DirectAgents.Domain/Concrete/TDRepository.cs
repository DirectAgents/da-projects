using System;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;
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

        #region DBO

        public Employee Employee(int id)
        {
            return context.Employees.Find(id);
        }
        public IQueryable<Employee> Employees()
        {
            return context.Employees;
        }

        public bool AddEmployee(Employee emp)
        {
            if (context.Employees.Any(e => e.Id == emp.Id))
                return false;
            context.Employees.Add(emp);
            context.SaveChanges();
            return true;
        }
        public bool SaveEmployee(Employee emp)
        {
            if (context.Employees.Any(e => e.Id == emp.Id))
            {
                var entry = context.Entry(emp);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion
        #region TD

        // --- Platforms/Advertisers ---

        public Platform Platform(int id)
        {
            return context.Platforms.Find(id);
        }
        public Platform Platform(string platformCode)
        {
            return context.Platforms.Where(p => p.Code == platformCode).FirstOrDefault();
        }
        public IQueryable<Platform> Platforms()
        {
            return context.Platforms;
        }

        public bool AddPlatform(Platform platform)
        {
            if (context.Platforms.Any(p => p.Id == platform.Id))
                return false;
            context.Platforms.Add(platform);
            context.SaveChanges();
            return true;
        }
        public bool SavePlatform(Platform platform)
        {
            if (context.Platforms.Any(p => p.Id == platform.Id))
            {
                var entry = context.Entry(platform);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Advertiser Advertiser(int id)
        {
            return context.Advertisers.Find(id);
        }
        public IQueryable<Advertiser> Advertisers()
        {
            return context.Advertisers;
        }

        public bool AddAdvertiser(Advertiser adv)
        {
            if (context.Advertisers.Any(a => a.Id == adv.Id))
                return false;
            context.Advertisers.Add(adv);
            context.SaveChanges();
            return true;
        }
        public bool SaveAdvertiser(Advertiser adv)
        {
            if (context.Advertisers.Any(a => a.Id == adv.Id))
            {
                var entry = context.Entry(adv);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        // --- Campaigns/Budgets ---

        public Campaign Campaign(int id)
        {
            return context.Campaigns.Find(id);
        }
        public IQueryable<Campaign> Campaigns(int? advId = null)
        {
            var campaigns = context.Campaigns.AsQueryable();
            if (advId.HasValue)
                campaigns = campaigns.Where(c => c.AdvertiserId == advId.Value);
            return campaigns;
        }

        public bool AddCampaign(Campaign camp)
        {
            if (context.Campaigns.Any(c => c.Id == camp.Id))
                return false;
            context.Campaigns.Add(camp);
            context.SaveChanges();
            return true;
        }
        public bool DeleteCampaign(int id)
        {
            var campaign = context.Campaigns.Find(id);
            if (campaign == null)
                return false;
            context.Campaigns.Remove(campaign);
            context.SaveChanges();
            return true;
        }
        public bool SaveCampaign(Campaign camp)
        {
            if (context.Campaigns.Any(c => c.Id == camp.Id))
            {
                var entry = context.Entry(camp);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public void FillExtended(Campaign camp)
        {
            if (camp.Advertiser == null)
                camp.Advertiser = context.Advertisers.Find(camp.AdvertiserId);
            if (camp.Budgets == null)
                camp.Budgets = BudgetInfos(campId: camp.Id).ToList();
        }

        public bool AddExtAccountToCampaign(int campId, int acctId)
        {
            var campaign = context.Campaigns.Find(campId);
            var extAcct = context.ExtAccounts.Find(acctId);
            if (campaign == null || extAcct == null)
                return false;
            campaign.ExtAccounts.Add(extAcct);
            context.SaveChanges();
            return true;
        }
        public bool RemoveExtAccountFromCampaign(int campId, int acctId)
        {
            var campaign = context.Campaigns.Find(campId);
            if (campaign == null)
                return false;
            var extAcct = campaign.ExtAccounts.Where(a => a.Id == acctId).FirstOrDefault();
            if (extAcct == null)
                return false;

            campaign.ExtAccounts.Remove(extAcct);
            context.SaveChanges();
            return true;
        }

        // monthToCreate is any day in the desired month
        public void CreateBudgetIfNotExists(Campaign campaign, DateTime monthToCreate)
        {
            // Note: If you create a new Campaign, you should set Budgets to a new Collection before calling this method
            var firstOfMonth = new DateTime(monthToCreate.Year, monthToCreate.Month, 1);
            if (!campaign.Budgets.Where(b => b.Date == firstOfMonth).Any())
            {
                var budget = new BudgetInfo { Date = firstOfMonth };
                budget.SetBudgetValsFrom(campaign.DefaultBudget);
                campaign.Budgets.Add(budget);
                context.SaveChanges();
            }
        }

        public BudgetInfo BudgetInfo(int campId, DateTime date)
        {
            return context.BudgetInfos.Find(campId, date);
        }
        public IQueryable<BudgetInfo> BudgetInfos(int? campId = null, DateTime? date = null)
        {
            var budgetInfos = context.BudgetInfos.AsQueryable();
            if (campId.HasValue)
                budgetInfos = budgetInfos.Where(b => b.CampaignId == campId.Value);
            if (date.HasValue)
                budgetInfos = budgetInfos.Where(b => b.Date == date.Value);
            return budgetInfos;
        }

        public bool AddBudgetInfo(BudgetInfo bi)
        {
            if (context.BudgetInfos.Any(b => b.CampaignId == bi.CampaignId && b.Date == bi.Date))
                return false;

            context.BudgetInfos.Add(bi);
            context.SaveChanges();
            return true;
        }
        public bool SaveBudgetInfo(BudgetInfo bi)
        {
            if (context.BudgetInfos.Any(b => b.CampaignId == bi.CampaignId && b.Date == bi.Date))
            {
                var entry = context.Entry(bi);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public void FillExtended(BudgetInfo bi)
        {
            if (bi.Campaign == null)
                bi.Campaign = context.Campaigns.Find(bi.CampaignId);
        }

        // ---

        public ExtAccount ExtAccount(int id)
        {
            return context.ExtAccounts.Find(id);
        }
        public IQueryable<ExtAccount> ExtAccounts(string platformCode = null, int? campId = null)
        {
            var extAccounts = context.ExtAccounts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(platformCode))
                extAccounts = extAccounts.Where(a => a.Platform.Code == platformCode);
            if (campId.HasValue)
                extAccounts = extAccounts.Where(a => a.CampaignId == campId.Value);
            return extAccounts;
        }

        public IQueryable<ExtAccount> ExtAccountsNotInCampaign(int campId)
        {
            var extAccounts = context.ExtAccounts.AsQueryable();
            var campaign = context.Campaigns.Find(campId);
            if (campaign != null)
            {
                var campaignAcctIds = campaign.ExtAccounts.Select(a => a.Id).ToArray();
                extAccounts = extAccounts.Where(a => !campaignAcctIds.Contains(a.Id));
            }
            return extAccounts;
        }

        public bool AddExtAccount(ExtAccount extAcct)
        {
            if (context.ExtAccounts.Any(ea => ea.Id == extAcct.Id))
                return false;
            context.ExtAccounts.Add(extAcct);
            context.SaveChanges();
            return true;
        }
        public bool SaveExtAccount(ExtAccount extAcct)
        {
            if (context.ExtAccounts.Any(ea => ea.Id == extAcct.Id))
            {
                var entry = context.Entry(extAcct);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        public void FillExtended(ExtAccount extAcct)
        {
            if (extAcct.Platform == null)
                extAcct.Platform = Platform(extAcct.PlatformId);
        }

        public DateTime? LatestStatDate(int? acctId = null)
        {
            var dSums = DailySummaries(null, null, acctId: acctId);
            if (!dSums.Any())
                return null;
            return dSums.Max(ds => ds.Date);
        }

        public IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? acctId = null)
        {
            var dSums = context.DailySummaries.AsQueryable();
            if (startDate.HasValue)
                dSums = dSums.Where(ds => ds.Date >= startDate.Value);
            if (endDate.HasValue)
                dSums = dSums.Where(ds => ds.Date <= endDate.Value);
            if (acctId.HasValue)
                dSums = dSums.Where(ds => ds.AccountId == acctId.Value);
            return dSums;
        }

        //NOTE: This will sum stats for ALL campaigns if none specified.
        public TDStat GetTDStat(DateTime? startDate, DateTime? endDate, Campaign campaign = null, MarginFeeVals marginFees = null)
        {
            var stat = new TDStat()
            {
                Campaign = campaign
            };
            var dSums = DailySummaries(startDate, endDate);
            if (campaign != null)
            {
                var accountIds = campaign.ExtAccounts.Select(a => a.Id).ToArray();
                dSums = dSums.Where(ds => accountIds.Contains(ds.AccountId));
            }
            if (dSums.Any())
                stat.SetStatsFrom(dSums);
            if (marginFees != null)
                stat.SetMarginFees(marginFees);

            return stat;
        }

        //NOTE: This will sum stats for ALL accounts if none specified.
        public TDStat GetTDStatWithAccount(DateTime? startDate, DateTime? endDate, ExtAccount extAccount = null, MarginFeeVals marginFees = null)
        {
            var stat = new TDStat
            {
                ExtAccount = extAccount
            };
            int? accountId = (extAccount != null) ? extAccount.Id : (int?)null;
            var dSums = DailySummaries(startDate, endDate, acctId: accountId);

            if (dSums.Any())
                stat.SetStatsFrom(dSums);
            if (marginFees != null)
                stat.SetMarginFees(marginFees);

            return stat;
        }

        #endregion
        #region AdRoll

        public Advertisable Advertisable(string eid)
        {
            return context.Advertisables.Where(a => a.Eid == eid).SingleOrDefault();
        }
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

        public TDStat GetAdRollStat(Ad ad, DateTime? startDate, DateTime? endDate)
        {
            var stat = new TDStat
            {
                Name = ad.Name
            };
            var ads = AdRoll_AdDailySummaries(null, ad.Id, startDate, endDate);
            if (ads.Any())
            {
                stat.Impressions = ads.Sum(a => a.Impressions);
                stat.Clicks = ads.Sum(a => a.Clicks);
                stat.PostClickConv = ads.Sum(a => a.CTC);
                stat.PostViewConv = ads.Sum(a => a.VTC);
                stat.Cost = Math.Round(ads.Sum(a => a.Cost), 2);
                //stat.Prospects = ads.Sum(a => a.Prospects);
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

        //public IQueryable<Creative> DBM_Creatives(int? ioID)
        //{
        //    var creatives = context.Creatives.AsQueryable();
        //    if (ioID.HasValue)
        //        creatives = creatives.Where(c => c.InsertionOrderID == ioID);
        //    return creatives;
        //}

        public IQueryable<CreativeDailySummary> DBM_CreativeDailySummaries(DateTime? startDate, DateTime? endDate, int? ioID = null, int? creativeID = null)
        {
            var cds = context.DBMCreativeDailySummaries.AsQueryable();
            if (ioID.HasValue)
                cds = cds.Where(c => c.InsertionOrderID == ioID.Value);
            if (creativeID.HasValue)
                cds = cds.Where(c => c.CreativeID == creativeID.Value);
            if (startDate.HasValue)
                cds = cds.Where(c => c.Date >= startDate.Value);
            if (endDate.HasValue)
                cds = cds.Where(c => c.Date <= endDate.Value);
            return cds;
        }

        public IQueryable<TDStat> GetDBMStatsByCreative(int ioID, DateTime? startDate, DateTime? endDate)
        {
            var cds = DBM_CreativeDailySummaries(startDate, endDate, ioID: ioID);
            var groups = cds.GroupBy(c => c.Creative);
            var stats = groups.Select(g => new TDStat
            {
                Name = g.Key.Name,
                Impressions = g.Sum(c => c.Impressions),
                Clicks = g.Sum(c => c.Clicks),
                PostClickConv = g.Sum(c => c.PostClickConv),
                PostViewConv = g.Sum(c => c.PostViewConv),
                Cost = g.Sum(c => c.Revenue)
            });
            return stats;
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
                stat.PostClickConv = cds.Sum(c => c.PostClickConv);
                stat.PostViewConv = cds.Sum(c => c.PostViewConv);
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
