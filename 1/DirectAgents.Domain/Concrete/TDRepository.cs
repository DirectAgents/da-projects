using System;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.AdRoll;

namespace DirectAgents.Domain.Concrete
{
    public class TDRepository : ITDRepository, IDisposable
    {
        private DATDContext context;

        public TDRepository(DATDContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        #region AdRoll

        public IQueryable<Advertisable> Advertisables()
        {
            return context.Advertisables;
        }

        public IQueryable<AdvertisableStat> AdvertisableStats(int? advertisableId, DateTime? startDate, DateTime? endDate)
        {
            var advStats = context.AdvertisableStats.AsQueryable();
            if (advertisableId.HasValue)
                advStats = advStats.Where(a => a.AdvertisableId == advertisableId.Value);
            if (startDate.HasValue)
                advStats = advStats.Where(a => a.Date >= startDate.Value);
            if (endDate.HasValue)
                advStats = advStats.Where(a => a.Date <= endDate.Value);
            return advStats;
        }

        public void FillStats(Advertisable adv, DateTime? startDate, DateTime? endDate)
        {
            var advStats = AdvertisableStats(adv.Id, startDate, endDate);
            if (advStats.Any())
            {
                adv.Stats = new AdRollStat
                {
                    Impressions = advStats.Sum(a => a.Impressions),
                    Clicks = advStats.Sum(a => a.Clicks),
                    ClickThruConv = advStats.Sum(a => a.CTC),
                    ViewThruConv = advStats.Sum(a => a.VTC),
                    Spend = advStats.Sum(a => a.Cost),
                    Prospects = advStats.Sum(a => a.Prospects)
                };
                adv.Stats.Spend = Math.Round(adv.Stats.Spend, 2);
            }
            else
            {
                adv.Stats = new AdRollStat();
            }
        }
        #endregion

        // ---

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
