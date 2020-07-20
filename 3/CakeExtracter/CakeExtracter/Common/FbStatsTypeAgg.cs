namespace CakeExtracter.Commands
{
    public class FbStatsTypeAgg : StatsTypeAgg
    {
        public const string ReachArg = "REACH";

        public const string CampaignReachArg = "CAMPREACH";

        public bool Reach { get; set; }

        public bool CampaignReach { get; set; }

        public override bool All => base.All && Reach && CampaignReach;

        public FbStatsTypeAgg(string statsTypeString)
            : base(statsTypeString)
        {
            var statsTypeUpper = (statsTypeString == null) ? "" : statsTypeString.ToUpper();
            if (statsTypeUpper.StartsWith(ReachArg))
            {
                Reach = true;
            }
            if (statsTypeUpper.StartsWith(CampaignReachArg))
            {
                CampaignReach = true;
            }
        }

        public override void SetAllTrue()
        {
            Reach = true;
            CampaignReach = true;
            base.SetAllTrue();
        }
    }
}