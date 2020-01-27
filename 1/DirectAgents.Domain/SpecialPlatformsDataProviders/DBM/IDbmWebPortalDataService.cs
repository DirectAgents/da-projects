using System;
using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.DBM.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.DBM
{
    /// <summary>
    /// DBM web portal data provider service.
    /// </summary>
    public interface IDbmWebPortalDataService
    {
        /// <summary>
        /// Gets the accounts latests information.
        /// </summary>
        /// <returns>Latests dates for accounts.</returns>
        IEnumerable<DbmLatestsInfo> GetAccountsLatestsInfo();
    }
}
