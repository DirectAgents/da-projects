using System.Collections.Concurrent;
using System.Collections.Generic;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters
{
    public class FacebookAdMetadataExtracter
    {
        private static ConcurrentDictionary<string, object> extIdLocksDictionary = new ConcurrentDictionary<string, object>();

        private static ConcurrentDictionary<string, List<AdCreativeData>> creativesDataDictionary = new ConcurrentDictionary<string, List<AdCreativeData>>();

        private readonly FacebookAdMetadataProvider adMetadataProvider;

        public FacebookAdMetadataExtracter(FacebookAdMetadataProvider adMetadataProvider)
        {
            this.adMetadataProvider = adMetadataProvider;
        }

        public List<AdCreativeData> GetAdCreativesData(string  accountExtId)
        {
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
