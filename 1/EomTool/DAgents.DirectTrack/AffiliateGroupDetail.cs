using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DirectTrack;
using DirectTrack.Rest;

namespace DAgents.Synch
{
    public class AffiliateGroupDetail : RestEntity<affiliateGroup>
    {
        public AffiliateGroupDetail(string xml)
            : base(xml)
        {
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
