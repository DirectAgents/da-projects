using System.Collections.Concurrent;
using System.Collections.Generic;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters.V2
{
    /// <summary>
    /// Facebook ad metadata extractor.
    /// Ad creative data can't be extracted together with ads insights stats. 
    /// Separate call to facebook graph api needed to extract cerative's stats.
    /// </summary>
    public class FacebookAdMetadataExtractorV2
    {
        private static ConcurrentDictionary<string, object> extIdLocksDictionary = new ConcurrentDictionary<string, object>();

        private static ConcurrentDictionary<string, List<AdCreativeData>> creativesDataDictionary = new ConcurrentDictionary<string, List<AdCreativeData>>();

        private readonly FacebookAdMetadataProvider adMetadataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdMetadataExtractorV2"/> class.
        /// </summary>
        /// <param name="adMetadataProvider">The ad metadata provider.</param>
        public FacebookAdMetadataExtractorV2(FacebookAdMetadataProvider adMetadataProvider)
        {
            this.adMetadataProvider = adMetadataProvider;
        }

        /// <summary>
        /// Gets the ad creatives data by account external id.
        /// </summary>
        /// <param name="accountExtId">The account ext identifier.</param>
        /// <returns></returns>
        public List<AdCreativeData> GetAdCreativesData(string  accountExtId)
        {
            // For each facebook account there's 4(per each platform) accounts with the same external id. To not fetch the same ads data for each account
            //there is a creativesDataDictionary and multithreading concurrency stuff for filling dictionary and arbitrating ETL processing. 
            lock (extIdLocksDictionary.GetOrAdd(accountExtId, (val) => new object()))
            {
                if (creativesDataDictionary.ContainsKey(accountExtId))
                {
                    return creativesDataDictionary[accountExtId];
                }
                else
                {
                    Logger.Info("Fetching metadata.");
                    var adData = adMetadataProvider.TryExtractAllAdsMetadataForAccount(accountExtId);
                    creativesDataDictionary[accountExtId] = adData;
                    return adData;
                }
            }
        }
    }
}
