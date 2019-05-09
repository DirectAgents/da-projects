using System;
using System.Linq;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.DBM;

namespace DirectAgents.Domain.Abstract
{
    public interface ITDRepository : IDisposable
    {
        void SaveChanges();

        #region AdRoll

        Advertisable Advertisable(string eid);
        IQueryable<Advertisable> Advertisables();
        IQueryable<Ad> AdRoll_Ads(int? advId = null, string advEid = null);
        IQueryable<AdDailySummary> AdRoll_AdDailySummaries(int? advertisableId, int? adId, DateTime? startDate, DateTime? endDate);
        TDRawStat GetAdRollStat(Ad ad, DateTime? startDate, DateTime? endDate);

        #endregion

        #region DBM

        InsertionOrder InsertionOrder(int ioID);
        IQueryable<InsertionOrder> InsertionOrders();
        IQueryable<TDRawStat> GetDBMStatsByCreative(int ioID, DateTime? startDate, DateTime? endDate);
        
        #endregion
    }
}
