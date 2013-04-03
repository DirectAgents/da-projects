using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface ICakeRepository
    {
        void SaveChanges();

        IQueryable<CakeAdvertiser> Advertisers { get; }
        CakeAdvertiser Advertiser(int advertiserId);
        IQueryable<CakeOffer> Offers(int? advertiserId);
        IQueryable<DailySummary> GetDailySummaries(DateTime? start, DateTime? end, int advertiserId, int? offerId, out string currency);
        DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, int advertiserId, int? offerId);
        IQueryable<MonthlyInfo> GetMonthlyInfosFromDaily(DateTime? start, DateTime? end, int advertiserId, int? offerId);
        IQueryable<OfferInfo> GetOfferInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<DailyInfo> GetDailyInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<MonthlyInfo> GetMonthlyInfos(string type, DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<ConversionSummary> GetConversionSummaries(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<CakeConversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<CakeConversion> Conversions { get; }
    }
}
