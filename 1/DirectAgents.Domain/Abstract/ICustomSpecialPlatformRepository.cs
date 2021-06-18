using System;
using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformProviders.Implementation;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Models;

namespace DirectAgents.Domain.Abstract
{
    public interface ICustomSpecialPlatformRepository : IDisposable
    {
        void SaveChanges();

        VcdProvider Provider { get; set; }

        /// <summary>
        /// The method gets the date ranges (earliest - latest) jobs of all special platforms.
        /// </summary>
        /// <returns>List of date ranges (earliest - latest) jobs of all special platforms.</returns>
        IEnumerable<CustomSpecialPlatformLatestsSummary> GetSpecialPlatformStats();
    }
}
