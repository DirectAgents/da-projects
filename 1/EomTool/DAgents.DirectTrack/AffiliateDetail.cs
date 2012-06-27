using System.Collections.Generic;
using System.Text.RegularExpressions;
using DirectTrack;
using DirectTrack.Rest;
using System.Linq;
namespace DAgents.Synch
{
    public class AffiliateDetail : RestEntity<affiliate>
    {
        public static AffiliateDetail Create(int affid)
        {
            return new AffiliateDetail(DirectTrack.Rest.XmlGetter.ViewAffilaite(affid));
        }
        private AffiliateDetail(string xml)
            : base(xml)
        {
        }
        public string AddCode
        {
            get
            {
                return this.inner.addCode;
            }
        }
        public string Email
        {
            get
            {
                return this.inner.email;
            }
        }
        public string CompanyName
        {
            get
            {
                return this.inner.company;
            }
        }
        public string PaymentCurrency
        {
            get
            {
                return this.inner.currency;
            }
        }
        public IEnumerable<string> AffiliateGroupIDs
        {
            get
            {
                foreach (var item in this.inner.affiliateGroups)
                {
                    yield return Regex.Match(item.location, @"c\d+").Value;
                }
            }
        }
    }
}
