using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Domain.Concrete
{
    public class ClientRepository : IClientRepository
    {
        // The underlying repositories:
        private ICPProgRepository cpProgRepo;
        private ICPSearchRepository cpSearchRepo;

        //NOTE: In DAWeb, the underlying repositories are instantiated via ninject and disposed via ControllerBase.Dispose().
        //      We don't have the IRepositories in a constructor here so the controllers can access the child repositories directly.

        public void SetRepositories(ICPProgRepository cpProgRepo, ICPSearchRepository cpSearchRepo)
        {
            this.cpProgRepo = cpProgRepo;
            this.cpSearchRepo = cpSearchRepo;
        }

        public IEnumerable<ClientReport> ClientReports()
        {
            var clientReports = cpSearchRepo.ClientReports().ToList();
            foreach (var cRep in clientReports)
            {
                if (cRep.ProgCampaignId.HasValue)
                    cRep.ProgCampaign = cpProgRepo.Campaign(cRep.ProgCampaignId.Value);
                if (cRep.SearchProfileId.HasValue)
                    cRep.SearchProfile = cpSearchRepo.GetSearchProfile(cRep.SearchProfileId.Value);
                yield return cRep;
            }
        }
    }
}
