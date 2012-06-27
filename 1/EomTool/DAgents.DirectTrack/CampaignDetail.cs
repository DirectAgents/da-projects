using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DirectTrack;
using DirectTrack.Rest;
namespace DAgents.Synch
{
    public class CampaignDetail : RestEntity<campaign>
    {
        public CampaignDetail(string xml)
            : base(xml)
        {
        }

        public static CampaignDetail Create(int pid)
        {
            string xml = XmlGetter.ViewCampaign(pid);
            return new CampaignDetail(xml);
        }

        public string CampaignName
        {
            get
            {
                return this.inner.campaignName;
            }
        }
        public string CampaignType
        {
            get
            {
                return this.inner.campaignType.ToString();
            }
        }
        public IEnumerable<int> CampaignGroupIDs
        {
            get
            {
                foreach (var item in this.inner.campaignGroups)
                {
                    yield return Convert.ToInt32(Regex.Match(item.location, @"\d+").Value);
                }
            }
        }
        public string CampaignStatus
        {
            get
            {
                if (inner.statusSpecified)
                    return Enum.GetName(typeof(status1), inner.status);
                else
                    return string.Empty;
            }
        }
        public IEnumerable<string> AllowedCountries
        {
            get
            {
                if (inner.allowedCountries.allCountriesSpecified
                    && inner.allowedCountries.allCountries == booleanInt1.Item1)
                {
                    yield return "ALL";
                }
                else
                {
                    foreach (var item in inner.allowedCountries.country)
                    {
                        yield return item;
                    }
                }
            }
        }
        public string CampaignUrl
        {
            get
            {
                return inner.campaignURL;
            }
        }
    }
}
