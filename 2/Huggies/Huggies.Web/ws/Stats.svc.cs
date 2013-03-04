using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Huggies.Web.ws
{
    public class Stats : IStats
    {
        public StatsResult Get(DateTimeOffset lastChecked)
        {
            return new StatsResult
                {
                    NumNew = 2
                };
        }
    }

    public class StatsResult
    {
        public int NumNew { get; set; }
    }
}
