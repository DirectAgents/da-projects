using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Screens.Final.Models
{
    public class CampaignsModel
    {
        public void FillVerifiedCampaigns(Data.DataSet1 data)
        {
            using (var db = Models.EomEntities.Create())
            {
                var campaignsQuery = from campaign in db.Campaigns
                                     from item in db.Items
                                     where campaign.pid == item.pid && item.campaign_status_id == (int)CampaignStatusId.Verified
                                     group item by new { Campaign = campaign, Currency = item.Currency1 } into g
                                     select new
                                     {
                                         Pid = g.Key.Campaign.pid,
                                         Campaign = g.Key.Campaign.campaign_name,
                                         Revenue = g.Sum(c => c.total_revenue),
                                         Currency = g.Key.Currency.name
                                     };

                foreach (var item in campaignsQuery)
                {
                    data.Campaigns.AddCampaignsRow(item.Pid, item.Campaign, item.Revenue.Value, item.Currency);
                }

                var itemsQuery = from item in db.Items
                                 where item.campaign_status_id == (int)CampaignStatusId.Verified
                                 select item;

                foreach (var item in itemsQuery)
                {
                    var row = data.CampaignItems.NewCampaignItemsRow();
                    row.PID = item.pid;
                    row.Id = item.id;
                    data.CampaignItems.AddCampaignItemsRow(row);
                }
            }
        }

        public void UpdateCampaignItemStatus(Data.DataSet1 data, int pid)
        {
            var query = from c in data.CampaignItems
                        where c.PID == pid
                        select c.Id;

            var itemIDs = query.ToList();

            using (var db = EomEntities.Create())
            {
                var items = from c in db.Items
                            where itemIDs.Contains(c.id)
                            select c;

                items.ToList().ForEach(i => i.campaign_status_id = (int)CampaignStatusId.Default);

                db.SaveChanges();
            }
        }

        public void FillCampaignPublishers(Data.DataSet1 data, int pid)
        {
            var publisherModel = new PublishersModel(pid);
            publisherModel.FillPublishers(data, CampaignStatusId.Verified);
        }
    }
}
