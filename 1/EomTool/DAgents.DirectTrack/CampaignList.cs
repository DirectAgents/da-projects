using System.Collections.Generic;
using DirectTrack.Rest;
using System.Linq;

namespace DAgents.Synch
{
    public class CampaignList : RestEntity<resourceList>
    {
        private CampaignList(string xml)
            : base(xml)
        {
        }

        public static CampaignList PullFromDirectTrack()
        {
            return new CampaignList(XmlGetter.ListCampaigns());
        }

        public static CampaignList PullActiveFromDirectTrack()
        {
            return new CampaignList(XmlGetter.ListActiveCampaigns());
        }

        public static List<CampaignItem> ActiveCampaigns()
        {
            return new CampaignList(XmlGetter.ListActiveCampaigns()).CampaignItems.ToList();
        }

        public IEnumerable<CampaignItem> CampaignItems
        {
            get
            {
                foreach (var resourceURL in this.inner.resourceURL)
                {
                    yield return new CampaignItem(resourceURL);
                }
            }
        }
    }
}
