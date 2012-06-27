using System;
using DirectTrack.Rest;
using System.Linq;

namespace DAgents.Synch
{
    public class AdvertiserItem : RestEntity<resourceListResourceURL>
    {
        public AdvertiserItem(resourceListResourceURL resourceListResourceURL)
            : base(resourceListResourceURL)
        {
        }

        public string Company
        {
            get
            {
                return inner.metaData1;
            }
        }

        public string Name
        {
            get
            {
                return inner.metaData2;
            }
        }

        public string Email
        {
            get
            {
                return inner.metaData3;
            }
        }

        public int DirectTrackAdvertiserId
        {
            get
            {
                string last = inner.location.Split('/').Last();
                return Int32.Parse(last);
            }
        }

        public string GetDetail()
        {
            return XmlGetter.ViewAdvertiser(DirectTrackAdvertiserId);
        }
    }
}
