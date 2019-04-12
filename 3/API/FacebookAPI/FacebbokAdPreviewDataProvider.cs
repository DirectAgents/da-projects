using Facebook;
using FacebookAPI.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FacebookAPI
{
    public class FacebbokAdPreviewDataProvider : BaseFacebookDataProvider
    {
        public FacebbokAdPreviewDataProvider(Action<string> logInfo, Action<string> logError)
           : base(logInfo, logError)
        {
        }

        public IEnumerable<FBAdPreview> GetAdPreviews(IEnumerable<string> fbAdIds)
        {
            //TODO: create FacebookClient once, here. Then pass in to GetAdPreviewsAPI().
            foreach (var adId in fbAdIds)
            {
                var fbAdPreviews = GetAdPreviewsAPI(adId);

                foreach (var fbAdPreview in fbAdPreviews)
                {
                    yield return fbAdPreview;
                }
            }
        }

        // Reference: https://developers.facebook.com/docs/marketing-api/generatepreview/v2.6
        public IEnumerable<FBAdPreview> GetAdPreviewsAPI(string adId)
        {
            LogInfo(string.Format("GetAdPreviews (adId: {0})", adId));
            var fbClient = CreateFBClient();
            var path = adId + "/previews";

            var afterVal = "";
            bool moreData;
            do
            {
                moreData = false;
                var parms = new
                {
                    ad_format = "DESKTOP_FEED_STANDARD",
                    after = afterVal
                };
                dynamic retObj = null;
                int tryNumber = 0;
                do
                {
                    try
                    {
                        retObj = fbClient.Get(path, parms);
                        tryNumber = 0; // Mark as call succeeded (no exception)
                    }
                    catch (Exception ex)
                    {
                        LogError(ex.Message);
                        tryNumber++;
                        if (tryNumber < 10)
                        {
                            LogInfo("Waiting 90 seconds before trying again.");
                            Thread.Sleep(90000);
                        }
                    }
                } while (tryNumber > 0 && tryNumber < 10);
                if (tryNumber >= 10)
                    throw new Exception("Tried 10 times. Throwing exception.");

                if (retObj == null)
                    continue;

                if (retObj.data != null)
                {
                    foreach (var row in retObj.data)
                    {
                        var fbAdPreview = new FBAdPreview
                        {
                            AdId = adId,
                            BodyHTML = System.Net.WebUtility.HtmlDecode(row.body)
                        };
                        yield return fbAdPreview;
                    }
                }
                moreData = (retObj.paging != null && retObj.paging.next != null);
                if (moreData)
                    afterVal = retObj.paging.cursors.after;
            } while (moreData);
        }

        private FacebookClient CreateFBClient()
        {
            var fbClient = new FacebookClient(AccessToken) { Version = "v" + ApiVersion };
            return fbClient;
        }
    }
}
