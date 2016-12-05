using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Domain.Abstract
{
    public interface ISuperRepository
    {
        void SetRepositories(IMainRepository mainRepo, IRevTrackRepository rtRepo);

        IEnumerable<ABStat> StatsByClient(DateTime monthStart);
    }
}
