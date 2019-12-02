using System;
using System.Collections.Generic;
using FacebookAPI.Constants;
using FacebookAPI.Entities.AdDataEntities;
using Polly;

namespace FacebookAPI.Providers
{
    /// <summary>
    /// Provider for facebook ads metadata. 
    /// </summary>
    /// <seealso cref="BaseFacebookDataProvider" />
    public class FacebookAdMetadataProvider : BaseFacebookDataProvider
    {
        private const int MetadataFetchingPageSize = 300;

        private const int MaxRetries = 5;

        private const int SecondsToWaitBetweenRetries = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookAdMetadataProvider"/> class.
        /// </summary>
        /// <param name="logInfo">Log Info Action.</param>
        /// <param name="logError">Log Error Action.</param>
        public FacebookAdMetadataProvider(Action<string> logInfo, Action<string> logError)
           : base(logInfo, logError)
        {
        }

        /// <summary>
        /// Extracts all ads metadata for account.
        /// </summary>
        /// <param name="accountExternalId">The account external identifier.</param>
        /// <returns>Collection of Ads Creative Data.</returns>
        public List<AdCreativeData> ExtractAllAdsMetadataForAccount(string accountExternalId)
        {
            var allAdsMetadata = new List<AdCreativeData>();
            try
            {
                // by default Facebook Api doesn't return archived ad's info. Need separate request to fetch archived data.
                allAdsMetadata.AddRange(TryExtractAdsMetadataForAccount(accountExternalId, false));
                allAdsMetadata.AddRange(TryExtractAdsMetadataForAccount(accountExternalId, true));
            }
            catch
            {
                LogError("Failed Fetch Ads Metadata values");
            }
            return allAdsMetadata;
        }

        private List<AdCreativeData> TryExtractAdsMetadataForAccount(string accountExternalId, bool isArchived)
        {
            var data = Policy
                .Handle<Exception>()
                .WaitAndRetry(MaxRetries, GetPauseBetweenAttempts, (exception, timeSpan, retryCount, context) =>
                    LogInfo($"Ads metadata extraction failed. Waiting {SecondsToWaitBetweenRetries} seconds before trying again."))
                .Execute(() => GetAllAdsMetadataForAccount(accountExternalId, isArchived));
            return data;
        }

        /// <summary>
        /// Extracts Ad Metadata for account.
        /// </summary>
        /// <param name="accountExternalId">Account External Id.</param>
        /// <param name="isArchived">Is Archived Flag.</param>
        /// <returns>Collection of Ads Creative Data.</returns>
        private List<AdCreativeData> GetAllAdsMetadataForAccount(string accountExternalId, bool isArchived)
        {
            bool moreData;
            var creativesData = new List<AdCreativeData>();
            var fbClient = CreateFBClient();
            dynamic parameters = InitRequestParameters(string.Empty, isArchived);
            do
            {
                dynamic result = fbClient.Get($"act_{accountExternalId}/ads", parameters);
                if (result.data != null)
                {
                    creativesData.AddRange(GetPageAdData(result.data));
                }
                moreData = result.paging != null && result.paging.next != null;
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
                limit = MetadataFetchingPageSize,
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
                    ThumbnailUrl = row.creative.thumbnail_url,
                },
            };
            return adData;
        }

        private TimeSpan GetPauseBetweenAttempts(int attemptNumber)
        {
            return new TimeSpan(0, 0, SecondsToWaitBetweenRetries);
        }
    }
}
