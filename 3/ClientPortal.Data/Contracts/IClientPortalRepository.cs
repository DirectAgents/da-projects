using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface IClientPortalRepository
    {
        DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, string advertiserId, int? offerId);

        IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId);

        IQueryable<Click> GetClicks(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<Conversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
    }
}
