using System;
using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Domain.Abstract
{
    public interface IClientRepository
    {
        void SetRepositories(ICPProgRepository cpProgRepo, ICPSearchRepository cpSearchRepo);

        IEnumerable<ClientReport> ClientReports();
    }
}
