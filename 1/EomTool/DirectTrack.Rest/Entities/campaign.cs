using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace DirectTrack.Rest.Entities
{
    public partial class campaign
    {
        private campaign()
        {
        }

        public static campaign Create(string directTrackResourceXML)
        {
            var reader = XDocument.Parse(directTrackResourceXML).CreateReader();
            return (campaign)(new XmlSerializer(typeof(campaign))).Deserialize(reader);
        }

        public int CampaignNumber
        {
            get
            {
                string s = Regex.Match(location, "campaign/(.*)$").Groups[1].Value;
                return Int32.Parse(s);
            }
        }

        public string CampaignName
        {
            get
            {
                return this.campaignName;
            }
        }

        public string CampaignType
        {
            get
            {
                return campaignType.ToString();
            }
        }

        public IEnumerable<int> CampaignGroupIDs
        {
            get
            {
                foreach (var item in campaignGroups)
                {
                    yield return Convert.ToInt32(Regex.Match(item.location, @"\d+").Value);
                }
            }
        }

        public string CampaignStatus
        {
            get
            {
                if (this.statusSpecified)
                {
                    return Enum.GetName(typeof(status1), status);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public IEnumerable<string> AllowedCountries
        {
            get
            {
                if (allowedCountries.allCountriesSpecified && allowedCountries.allCountries == booleanInt.Item1)
                {
                    yield return "All Countries";
                }
                else
                {
                    foreach (var country in allowedCountries.country)
                    {
                        yield return country;
                    }
                }
            }
        }

        public string CampaignUrl
        {
            get
            {
                return campaignURL;
            }
        }
    }
}
