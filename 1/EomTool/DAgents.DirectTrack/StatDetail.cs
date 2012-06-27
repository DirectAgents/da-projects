using System;
using System.Text.RegularExpressions;
using DirectTrack.Rest;
namespace DAgents.Synch
{
    public class StatDetail : RestEntity<resource>
    {
        private string location;
        public StatDetail(string location, resource inner)
            : base(inner)
        {
            this.location = location;
        }
        public string api_url
        {
            get
            {
                return this.location + "," + inner.breakdownResourceURL;
            }
        }
        public int pid
        {
            get
            {
                return Convert.ToInt32(ParseCampaignFromUri(this.location));
            }
        }
        public int affid
        {
            get
            {
                int result = Convert.ToInt32(ParseAffiliateFromUri(this.inner.breakdownResourceURL));
                return result;
            }
        }
        public DateTime date
        {
            get
            {
                string d = this.inner.date;
                string[] pieces = d.Split('-');
                int year = Convert.ToInt32(pieces[0]);
                int month = Convert.ToInt32(pieces[1]);
                int day = Convert.ToInt32(pieces[2]);
                return new DateTime(year, month, day);
            }
        }
        public int clicks
        {
            get
            {
                return Convert.ToInt32(inner.clicks);
            }
        }
        public int leads
        {
            get
            {
                return Convert.ToInt32(inner.leads);
            }
        }
        public int num_sales
        {
            get
            {
                return Convert.ToInt32(inner.numSales);
            }
        }
        public int num_sub_sales
        {
            get
            {
                return Convert.ToInt32(inner.numSubSales);
            }
        }
        public string currency
        {
            get
            {
                return inner.currency;
            }
        }
        public decimal dt_they_get
        {
            get
            {
                return Convert.ToDecimal(inner.theyGet);
            }
        }
        public decimal dt_we_get
        {
            get
            {
                return Convert.ToDecimal(inner.weGet);
            }
        }
        public decimal dt_etc
        {
            get
            {
                return Convert.ToDecimal(inner.epc);
            }
        }
        public decimal dt_revenue
        {
            get
            {
                return Convert.ToDecimal(inner.revenue);
            }
        }
        private static string ParseCampaignFromUri(string uri)
        {
            string pattern = @"^.*/campaign/(\d+).*$";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(uri);
            return matches[0].Groups[1].Captures[0].Value;
        }
        private static string ParseAffiliateFromUri(string uri)
        {
            string pattern = @"^.*\.\./affiliate/(\d+).*$";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(uri);
            return matches[0].Groups[1].Captures[0].Value;
        }
    }
}
