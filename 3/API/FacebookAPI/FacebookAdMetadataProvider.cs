using FacebookAPI.Entities;
using FacebookAPI.Entities.AdDataEntities;
using Polly;
using System;
using System.Collections.Generic;

namespace FacebookAPI
{
    /// <summary>
    /// Provider for facebook ads metadata. 
    /// </summary>
    /// <seealso cref="FacebookAPI.BaseFacebookDataProvider" />
    public class FacebookAdMetadataProvider : BaseFacebookDataProvider
    {
        private const int metadataFetchingPageSize = 300;

        public const int MaxRetries = 5;

        public FacebookAdMetadataProvider(Action<string> logInfo, Action<string> logError)
           : base(logInfo, logError)
        {
        }

        /// <summary>
        /// Gets all ads data for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        public List<AdCreativeData> TryExtractAllAdsMetadataForAccount(string accountExternalId)
        {
            var maxRetryAttempts = 5;
            var secondsToWait = 5;
            var data = Policy
                .Handle<Exception>()
                .Retry(maxRetryAttempts, (exception, retryCount, context) => LogInfo(String.Format("Ads metadata extraction failed. Waiting {0} seconds before trying again.", secondsToWait)))
                .Execute(() => GetAllAdsMetadataForAccount(accountExternalId));
            return data;
        }

        /// <summary>
        /// Gets all ads data for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        private List<AdCreativeData> GetAllAdsMetadataForAccount(string accountExternalId)
        {
            bool moreData;
            var creativesData = new List<AdCreativeData>();
            var parameters = new
            {
                fields = "id,effective_status,name,creative{image_url,title,body,thumbnail_url,name,id},adset{name,id},campaign{name,id}",
                after = "",
                limit = metadataFetchingPageSize,
            };
            var fbClient = CreateFBClient();
            do
            {
                dynamic result = fbClient.Get($"act_{accountExternalId}/ads", parameters);
                if (result.data != null)
                {
                    creativesData.AddRange(GetPageAdData(result.data));
                }
                moreData = (result.paging != null && result.paging.next != null);
                if (moreData)
                {
                    parameters = new
                    {
                        parameters.fields,
                        after = (string)result.paging.cursors.after,
                        parameters.limit,
                    };
                }
            }
            while (moreData);
            return creativesData;
        }

        private List<AdCreativeData> GetPageAdData(dynamic pageData)
        {
            var res = new List<AdCreativeData>();
            foreach (var row in pageData)
            {
                var rowAdData = InitAdDataFromResponseRow(row);
                res.Add(rowAdData);
            }
            return res;
        }

        private AdCreativeData InitAdDataFromResponseRow(dynamic row)
        {
            var adData = new AdCreativeData
            {
                Id = row.id,
                Name = row.name,
                Status = row.effective_status,
                Creative = new Creative
                {
                    Id = row.creative.id,
                    Name = row.creative.name,
                    Title = row.creative.title,
                    Body = row.creative.body,
                    ImageUrl = row.creative.image_url,
                    ThumbnailUrl = row.creative.thumbnail_url
                }
            };
            return adData;
        }
    }
}
