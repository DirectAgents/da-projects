using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DirectTrack.Rest;
namespace DAgents.Synch
{
    public class PayoutDetail : RestEntity<payout>
    {
        private readonly string payoutID;

        public static IEnumerable<PayoutDetail> FromCampaignId(int pid)
        {
            foreach (var item in PayoutList.FromPID(pid).PayoutItems)
            {
                yield return PayoutDetail.FromPayoutId(item.PayoutID);
            }
        }
        public static PayoutDetail FromPayoutId(string payoutID)
        {
            return new PayoutDetail(
                payoutID,
                DirectTrack.Rest.XmlGetter.ViewPayout(payoutID));
        }
        private PayoutDetail(string payoutID, string xml)
            : base(xml)
        {
            this.payoutID = payoutID;
        }
        public string PayoutId
        {
            get
            {
                return this.payoutID;
            }
        }
        public string PayoutType
        {
            get
            {
                return this.inner.payoutType.ToString();
            }
        }
        public bool IsAllAffiliates
        {
            get
            {
                if (this.inner.affiliate.allAffiliatesSpecified == true &&
                    this.inner.affiliate.allAffiliates == booleanInt3.Item1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public int AffiliateId
        {
            get
            {
                return Convert.ToInt32(Regex.Match(this.inner.affiliate.affiliateResourceURL, @"\d+$").Value);
            }
        }
        public int CampaignId
        {
            get
            {
                return Convert.ToInt32(Regex.Match(this.inner.campaignResourceURL.location, @"\d+$").Value);
            }
        }

        public decimal Impression
        {
            get
            {
                return this.inner.impressionSpecified ? this.inner.impression : 0;
            }
        }
        public decimal Click
        {
            get
            {
                return this.inner.clickSpecified ? this.inner.click : 0;
            }
        }
        public decimal Lead
        {
            get
            {
                return this.inner.leadSpecified ? this.inner.lead : 0;
            }
        }
        public decimal PercentSale
        {
            get
            {
                return this.inner.percentSaleSpecified ? this.inner.percentSale : 0;
            }
        }
        public decimal FlatSale
        {
            get
            {
                return this.inner.flatSaleSpecified ? this.inner.flatSale : 0;
            }
        }
        public decimal PercentSubSale
        {
            get
            {
                return this.inner.percentSubSaleSpecified ? this.inner.percentSubSale : 0;
            }
        }
        public decimal FlatSubSale
        {
            get
            {
                return this.inner.flatSubSaleSpecified ? this.inner.flatSubSale : 0;
            }
        }
        public DateTime EffectiveDate
        {
            get
            {
                return Convert.ToDateTime(this.inner.effectiveDate);
            }
        }
        public DateTime ModifyDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(this.inner.modifyDate);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
        }
        public string Curency
        {
            get
            {
                return this.inner.currency;
            }
        }
        public int ProductId
        {
            get
            {
                return this.inner.productResourceURL != null ? Convert.ToInt32(
                    Regex.Match(this.inner.productResourceURL.location, @"\d+$").Value) : 0;
            }
        }
    }
}
