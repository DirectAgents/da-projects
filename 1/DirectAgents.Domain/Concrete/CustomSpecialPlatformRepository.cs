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

        public CustomSpecialPlatformRepository(ClientPortalProgContext context,
            VcdProvider provider)
        {
            this.context = context;
            this.Provider = provider;
        }

        public IEnumerable<CustomSpecialPlatformLatestsSummary> GetSpecialPlatformStats()
        {
            return Provider.GetCustomDatesRangeByAccounts(context);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
