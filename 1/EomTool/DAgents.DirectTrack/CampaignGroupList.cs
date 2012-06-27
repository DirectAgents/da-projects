using System.Collections.Generic;
using DAgents.Common;
using DirectTrack.Rest;
namespace DAgents.Synch
{
    public class CampaignGroupList : RestEntity<resourceList>
    {
        private ILogger logger;

        public CampaignGroupList(string xml, ILogger logger)
            : base(xml)
        {
            this.logger = logger;
        }

        public CampaignGroupList(string xml)
            : this(xml, null)
        {
        }

        public static CampaignGroupList PullFromDirectTrack()
        {
            return new CampaignGroupList(XmlGetter.ListCampaignGroups());
        }

        public IEnumerable<CampaignGroup> CampaignGroups
        {
            get
            {
                foreach (var resourceURL in this.inner.resourceURL)
                {
                    string id = resourceURL.location;
                    string xml = XmlGetter.ViewCampaignGroup(id);
                    yield return new CampaignGroup(xml);
                }
            }
        }

        public int ResourceCount
        {
            get
            {
                return this.inner.resourceURL.Length;
            }
        }
    }
}
