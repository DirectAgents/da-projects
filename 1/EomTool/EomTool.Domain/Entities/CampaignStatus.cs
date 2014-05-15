
namespace EomTool.Domain.Entities
{
    public partial class CampaignStatus
    {
        public const int Default = 1;
        public const int Finalized = 2;
        public const int Active = 3;
        public const int Verified = 4;

        public static string DisplayVal(int? campaignStatus)
        {
            if (campaignStatus.HasValue)
                return DisplayVal(campaignStatus.Value);
            else
                return "All";
        }
        public static string DisplayVal(int campaignStatus)
        {
            switch (campaignStatus)
            {
                case CampaignStatus.Default:
                    return "Unfinalized";
                case CampaignStatus.Finalized:
                    return "Finalized";
                case CampaignStatus.Verified:
                    return "Verified";
                default:
                    return "unknown";
            }
        }
    }
}
