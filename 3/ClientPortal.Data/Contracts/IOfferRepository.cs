using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface IOfferRepository
    {
        void SaveChanges();

        IQueryable<OfferInfo> GetOfferInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<DailyInfo> GetDailyInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<MonthlyInfo> GetMonthlyInfos(string type, DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<CakeConversion> Conversions { get; }
    }
}
