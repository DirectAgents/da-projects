using System;
using DirectTrack.Rest;

namespace DAgents.Synch
{
    public class CampaignItem : RestEntity<resourceListResourceURL>
    {
        public CampaignItem(resourceListResourceURL inner)
            : base(inner)
        {
        }

        // Map metaData1 --> CampaignId
        // Converts string to Int32
        public int CampaignId
        {
            get
            {
                return Convert.ToInt32(this.inner.metaData1);
            }
        }

        // Map metaData2 --> CampaignName
        public string CampaignName
        {
            get
            {
                return this.inner.metaData2;
            }
        }

        // Map metaData3 --> CampaignType
        public string CampaignType
        {
            get
            {
                return this.inner.metaData3;
            }
        }

    }
}
