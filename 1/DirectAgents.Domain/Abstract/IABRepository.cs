using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Domain.Abstract
{
    public interface IABRepository : IDisposable
    {
        void SaveChanges();

        ABClient Client(int id);
        IQueryable<ABClient> Clients();
        bool AddClient(ABClient client);
        void AddClients(IEnumerable<ABClient> clients);
        bool SaveClient(ABClient client);

        ClientBudget ClientBudget(int clientId, DateTime date);
        ClientPayment ClientPayment(int id);

        ClientAccount ClientAccount(int id);
        bool SaveClientAccount(ClientAccount clientAccount);
        AccountBudget AccountBudget(int clientAccountId, DateTime date);
    }
}
