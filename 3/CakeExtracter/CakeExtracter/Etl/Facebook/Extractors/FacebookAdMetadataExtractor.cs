using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;
using FacebookAPI.Entities.AdDataEntities;

namespace CakeExtracter.Etl.Facebook.Extractors
{
    /// <summary>
    /// Facebook ad metadata extractor.
    /// Ad creative data can't be extracted together with ads insights stats. 
    /// Separate call to facebook graph api needed to extract creative's stats.
    /// </summary>
    public class FacebookAdMetadataExtractor
    {
        private static ConcurrentDictionary<string, object> extIdLocksDictionary = new ConcurrentDictionary<string, object>();
        private static ConcurrentDictionary<string, List<AdCreativeData>> creativesDataDictionary = new ConcurrentDictionary<string, List<AdCreativeData>>();
        private readonly FacebookAdMetadataProvider adMetadataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdMetadataExtractor"/> class.
        /// </summary>
        /// <param name="adMetadataProvider">The ad metadata provider.</param>
        public FacebookAdMetadataExtractor(FacebookAdMetadataProvider adMetadataProvider)
        {
            this.adMetadataProvider = adMetadataProvider;
        }

        /// <summary>
        /// Gets the ad creatives data by account external id.
        /// </summary>
        /// <param name="accountExtId">The account ext identifier.</param>
        /// <returns></returns>
        public List<AdCreativeData> GetAdCreativesData(ExtAccount account)
        {
            // For each facebook account there's 4(per each platform) accounts with the same external id. To not fetch the same ads data for each account
            //there is a creativesDataDictionary and multi threading concurrency stuff for filling dictionary and arbitrating ETL processing. 
            lock (extIdLocksDictionary.GetOrAdd(account.ExternalId, (val) => new object()))
            {
                try
                {
                    if (creativesDataDictionary.ContainsKey(account.ExternalId))
                    {
                        return creativesDataDictionary[account.ExternalId];
                    }
                    else
                    {
                        Logger.Info(account.Id, "Fetching ads metadata.");
                        var adData = adMetadataProvider.ExtractAllAdsMetadataForAccount(account.ExternalId);
                        creativesDataDictionary[account.ExternalId] = adData;
                        return adData;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(account.Id, ex);
                    return new List<AdCreativeData>();
                }
            }
        }
    }
}
