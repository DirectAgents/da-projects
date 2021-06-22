using System;
using System.Collections.Generic;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.SpecialPlatformProviders.Implementation;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.Concrete
{
    public class CustomSpecialPlatformRepository : ICustomSpecialPlatformRepository
    {
        public VcdProvider Provider { get; set; }

        private ClientPortalProgContext context;

        private bool disposed;

        public CustomSpecialPlatformRepository(ClientPortalProgContext context,
            VcdProvider provider)
        {
            this.context = context;
            this.Provider = provider;
        }

        ~CustomSpecialPlatformRepository()
        {
            Dispose(false);
        }

        public IEnumerable<CustomSpecialPlatformLatestsSummary> GetSpecialPlatformStats()
        {
            return Provider.GetCustomDatesRangeByAccounts(context);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
