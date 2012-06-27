
namespace DAgents.Synch
{
    public class PayoutItem
    {
        private string p;

        public string PayoutID
        {
            get { return p; }
            set { p = value; }
        }
        private string p_2;

        public string CampaignID
        {
            get { return p_2; }
            set { p_2 = value; }
        }
        private string p_3;

        public string AffiliateID
        {
            get { return p_3; }
            set { p_3 = value; }
        }

        public PayoutItem(string p, string p_2, string p_3)
        {
            this.p = p;
            this.p_2 = p_2;
            this.p_3 = p_3;
        }
    }
}
