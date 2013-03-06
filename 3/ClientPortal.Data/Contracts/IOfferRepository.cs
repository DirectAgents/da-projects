using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface IOfferRepository
    {
        IQueryable<OfferInfo> GetOfferInfos(DateTime? start, DateTime? end);
        IQueryable<DailyInfo> GetDailyInfos(DateTime? start, DateTime? end, int advertiserId);
        IQueryable<MonthlyInfo> GetMonthlyInfos(string type, DateTime? start, DateTime? end, int? advertiserId);
        IQueryable<ConversionInfo> GetConversions(DateTime? start, DateTime? end, int? advertiserId);
    }
}
