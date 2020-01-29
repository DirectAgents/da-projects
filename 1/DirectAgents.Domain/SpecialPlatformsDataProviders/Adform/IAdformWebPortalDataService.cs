using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Adform.Models;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Adform
{
    /// <summary>
    /// Adform web portal data provider service.
    /// </summary>
    public interface IAdformWebPortalDataService
    {
        /// <summary>
        /// Gets the accounts latests information.
        /// </summary>
        /// <returns>Latests dates for accounts.</returns>
        IEnumerable<AdformLatestsInfo> GetAccountsLatestsInfo();
    }
}
