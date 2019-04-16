using System;
using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;

namespace CakeExtracter.Etl.SocialMarketing.Extracters
{
    public class FacebookAdPreviewExtracter : Extracter<FBAdPreview>
    {
        private IEnumerable<string> fbAdIds;

        private FacebbokAdPreviewDataProvider adPreviewDataProvider;

        private readonly int accountId;

        public FacebookAdPreviewExtracter(ExtAccount account, IEnumerable<string> fbAdIds, FacebbokAdPreviewDataProvider adPreviewDataProvider)
        {
            this.fbAdIds = fbAdIds;
            accountId = account.Id;
            this.adPreviewDataProvider = adPreviewDataProvider;
        }

        protected override void Extract()
        {
            Logger.Info(accountId, "Extracting Ad Previews from Facebook API for ({0})", this.accountId);
            try
            {
                var fbAds = adPreviewDataProvider.GetAdPreviews(fbAdIds);
                Add(fbAds);
            }
            catch (Exception ex)
            {
                Logger.Error(accountId, ex);
            }
            End();
        }
    }
}
