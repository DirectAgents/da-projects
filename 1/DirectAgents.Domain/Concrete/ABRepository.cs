using System;
using System.Collections.Generic;
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

        public IQueryable<ABClient> ABClients()
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
