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

        DateRangeSummary GetDateRangeSummary(DateTime? start, DateTime? end, string advertiserId, int? offerId, bool includeConversionData);

        IQueryable<ConversionInfo> GetConversionInfos(DateTime? start, DateTime? end, int? advertiserId, int? offerId);

        IQueryable<Click> GetClicks(DateTime? start, DateTime? end, int? advertiserId, int? offerId);
        IQueryable<Conversion> GetConversions(DateTime? start, DateTime? end, int? advertiserId, int? offerId);

        IEnumerable<DeviceClicks> GetClicksByDeviceName(DateTime? start, DateTime? end, int? advertiserId, int? offerId);

        IQueryable<Advertiser> Advertisers { get; }
        IQueryable<Contact> Contacts { get; }
        void AddAdvertiser(Advertiser entity);
        void AddContact(Contact entity);
        Advertiser GetAdvertiser(int id);
        Contact GetContact(string search);

        IQueryable<Goal> GetGoals(int advertiserId);
        Goal GetGoal(int id);
        bool DeleteGoal(int id, int? advertiserId);
    }
}
