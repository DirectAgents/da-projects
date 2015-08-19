using System;
using System.Linq;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.TD;

namespace DirectAgents.Domain.Abstract
{
    public interface ITDRepository : IDisposable
    {
        void SaveChanges();

        // TD
        IQueryable<Account> Accounts(string platformCode);
        IQueryable<DailySummary> DailySummaries(DateTime? startDate, DateTime? endDate, int? accountId = null);
        TDStat GetTDStat(DateTime? startDate, DateTime? endDate, Account account = null);

        // AdRoll
        IQueryable<Advertisable> Advertisables();
        IQueryable<Ad> AdRoll_Ads(int? advId);
        IQueryable<AdvertisableStat> AdvertisableStats(int? advertisableId, DateTime? startDate, DateTime? endDate);
        IQueryable<AdDailySummary> AdRoll_AdDailySummaries(int? advertisableId, int? adId, DateTime? startDate, DateTime? endDate);
        void FillStats(Advertisable adv, DateTime? startDate, DateTime? endDate);
        AdRollStat GetAdRollStat(Advertisable adv, DateTime? startDate, DateTime? endDate);
        AdRollStat GetAdRollStat(Ad ad, DateTime? startDate, DateTime? endDate);
    }
}
