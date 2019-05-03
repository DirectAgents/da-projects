using FacebookAPI.Constants;
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

        private const int maxRetries = 5;

        private const int secondsToWaitBetweenReties = 5;

        public FacebookAdMetadataProvider(Action<string> logInfo, Action<string> logError)
           : base(logInfo, logError)
        {
        }

        /// <summary>
        /// Extracts all ads metadata for account.
        /// </summary>
        /// <param name="accountExternalId">The account external identifier.</param>
        /// <returns></returns>
        public List<AdCreativeData> ExtractAllAdsMetadataForAccount(string accountExternalId)
        {
            var allAdsMetadata = new List<AdCreativeData>();
            try
            {
                // by default Facebook Api doesn't return archived ad's info. Need separate request to fetch archived data.
                allAdsMetadata.AddRange(TryExtractAdsMetadataForAccount(accountExternalId, false));
                allAdsMetadata.AddRange(TryExtractAdsMetadataForAccount(accountExternalId, true)); 
            }
            catch (Exception ex)
            {
                LogError("Failed Fetch Ads Metadata values");
            }
            return allAdsMetadata;
        }

        private List<AdCreativeData> TryExtractAdsMetadataForAccount(string accountExternalId, bool isArchived)
        {
            var data = Policy
                .Handle<Exception>()
                .Retry(maxRetries, (exception, retryCount, context) =>
                    LogInfo($"Ads metadata extraction failed. Waiting {secondsToWaitBetweenReties} seconds before trying again."))
                .Execute(() => GetAllAdsMetadataForAccount(accountExternalId, isArchived));
            return data;
        }

        /// <summary>
        /// Gets all ads data for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        private List<AdCreativeData> GetAllAdsMetadataForAccount(string accountExternalId, bool isArchived)
        {
            bool moreData;
            var creativesData = new List<AdCreativeData>();
            var fbClient = CreateFBClient();
            dynamic parameters = InitRequestParameters(String.Empty, isArchived);
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
                    parameters = InitRequestParameters((string)result.paging.cursors.after, isArchived);
                }
            }
            while (moreData);
            return creativesData;
        }

        private dynamic InitRequestParameters(string afterValue, bool isArchived)
        {
            const string fieldsToFetchValue = "id,effective_status,name,creative{image_url,title,body,thumbnail_url,name,id},adset{name,id},campaign{name,id}";
            dynamic parameters = new
            {
                fields = fieldsToFetchValue,
                after = afterValue,
                effective_status = isArchived ? new string[] { EffectiveStatuses.Archived } : null,
                limit = metadataFetchingPageSize,
            };
            return parameters;
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
