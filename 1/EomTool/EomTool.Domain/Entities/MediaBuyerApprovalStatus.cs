
namespace EomTool.Domain.Entities
{
    public partial class MediaBuyerApprovalStatus
    {
        //TODO: change to const ?
        public static int Default = 1;
        public static int Queued = 2;
        public static int Sent = 3;
        public static int Approved = 4;
        public static int Held = 5;
    }
}
