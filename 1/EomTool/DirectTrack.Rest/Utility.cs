using System;
using System.Xml;
using DAgents.Common;
namespace DirectTrack.Rest
{
    public static class Utility
    {
        /*
        private const string BASE1 = "https://da-tracking.com/apifleet/rest/1137/";
        private const string BASE2 = BASE1 + "1/";

        // Takes a URL to a DirectTrack REST API endpoint (GET) and retrieves the response as
        // a string.
        public static string GetXml(string url)
        {
            XmlDocument xmldoc = DirectTrackRestCall.GetXmlDoc(url);
            return Utilities.FormatXml(xmldoc);
        }

        public static class AffiliateManagement
        {
            public static class Affiliates
            {
                public static class GetAffiliateDetail
                {
                    public static string ViewAffilaite(int affid)
                    {
                        return GetXml(BASE1 + "affiliate/" + affid)
                            .Replace("<accessID />", "<accessID>999</accessID>")
                            .Replace("<paymentMethod />", "<paymentMethod>check</paymentMethod>")
                            .Replace("<approval />", "");
                    }
                }
            }
            public static class AffiliateGroups
            {
                public static class ListAffiliateGroup
                {
                    public static string ListAffilaiteGroup()
                    {
                        return GetXml(BASE2 + "affiliateGroup/");
                    }
                }
            }
        }
        public static class CampaignManagement
        {
            public static class Campaigns
            {
                public static class GetCampaignDetail
                {
                    public static string ViewCampaign(int pid)
                    {
                        return GetXml(BASE1 + "campaign/" + pid);
                    }
                }
            }
            public static class CampaignGroups
            {
                public static class ListCampaignGroup
                {
                    public static string ListCampaignGroups()
                    {
                        return GetXml(BASE1 + "campaignGroup/");
                    }
                }
            }
            public static class Payouts
            {
                public static class ListPayout
                {
                    public static string ListPayouts(int pid)
                    {
                        return GetXml(BASE2 + "payout/campaign/" + pid + "/");
                    }
                }
                public static class GetPayout
                {
                    public static string ViewPayout(string payoutID)
                    {
                        return GetXml(BASE1 + "payout/" + payoutID);
                    }
                }
            }
        }
        public static class DataManagement
        {
            public static class Leads
            {
                public static class ListProgramLeads
                {
                    public static string GetProgramLeadsByCampaignAndDateXml(int cid, DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("programLead/campaign/{0}/{1}-{2}-{3}/",
                            cid, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                    public static string GetProgramLeadsByAffiliateAndDateXml(int aid, DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("programLead/affiliate/{0}/{1}-{2}-{3}/",
                            aid, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                    public static string GetProgramLeadsByCampaignAndAffiliateAndDateXml(int cid, int aid, DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("programLead/campaign/{0}/affiliate/{1}/{2}-{3}-{4}/",
                            cid, aid, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                }
            }
            public static class Sales
            {
                public static class ListSaleDetail
                {
                    public static string GetDetailedSaleByCampaignAndDateXml(int cid, DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("saleDetail/campaign/{0}/{1}-{2}-{3}/",
                            cid, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                    public static string GetDetailedSaleByAffiliateAndDateXml(int aid, DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("saleDetail/affiliate/{0}/{1}-{2}-{3}/",
                            aid, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                    public static string GetDetailedSaleByCampaignAndAffiliateAndDateXml(int cid, int aid, DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("saleDetail/campaign/{0}/affiliate/{1}/{2}-{3}-{4}/",
                            cid, aid, dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                }
            }
            public static class Reporting
            {
                public static class CampaignStatistics
                {
                    public static class GetCampaignStatistics
                    {
                        public static string ByAffiliateForASingleDay(int pid, DateTime dt)
                        {
                            string xml = GetXml(BASE2 + "statCampaign/affiliate/campaign/" +
                                pid + "/" + dt.ToString("yyyy-MM-dd"));

                            string fn = @"c:\zzzNovEom\statxml\"
                                + System.DateTime.Now.Ticks.ToString() + ".xml";

                            System.IO.File.WriteAllText(fn, xml);
                            return xml;

                        }
                    }
                }
                public static class QuickStatistics
                {
                    public static string GetCampaignQuickReportXmlForDay(DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("statCampaign/quick/{0}-{1}-{2}",
                            dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                    public static string GetCampaignQuickReportXmlForMonth(DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("statCampaign/quick/{0}-{1}",
                            dt.Year.ToString(), dt.Month.ToString()));
                    }
                    public static string GetAffiliateQuickReportXmlForDay(DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("statAffiliate/quick/{0}-{1}-{2}",
                            dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                    public static string GetAffiliateQuickReportXmlForMonth(DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("statAffiliate/quick/{0}-{1}",
                            dt.Year.ToString(), dt.Month.ToString()));
                    }
                    public static string GetCreativeQuickReportXmlForDay(DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("statCreative/quick/{0}-{1}-{2}",
                            dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                    public static string GetCreativeQuickReportXmlForMonth(DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("statCreative/quick/{0}-{1}",
                            dt.Year.ToString(), dt.Month.ToString()));
                    }
                    public static string GetProductQuickReportXmlForDay(DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("statProduct/quick/{0}-{1}-{2}",
                            dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()));
                    }
                    public static string GetProductQuickReportXmlForMonth(DateTime dt)
                    {
                        return GetXml(BASE2 + string.Format("statProduct/quick/{0}-{1}",
                            dt.Year.ToString(), dt.Month.ToString()));
                    }
                }

            }
        }
         */
    }
}
