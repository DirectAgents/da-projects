using System.Linq;

namespace EomApp1.Screens.Final.Models
{
    using UI;

    public class Campaigns : CampaignsBase
    {
        public Campaigns(Data data)
            : base(data)
        {
        }

        public void FillVerifiedCampaigns()
        {
            using (var db = Eom.Create())
            {
                foreach (var item in db.VerifiedItemsCampaignRevenueSummaries)
                {
                    Data.Campaigns.AddCampaignsRow(item.pid, item.CampaignName, item.TotalRevenue.Value, item.CurrencyName, item.ItemIds, item.AdvertiserName, item.CostCurrencyName);
                }
            }
        }

        public void UpdateCampaignItemStatus(int pid, string currency, CampaignStatusId status)
        {
            var ids = Data.Campaigns.ItemIds(pid, currency);

            using (var db = Eom.Create())
            {
                var items = db.Items.Where(item => ids.Contains(item.id));

                foreach (var item in items.ToList())
                {
                    item.campaign_status_id = (int)status;
                }

                db.SaveChanges();
            }
        }
    }
}
