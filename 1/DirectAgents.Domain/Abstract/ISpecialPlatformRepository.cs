using System;
using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformProviders.Contracts;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.Abstract
{
    public interface ISpecialPlatformRepository : IDisposable
    {
        void SaveChanges();
        IEnumerable<SpecialPlatformProvider> SpecialPlatformProviders { get; set; }
        /// <summary>
        /// The method gets the date ranges (earliest - latest) jobs of all special platforms.
        /// Can specify a specific platform code
        /// </summary>
        /// <param name="platformCode">Code of platform</param>
        /// <returns>List of date ranges (earliest - latest) jobs of all special platforms</returns>
        IEnumerable<SpecialPlatformLatestsSummary> GetSpecialPlatformStats(string platformCode);
    }
}
