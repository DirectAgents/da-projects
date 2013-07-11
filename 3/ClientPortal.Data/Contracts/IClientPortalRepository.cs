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
        void AddConversionData(ConversionData entity);
        IQueryable<ConversionData> ConversionData { get; }

        IQueryable<Offer> Offers(int? advertiserId);
        IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId, out string currency);
        DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, int? advertiserId, int? offerId, bool includeConversionData);

        IQueryable<MonthlyInfo> GetMonthlyInfosFromDaily(DateTime? start, DateTime? end, int advertiserId, int? offerId);
        IQueryable<OfferInfo> GetOfferInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<ConversionSummary> GetConversionSummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<AffiliateSummary> GetAffiliateSummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<MonthlyInfo> GetMonthlyInfos(string type, DateTime? start, DateTime? end, int? advertiserId);

        IQueryable<Click> GetClicks(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<DailyInfo> GetDailyInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<Conversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId);

        IEnumerable<DeviceClicks> GetClicksByDeviceName(DateTime? start, DateTime? end, int? advertiserId, int? offerId);

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

        // Search
        IQueryable<SearchStat> GetWeekStats(int? numWeeks);
        IQueryable<SearchStat> GetMonthStats(int? numMonths);
        IQueryable<SearchStat> GetChannelStats();
        IQueryable<SearchStat> GetCampaignStats(DateTime? start, DateTime? end);
        IQueryable<SearchStat> GetCampaignStats(string channel);
        IQueryable<SearchStat> GetAdgroupStats();
    }
}
