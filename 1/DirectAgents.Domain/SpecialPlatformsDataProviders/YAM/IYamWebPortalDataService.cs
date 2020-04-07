using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.YAM.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.YAM
{
    /// <summary>
    /// YAM web portal data provider service.
    /// </summary>
    public interface IYamWebPortalDataService
    {
        /// <summary>
        /// Gets the accounts latests information.
        /// </summary>
        /// <returns>Latests dates for accounts.</returns>
        IEnumerable<YamLatestsInfo> GetAccountsLatestsInfo();
    }
}
