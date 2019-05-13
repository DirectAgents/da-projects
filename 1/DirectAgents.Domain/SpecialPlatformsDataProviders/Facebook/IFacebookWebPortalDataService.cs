using System;
using System.Collections.Generic;
using DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models;

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
        /// <returns>Latests dates for accounts</returns>
        IEnumerable<FacebookLatestsInfo> GetAccountsLatestsInfo();

        /// <summary>
        /// Gets the accounts totals information on Campaigns Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Latests Campaigns Totals for accounts.</returns>
        IEnumerable<FacebookTotalsInfo> GetCampaignTotalsInfo(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the accounts totals information on Daily Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Latests Daily Totals for accounts.</returns>
        IEnumerable<FacebookTotalsInfo> GetDailyTotalsInfo(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the accounts totals information on Adsets Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Latests Adsets Totals for accounts.</returns>
        IEnumerable<FacebookTotalsInfo> GetAdsetsTotalsInfo(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the accounts totals information on Ads Level.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Latests Ads Totals for accounts.</returns>
        IEnumerable<FacebookTotalsInfo> GetAdsTotalsInfo(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the ad set actions totals information.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Adset Actions Totals Info.</returns>
        IEnumerable<FacebookAccountActionsTotals> GetAdSetActionsTotalsInfo(DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Gets the ads actions totals information.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns>Ads Actions Totals Info.</returns>
        IEnumerable<FacebookAccountActionsTotals> GetAdsActionsTotalsInfo(DateTime fromDate, DateTime toDate);
    }
}
