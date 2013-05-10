using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface IClientPortalRepository
    {
        void SaveChanges();
        void AddConversionData(ConversionData entity);
        IQueryable<ConversionData> ConversionData { get; }

        DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, string advertiserId, int? offerId, bool includeConversionData);

        IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId);

        IQueryable<Click> GetClicks(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<Conversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
    }
}
