using System;
using System.Collections.Generic;
using System.Linq;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;

namespace ClientPortal.Data.Contracts
{
    public interface IClientPortalRepository
    {
        void SaveChanges();

        IQueryable<Offer> Offers(int? advertiserId);
        IQueryable<Offer> Offers(int? accountManagerId, int? advertiserId, bool cpmOnly, int? minCampaigns = null);
        Offer GetOffer(int id);
        IQueryable<Campaign> Campaigns(int? offerId, bool cpmOnly);
        Campaign GetCampaign(int id);
        IQueryable<Creative> Creatives(int? offerId);
        Creative GetCreative(int id);
        bool SaveCreative(Creative creative, bool saveChanges = false);
        void FillExtended_Creative(Creative creative);

        IQueryable<CPMReport> CPMReports(int? offerId);
        CPMReport GetCPMReport(int id, bool includeAdvertiser = false);
        void SaveCPMReport(CPMReport inReport, bool saveChanges = false);
        bool AddDropToCPMReport(int cpmReportId, int campaignDropId, bool saveChanges = false);
        bool RemoveDropFromCPMReport(int cpmReportId, int campaignDropId, bool saveChanges = false);
        void FillExtended_CPMReport(CPMReport inReport);

        IQueryable<CampaignDrop> CampaignDrops(int? offerId, int? campaignId);
        CampaignDrop GetCampaignDrop(int id);
        CampaignDrop AddCampaignDrop(int campaignId, DateTime date, string subject, decimal? cost, int creativeId, bool saveChanges = false);
        bool SaveCampaignDrop(CampaignDrop campaignDrop, bool saveChanges = false);
        void FillExtended_CampaignDrop(CampaignDrop campaignDrop);
        Campaign DeleteCampaignDrop(int campaignDropId, bool saveChanges = false);

        void SaveCreativeStat(CreativeStat creativeStat, bool saveChanges = false);
        int? DeleteCreativeStat(int creativeStatId, bool saveChanges = false);
        bool UpdateCreativeStatFromSummaries(int creativeStatId, bool saveChanges = false);

        IQueryable<Advertiser> Advertisers { get; }
        IQueryable<Contact> Contacts { get; }
        void AddAdvertiser(Advertiser entity);
        void AddContact(Contact entity);
        Advertiser GetAdvertiser(int id);
        Contact GetContact(string search);

        IQueryable<ScheduledReport> GetScheduledReports(int advertiserId);
        ScheduledReport GetScheduledReport(int id);
        void AddScheduledReport(ScheduledReport scheduledReport);
        //void SaveScheduledReport(ScheduledReport scheduledReport);
        bool DeleteScheduledReport(int id, int? advertiserId);
        void DeleteScheduledReportRecipient(ScheduledReportRecipient recipient);

        IQueryable<FileUpload> GetFileUploads(int? advertiserId);
        FileUpload GetFileUpload(int id);
        void AddFileUpload(FileUpload fileUpload, bool saveChanges = false);
        void DeleteFileUpload(FileUpload fileUpload, bool saveChanges = false);

        IQueryable<Goal> Goals { get; }
        IQueryable<Goal> GetGoals(int advertiserId);
        Goal GetGoal(int id);
        void AddGoal(Goal goal, bool saveChanges = false);
        bool DeleteGoal(int id, int? advertiserId);

        IQueryable<UserEvent> UserEvents { get; }
        void AddUserEvent(UserEvent userEvent, bool saveChanges = false);
        void AddUserEvent(string userName, string eventString, bool saveChanges = false);
        void AddUserEvent(int userId, string eventString, bool saveChanges = false);

        // Stats
        IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId, out string currency);
        DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, int? advertiserId, int? offerId, bool includeConversionData);

        IQueryable<MonthlyInfo> GetMonthlyInfosFromDaily(DateTime? start, DateTime? end, int advertiserId, int? offerId);
        IQueryable<OfferInfo> GetOfferInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<DailyInfo> GetDailyInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<ConversionSummary> GetConversionSummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<AffiliateSummary> GetAffiliateSummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<MonthlyInfo> GetMonthlyInfos(string type, DateTime? start, DateTime? end, int? advertiserId);

        IQueryable<Conversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IList<DeviceClicks> GetClicksByDeviceName(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IList<ConversionsByRegion> GetConversionCountsByRegion(DateTime start, DateTime end, int advertiserId);

        void AddConversionData(ConversionData entity);
        IQueryable<ConversionData> ConversionData { get; }

        // Search
        SearchStat GetSearchStats(int? advertiserId, DateTime? start, DateTime? end, bool includeToday = true);
        IQueryable<SearchStat> GetWeekStats(int? advertiserId, string channel, int? searchAccountId, string channelPrefix, string device, int? numWeeks, DayOfWeek startDayOfWeek, bool useAnalytics, bool includeToday);
        IQueryable<WeeklySearchStat> GetCampaignWeekStats2(int? advertiserId, DateTime start, DateTime end, DayOfWeek startDayOfWeek, bool useAnalytics);
        IQueryable<SearchStat> GetMonthStats(int? advertiserId, int? numMonths, bool useAnalytics, bool includeToday);
        IQueryable<SearchStat> GetChannelStats(int? advertiserId, DayOfWeek startDayOfWeek, bool useAnalytics, bool includeToday, bool includeAccountBreakdown, bool includeSearchChannels);
        IQueryable<SearchStat> GetCampaignStats(int? advertiserId, string channel, DateTime? start, DateTime? end, bool breakdown, bool useAnalytics);
        IQueryable<SearchStat> GetAdgroupStats();
    }
}
