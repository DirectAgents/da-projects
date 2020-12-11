using CakeExtracter.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Common
{
    class AdfStatsTypeAgg : StatsTypeAgg
    {
        public const string TrackingPointArg = "TRACKPT";

        public bool TrackingPoint { get; set; }

        public override bool All => base.All && TrackingPoint;

        public AdfStatsTypeAgg(string statsTypeString)
            : base(statsTypeString)
        {
            var statsTypeUpper = (statsTypeString == null) ? "" : statsTypeString.ToUpper();
            if (statsTypeUpper.StartsWith(TrackingPointArg))
            {
                TrackingPoint = true;
            }
        }

        public override void SetAllTrue()
        {
            TrackingPoint = true;
            base.SetAllTrue();
        }

    }
}
