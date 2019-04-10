using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;

namespace DirectAgents.Domain.Concrete
{
    public class SpecialPlatformRepository : ISpecialPlatformRepository
    {
        public IEnumerable<SpecialPlatformProvider> SpecialPlatformProviders { get; set; }

        private ClientPortalProgContext context;

        public SpecialPlatformRepository(ClientPortalProgContext context,
            IEnumerable<SpecialPlatformProvider> specialPlatformProviders)
        {
            this.context = context;
            this.SpecialPlatformProviders = specialPlatformProviders;
        }

        public IEnumerable<SpecialPlatformSummary> GetSpecialPlatformStats(string platformCode)
        {
            if (this.SpecialPlatformProviders.All(job => job.PlatformCode != platformCode))
            {
                return GetAllSummaries(this.SpecialPlatformProviders);
            }

            var currentJob = GetCurrentJob(this.SpecialPlatformProviders, platformCode);
            return currentJob.GetDatesRangeByAccounts(context);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
        
        private IEnumerable<SpecialPlatformSummary> GetAllSummaries(IEnumerable<SpecialPlatformProvider> providers)
        {
            var allSummaries = new List<SpecialPlatformSummary>();
            foreach (var provider in providers)
            {
                allSummaries.AddRange(provider.GetDatesRangeByAccounts(context));
            }
            return allSummaries;
        }

        private static SpecialPlatformProvider GetCurrentJob(IEnumerable<SpecialPlatformProvider> jobs, string platformCode)
        {
            var currentJob = jobs.First(job => job.PlatformCode == platformCode);
            return currentJob;
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
