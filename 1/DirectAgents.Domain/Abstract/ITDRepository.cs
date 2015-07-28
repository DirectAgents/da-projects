using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;
using DirectAgents.Domain.Entities.AdRoll;

namespace DirectAgents.Domain.Abstract
{
    public interface ITDRepository : IDisposable
    {
        void SaveChanges();

        // AdRoll
        IQueryable<Advertisable> Advertisables();
        IQueryable<AdvertisableStat> AdvertisableStats(int? advertisableId, DateTime? startDate, DateTime? endDate);
        void FillStats(Advertisable adv, DateTime? startDate, DateTime? endDate);
    }
}
