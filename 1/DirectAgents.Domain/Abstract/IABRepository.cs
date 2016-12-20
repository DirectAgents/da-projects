using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Domain.Abstract
{
    public interface IABRepository : IDisposable
    {
        void SaveChanges();

        IQueryable<ABClient> ABClients();
        bool AddClient(ABClient client);
        void AddClients(IEnumerable<ABClient> clients);
    }
}
