using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using DAgents.Common;
using System.IO;
using System.Linq;
namespace DirectTrack.Rest
{
    public static class XmlGetter
    {
        private const string UrlNoClientId = "https://da-tracking.com/apifleet/rest/1137/";
        private const string UrlWithClientId = UrlNoClientId + "1/";
        public static string GetXml(string url)
        {
            XmlDocument xmldoc = DirectTrackRestCall.GetXmlDoc(url);
            return Utilities.FormatXml(xmldoc);
        }
        public static XDocument GetDoc(string url)
        {
            var xml = DirectTrackRestCall.GetXml(UrlNoClientId + url);
            var result = XDocument.Parse(xml);
            return result;
        }
        public static string ListPayouts(int pid)
        {
            return GetXml(UrlWithClientId + "payout/campaign/" + pid + "/");
        }
        public static string ViewPayout(string payoutID)
        {
            return GetXml(UrlNoClientId + "payout/" + payoutID);
        }
        public static string ViewAffilaite(int affid)
        {
            return GetXml(UrlNoClientId + "affiliate/" + affid)
                .Replace("<accessID />", "<accessID>999</accessID>")
                .Replace("<paymentMethod />", "<paymentMethod>check</paymentMethod>")
                .Replace("<approval />", "");
        }
        public static string ListAdvertisers()
        {
            return DirectTrackRestCall.GetXml(UrlWithClientId + "advertiser/");
        }
        public static string ViewAdvertiser(int directTrackAdvertiserId)
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "advertiser/" + directTrackAdvertiserId);
        }
        public static string ListCampaigns()
        {
            return DirectTrackRestCall.GetXml(UrlWithClientId + "campaign/");
        }
        public static string ListActiveCampaigns()
        {
            return DirectTrackRestCall.GetXml(UrlWithClientId + "campaign/active/");
        }
        public static string ViewCampaignGroup(string id)
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "campaignGroup/" + id);
        }
        public static string ViewAffiliateGroup(string id)
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "affiliateGroup/" + id);
        }
        public static string ListCampaignGroups()
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "campaignGroup/");
        }
        public static string ViewCampaign(int pid)
        {
            return DirectTrackRestCall.GetXml(UrlNoClientId + "campaign/" + pid);
        }
        public static string ByAffiliateForASingleDay(int pid, DateTime dt)
        {
            string xml = GetXml(UrlWithClientId + "statCampaign/affiliate/campaign/" + pid + "/" + dt.ToString("yyyy-MM-dd"));
            return xml;
        }
        public static void GetResrouces(string startURL, List<XDocument> result, ILogger logger)
        {
            logger.Log("Getting resources at " + startURL);
            var doc = XDocument.Parse(GetXml(startURL));
            var xs = new XmlSerializer(typeof(resourceList));
            if (xs.CanDeserialize(doc.CreateReader()))
            {
                var rl = (resourceList)xs.Deserialize(doc.CreateReader());
                foreach (var item in rl.resourceURL)
                {
                    GetResrouces(startURL + item.location, result, logger);
                }
            }
            else
            {
                result.Add(doc);
            }
        }
        public static void GetResrouces2(string startURL, List<XDocument> result, ILogger logger)
        {
            try
            {
                logger.Log("Getting resources at " + startURL);
                var resultXDocument = XDocument.Parse(GetXml(startURL));
                var xmlSerializer = new XmlSerializer(typeof(resourceList));
                var xmlReader = resultXDocument.CreateReader();
                if (xmlSerializer.CanDeserialize(xmlReader))
                {
                    var resourceList = (resourceList)xmlSerializer.Deserialize(xmlReader);
                    foreach (var resourceURL in resourceList.resourceURL)
                    {
                        GetResrouces2(startURL + resourceURL.location, result, logger);
                    }
                }
                else
                {
                    result.Add(resultXDocument);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static resourceList ExpandResourceList(resourceList source, ILogger logger)
        {
            var result = new resourceList();
            ExpandResourceList(source, result, logger);
            return result;
        }
        static void ExpandResourceList(resourceList source, resourceList result, ILogger logger)
        {
            try
            {
                // loop through each resourceListResourceURL in the resourceList
                foreach (var resourceListResourceURL in source.resourceURL)
                {
                    // check if the current resourceListResourceURL has metadata
                    if (string.IsNullOrEmpty(resourceListResourceURL.metaData1))
                    {
                        // if the current resourceListResourceURL does not have metadata,
                        // get the xml at the location it points to and call this
                        // function recursively.
                        string xml = GetXml(source.location + resourceListResourceURL.location);
                        XmlReader xmlReader = XmlReader.Create(new StringReader(xml));
                        XmlSerializer serializer = new XmlSerializer(typeof(resourceList));
                        if (serializer.CanDeserialize(xmlReader))
                        {
                            ExpandResourceList((resourceList)serializer.Deserialize(xmlReader), result, logger);
                        }
                    }
                    else
                    {
                        var list = result.resourceURL != null ? result.resourceURL.ToList() : new List<resourceListResourceURL>();
                        list.Add(resourceListResourceURL);
                        result.resourceURL = list.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
    }
}
