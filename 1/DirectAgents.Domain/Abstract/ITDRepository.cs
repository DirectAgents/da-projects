using System;
using System.Collections.Generic;
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
        IQueryable<Platform> PlatformsWithoutBudgetInfo(int campId, DateTime date); // (without a "PlatformBudgetInfo")
        bool AddPlatform(Platform platform);
        bool SavePlatform(Platform platform);
        Advertiser Advertiser(int id);
        IQueryable<Advertiser> Advertisers();
        bool AddAdvertiser(Advertiser adv);
        bool SaveAdvertiser(Advertiser adv);
        Campaign Campaign(int id);
        IQueryable<Campaign> Campaigns(int? advId = null);
        IQueryable<Campaign> CampaignsActive(DateTime? monthStart = null);
        bool AddCampaign(Campaign camp);
        bool DeleteCampaign(int id);
        bool SaveCampaign(Campaign camp);
        void FillExtended(Campaign camp);
        bool AddExtAccountToCampaign(int campId, int acctId);
        bool RemoveExtAccountFromCampaign(int campId, int acctId);
        //void CreateBudgetIfNotExists(Campaign campaign, DateTime monthToCreate);
        BudgetInfo BudgetInfo(int campId, DateTime date);
        IQueryable<BudgetInfo> BudgetInfos(int? campId = null, DateTime? date = null);
        bool AddBudgetInfo(BudgetInfo bi);
        bool SaveBudgetInfo(BudgetInfo bi);
        void FillExtended(BudgetInfo bi);
        PlatformBudgetInfo PlatformBudgetInfo(int campId, int platformId, DateTime date);
        IQueryable<PlatformBudgetInfo> PlatformBudgetInfos(int? campId = null, int? platformId = null, DateTime? date = null);
        bool AddPlatformBudgetInfo(PlatformBudgetInfo pbi);
        bool DeletePlatformBudgetInfo(int campId, int platformId, DateTime date);
        bool SavePlatformBudgetInfo(PlatformBudgetInfo pbi);
        void FillExtended(PlatformBudgetInfo pbi);
        PlatColMapping PlatColMapping(int id);
        bool AddSavePlatColMapping(PlatColMapping platColMapping);
        void FillExtended(PlatColMapping platColMapping);
        ExtAccount ExtAccount(int id);
        IQueryable<ExtAccount> ExtAccounts(string platformCode = null, int? campId = null);
        IQueryable<ExtAccount> ExtAccountsNotInCampaign(int campId);
        bool AddExtAccount(ExtAccount extAcct);
        bool SaveExtAccount(ExtAccount extAcct);
        void FillExtended(ExtAccount extAcct);
        IQueryable<Strategy> Strategies(int? acctId);
        IQueryable<TDad> TDads(int? acctId);

        //TD Stats
        IEnumerable<BasicStat> DayOfWeekBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null, bool mondayFirst = false);
        IEnumerable<BasicStat> WeeklyBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null);
        IEnumerable<BasicStat> DailySummaryBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null);
        IEnumerable<BasicStat> MTDStrategyBasicStats(int advId, DateTime endDate);
        IEnumerable<BasicStat> StrategyBasicStats(int advId, DateTime startDate, DateTime endDate);
        IEnumerable<BasicStat> CreativePerfBasicStats(int advId, DateTime? startDate = null, DateTime? endDate = null);
        IEnumerable<BasicStat> MTDSiteBasicStats(int advId, DateTime endDate);
        BasicStat MTDBasicStat(int advId, DateTime endDate);
        BasicStat DateRangeBasicStat(int advId, DateTime startDate, DateTime endDate);

        DateTime? EarliestStatDate(int? advId, bool checkAll = false);
        DateTime? EarliestStatDate_Strategy(int? advId);
        DateTime? EarliestStatDate_TDad(int? advId);
        TDStatsGauge GetStatsGauge(ExtAccount extAccount);
        TDStatsGauge GetStatsGauge(int? acctId = null);
        DailySummary DailySummary(DateTime date, int acctId);
        bool AddDailySummary(DailySummary daySum);
        bool SaveDailySummary(DailySummary daySum);
        void FillExtended(DailySummary daySum);
        IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? acctId = null, int? campId = null, int? advId = null);
        IQueryable<StrategySummary> StrategySummaries(DateTime? startDate, DateTime? endDate, int? stratId = null, int? acctId = null, int? campId = null, int? advId = null);
        IQueryable<TDadSummary> TDadSummaries(DateTime? startDate, DateTime? endDate, int? tdadId = null, int? acctId = null, int? campId = null, int? advId = null);
        IQueryable<SiteSummary> SiteSummaries(DateTime? startDate, DateTime? endDate, int? acctId = null, int? campId = null);
        IQueryable<Conv> Convs(DateTime? startDate, DateTime? endDate, int? acctId = null, int? campId = null);
        //TDStat GetTDStat(DateTime? startDate, DateTime? endDate, Campaign campaign = null, MarginFeeVals marginFees = null);
        TDRawStat GetTDStatWithAccount(DateTime? startDate, DateTime? endDate, ExtAccount extAccount = null);
        IEnumerable<TDRawStat> GetStrategyStats(DateTime? startDate, DateTime? endDate, int? acctId = null);
        IEnumerable<TDRawStat> GetTDadStats(DateTime? startDate, DateTime? endDate, int? acctId = null);
        IEnumerable<TDRawStat> GetSiteStats(DateTime? startDate, DateTime? endDate, int? acctId = null, int? minImpressions = null);
        TDCampStats GetCampStats(DateTime monthStart, int campId);
        IEnumerable<TDLineItem> GetDailyStatsLI(int campId, DateTime? startDate, DateTime? endDate);

        ExtraItem ExtraItem(int id);
        IQueryable<ExtraItem> ExtraItems(DateTime? startDate, DateTime? endDate, int? campId = null);
        bool AddExtraItem(ExtraItem item);
        bool DeleteExtraItem(int id);
        bool SaveExtraItem(ExtraItem item);
        void FillExtended(ExtraItem item);

        // AdRoll
        Advertisable Advertisable(string eid);
        IQueryable<Advertisable> Advertisables();
        IQueryable<Ad> AdRoll_Ads(int? advId = null, string advEid = null);
        IQueryable<AdDailySummary> AdRoll_AdDailySummaries(int? advertisableId, int? adId, DateTime? startDate, DateTime? endDate);
        TDRawStat GetAdRollStat(Ad ad, DateTime? startDate, DateTime? endDate);

        // DBM
        InsertionOrder InsertionOrder(int ioID);
        IQueryable<InsertionOrder> InsertionOrders();
        //IQueryable<Creative> DBM_Creatives(int? ioID);
        IQueryable<CreativeDailySummary> DBM_CreativeDailySummaries(DateTime? startDate, DateTime? endDate, int? ioID = null, int? creativeID = null);
        IQueryable<TDRawStat> GetDBMStatsByCreative(int ioID, DateTime? startDate, DateTime? endDate);
        TDRawStat GetDBMStat(Creative creative, DateTime? startDate, DateTime? endDate);
    }
}
