namespace CakeExtracter.Commands
{
    public class FbStatsTypeAgg : StatsTypeAgg
    {
        public const string ReachArg = "REACH";

        public bool Reach { get; set; }

        public override bool All => base.All && Reach;

        public FbStatsTypeAgg(string statsTypeString)
            : base(statsTypeString)
        {
            var statsTypeUpper = (statsTypeString == null) ? "" : statsTypeString.ToUpper();
            if (statsTypeUpper.StartsWith(ReachArg))
            {
                Reach = true;
            }
        }

        public override void SetAllTrue()
        {
            Reach = true;
            base.SetAllTrue();
        }
    }
}