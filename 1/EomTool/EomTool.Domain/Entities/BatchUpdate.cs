namespace EomTool.Domain.Entities
{
    public partial class BatchUpdate
    {
        public string Action
        {
            get
            {
                if (this.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Approved
                    && this.from_media_buyer_approval_status_id == MediaBuyerApprovalStatus.Held)
                    return "Released";
                else
                    return (this.MediaBuyerApprovalStatus == null) ? null : this.MediaBuyerApprovalStatus.name;
            }
        }
    }
}
