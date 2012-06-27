using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DirectTrack;
using DirectTrack.Rest;

namespace DAgents.Synch
{
    public class CampaignGroup : RestEntity<campaignGroup>
    {
        public CampaignGroup(string xml)
            : base(xml)
        {
        }

        public IEnumerable<int> CampaignPIDs
        {
            get
            {
                foreach (var item in inner.campaigns)
                {
                    string cidStr = Regex.Match(item.location, @"\d+").Value;
                    yield return Convert.ToInt32(cidStr);
                }
            }
        }

        public string Name
        {
            get
            {
                return inner.groupName;
            }
        }
    }
}
