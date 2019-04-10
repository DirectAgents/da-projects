using Facebook;
using FacebookAPI.Entities;
using FacebookAPI.Entities.AdDataEntities;
using FacebookAPI.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace FacebookAPI
{
    public class AdDataProvider
    {
        private string AccessToken { get; set; }
        private string ApiVersion { get; set; }

        public AdDataProvider()
        {
            AccessToken = ConfigurationManager.AppSettings["FacebookToken"];
            ApiVersion = ConfigurationManager.AppSettings["FacebookApiVersion"];
        }

        public List<AdData> GetAdsCreativesForAccount(string accountId, DateTime start, DateTime end)
        {
            bool moreData;
            var creativesData = new List<AdData>();
            var parameters = new
            {
                fields = "id,effective_status,name,creative{image_url,title,body,thumbnail_url,name,id},adset{name,id},campaign{name,id}",
                after = "",
                time_range = new { since = FacebookRequestUtils.GetDateString(start), until = FacebookRequestUtils.GetDateString(end) },
            };
            var fbClient = new FacebookClient(AccessToken) { Version = "v" + ApiVersion };
            var path = $"act_{accountId}/ads";
            do
            {
                dynamic result = fbClient.Get(path, parameters);
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
                        parameters.time_range
                    };
                }
            }
            while (moreData);
            return creativesData;
        }

        private List<AdData> GetPageAdData(dynamic pageData)
        {
            var res = new List<AdData>();
            foreach (var row in pageData)
            {
                var rowAdData = InitAdDataFromResponseRow(row);
                res.Add(rowAdData);
            }
            return res;
        }

        private AdData InitAdDataFromResponseRow(dynamic row)
        {
            var adData = new AdData
            {
                Id = row.id,
                Name = row.name,
                AdSet = new AdSet
                {
                    Id = row.adset.id,
                    Name = row.adset.name
                },
                Campaign = new Campaign
                {
                    Id = row.campaign.id,
                    Name = row.campaign.name
                },
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
