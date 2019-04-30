using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models;
using System;
using System.Collections.Generic;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook
{
    /// <summary>
    /// Facebook web portal data provider service.
    /// </summary>
    public interface IFacebookWebPortalDataService
    {
        /// <summary>
        /// Gets the accounts latests information.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FacebookLatestsInfo> GetAccountsLatestsInfo();

        /// <summary>
        /// Gets the accounts totals information on Campaigns Level.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FacebookTotalsInfo> GetCampaignTotalsInfo(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the accounts totals information on Daily Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IEnumerable<FacebookTotalsInfo> GetDailyTotalsInfo(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the accounts totals information on Adsets Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IEnumerable<FacebookTotalsInfo> GetAdsetsTotalsInfo(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the accounts totals information on Ads Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        IEnumerable<FacebookTotalsInfo> GetAdsTotalsInfo(DateTime fromDate, DateTime toDate);
    }
}
