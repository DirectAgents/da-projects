using System.Linq;
using System.Data;
using DAgents.Common;

namespace EomApp1.Screens.MediaBuyerWorkflow
{
    public class MediaBuyerWorkflowModel
    {
        public void UpdateMediaBuyerApprovalStatus(string fromStatus, string toStatus, string itemIds)
        {
            string sql = @"
                                UPDATE Item 
                                SET media_buyer_approval_status_id = (SELECT id FROM MediaBuyerApprovalStatus WHERE name = '[[ToStatus]]') 
                                WHERE id IN ([[ItemIds]]) AND media_buyer_approval_status_id = (SELECT id FROM MediaBuyerApprovalStatus WHERE name = '[[FromStatus]]') 
                            "
                            .Replace("[[FromStatus]]", fromStatus)
                            .Replace("[[ToStatus]]", toStatus)
                            .Replace("[[ItemIds]]", itemIds)
                            .Trim();

            SqlUtility.ExecuteNonQuery(sql);
        }

        public MediaBuyerWorkflowDataSet.MediaBuyersDataTable MediaBuyers(string status)
        {
            var result = new MediaBuyerWorkflowDataSet.MediaBuyersDataTable();
            using (var db = MediaBuyerWorkflowEntities.Create())
            {
                var publisherPayouts = from c in db.PublisherPayouts
                                       where c.Media_Buyer_Approval_Status == status
                                       group c by c.Media_Buyer;

                foreach (var item in publisherPayouts)
                {
                    int publisherCount = item.Select(c => c.Publisher).Distinct().Count();
                    string costString = item.GroupBy(c => c.Cost_Currency).SumString(c => c.Cost ?? 0);
                    string itemIds = string.Join(",", item.Select(c => c.ItemIds));
                    result.AddMediaBuyersRow(item.Key, publisherCount, costString, itemIds);
                }
            }
            return result;
        }

        public MediaBuyerWorkflowDataSet.PublishersDataTable PublishersByMediaBuyer(string mediaBuyerName, string status)
        {
            var result = new MediaBuyerWorkflowDataSet.PublishersDataTable();
            using (var db = MediaBuyerWorkflowEntities.Create())
            {
                var publisherPayouts = from c in db.PublisherPayouts
                                       where c.Media_Buyer == mediaBuyerName && c.Media_Buyer_Approval_Status == status
                                       select c;

                foreach (var name in publisherPayouts.Select(c => c.Publisher).Distinct())
                {
                    result.AddPublishersRow(name, mediaBuyerName);
                }
            }
            return result;
        }
    }
}
