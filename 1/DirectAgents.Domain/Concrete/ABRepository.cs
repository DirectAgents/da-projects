using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Domain.Concrete
{
    public class ABRepository : IABRepository, IDisposable
    {
        private ABContext context;

        public ABRepository(ABContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        // ---

        public ABClient Client(int id)
        {
            return context.ABClients.Find(id);
        }

        public IQueryable<ABClient> Clients()
        {
            return context.ABClients;
        }

        public bool AddClient(ABClient client)
        {
            if (context.ABClients.Any(c => c.Id == client.Id))
                return false;
            context.ABClients.Add(client);
            context.SaveChanges();
            return true;
        }
        public void AddClients(IEnumerable<ABClient> clients)
        {
            context.ABClients.AddRange(clients);
            context.SaveChanges();
        }

        public bool SaveClient(ABClient client)
        {
            if (context.ABClients.Any(c => c.Id == client.Id))
            {
                var entry = context.Entry(client);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public ClientAccount ClientAccount(int id)
        {
            return context.ClientAccounts.Find(id);
        }

        public bool SaveClientAccount(ClientAccount clientAccount)
        {
            if (context.ClientAccounts.Any(ca => ca.Id == clientAccount.Id))
            {
                var entry = context.Entry(clientAccount);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public AccountBudget AccountBudget(int clientAccountId, DateTime date)
        {
            return context.AccountBudgets.Find(clientAccountId, date);
        }

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
