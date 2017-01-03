using System;
using System.Collections.Generic;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Domain.Abstract
{
    public interface ISuperRepository
    {
        void SetRepositories(IMainRepository mainRepo, IRevTrackRepository rtRepo, IABRepository abRepo);

        IEnumerable<ABStat> StatsByClient(DateTime monthStart, int? maxClients = null);
        IEnumerable<ABStat> StatsForClient(int id, DateTime monthStart);
    }
}
