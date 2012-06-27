namespace EomApp1.Formss.Final
{
    public partial class Campaign
    {
        partial void Oncampaign_status_idChanged()
        {
            string sql =
                string.Format(
                    @"
                    INSERT INTO [dbo].[CampaignStatusAuditTrail]
                    (
                        [campaign_id],
                        [campaign_status_id]
                    )
                    VALUES
                    (
                        {0},
                        {1}
                    )
                    ",
                    pid, campaign_status_id);
            //GlobalHelpers.AddPostSubmitSql(sql);
        }
    }
}