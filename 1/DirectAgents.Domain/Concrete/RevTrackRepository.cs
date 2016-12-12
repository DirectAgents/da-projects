using System;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.RevTrack;

namespace DirectAgents.Domain.Concrete
{
    public partial class RevTrackRepository : IRevTrackRepository, IDisposable
    {
        private RevTrackContext context;

        public RevTrackRepository(RevTrackContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        // ---

        public IQueryable<ProgClient> ProgClients()
        {
            return context.ProgClients;
        }

        public IQueryable<ProgVendor> ProgVendors()
        {
            return context.ProgVendors;
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
