using System;
using System.Linq;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.DBM;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Abstract
{
    public interface ITDRepository : IDisposable
    {
        void SaveChanges();

        // (dbo)
        Employee Employee(int id);
        IQueryable<Employee> Employees();
        bool AddEmployee(Employee emp);
        bool SaveEmployee(Employee emp);

        // TD
        Platform Platform(int id);
        Platform Platform(string platformCode);
        IQueryable<Platform> Platforms();
        bool AddPlatform(Platform platform);
        bool SavePlatform(Platform platform);
        Advertiser Advertiser(int id);
        IQueryable<Advertiser> Advertisers();
        bool AddAdvertiser(Advertiser adv);
        bool SaveAdvertiser(Advertiser adv);
        Campaign Campaign(int id);
        IQueryable<Campaign> Campaigns(int? advId = null);
        bool AddCampaign(Campaign camp);
        bool DeleteCampaign(int id);
        bool SaveCampaign(Campaign camp);
        void FillExtended(Campaign camp);
        bool AddExtAccountToCampaign(int campId, int acctId);
        bool RemoveExtAccountFromCampaign(int campId, int acctId);
        void CreateBudgetIfNotExists(Campaign campaign, DateTime monthToCreate);
        BudgetInfo BudgetInfo(int campId, DateTime date);
        IQueryable<BudgetInfo> BudgetInfos(int? campId = null, DateTime? date = null);
        bool AddBudgetInfo(BudgetInfo bi);
        bool SaveBudgetInfo(BudgetInfo bi);
        void FillExtended(BudgetInfo bi);
        ExtAccount ExtAccount(int id);
        IQueryable<ExtAccount> ExtAccounts(string platformCode = null, int? campId = null);
        IQueryable<ExtAccount> ExtAccountsNotInCampaign(int campId);
        bool AddExtAccount(ExtAccount extAcct);
        bool SaveExtAccount(ExtAccount extAcct);
        void FillExtended(ExtAccount extAcct);
        DateTime? LatestStatDate(int? acctId = null);
        DailySummary DailySummary(DateTime date, int acctId);
        bool AddDailySummary(DailySummary daySum);
        bool SaveDailySummary(DailySummary daySum);
        void FillExtended(DailySummary daySum);
        IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? acctId = null);
        TDStat GetTDStat(DateTime? startDate, DateTime? endDate, Campaign campaign = null, MarginFeeVals marginFees = null);
        TDStat GetTDStatWithAccount(DateTime? startDate, DateTime? endDate, ExtAccount extAccount = null, MarginFeeVals marginFees = null);

        // AdRoll
        Advertisable Advertisable(string eid);
        IQueryable<Advertisable> Advertisables();
        IQueryable<Ad> AdRoll_Ads(int? advId = null, string advEid = null);
        IQueryable<AdDailySummary> AdRoll_AdDailySummaries(int? advertisableId, int? adId, DateTime? startDate, DateTime? endDate);
        TDStat GetAdRollStat(Ad ad, DateTime? startDate, DateTime? endDate);

        // DBM
        InsertionOrder InsertionOrder(int ioID);
        IQueryable<InsertionOrder> InsertionOrders();
        //IQueryable<Creative> DBM_Creatives(int? ioID);
        IQueryable<CreativeDailySummary> DBM_CreativeDailySummaries(DateTime? startDate, DateTime? endDate, int? ioID = null, int? creativeID = null);
        IQueryable<TDStat> GetDBMStatsByCreative(int ioID, DateTime? startDate, DateTime? endDate);
        TDStat GetDBMStat(Creative creative, DateTime? startDate, DateTime? endDate);
    }
}
