using System.Collections.Generic;
using DirectTrack.Rest;
using System.Linq;
using DAgents.Common;

namespace DAgents.Synch
{
    public class AdvertiserList : RestEntity<resourceList>
    {
        private AdvertiserList(string xml)
            : base(xml)
        {
            inner = XmlGetter.ExpandResourceList(inner, new ConsoleLogger());
        }

        public static AdvertiserList PullFromDirectTrack()
        {
            return new AdvertiserList(XmlGetter.ListAdvertisers());
        }

        public IEnumerable<AdvertiserItem> Items
        {
            get
            {
                foreach (var resourceURL in this.inner.resourceURL)
                {
                    yield return new AdvertiserItem(resourceURL);
                }
            }
        }
    }
}
