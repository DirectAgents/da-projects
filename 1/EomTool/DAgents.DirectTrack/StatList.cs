using System.Collections.Generic;
using DirectTrack.Rest;
namespace DAgents.Synch
{
    public class StatList : RestEntity<stats>
    {
        public StatList(string xml)
            : base(xml)
        {
        }

        public IEnumerable<StatDetail> StatsDetailList
        {
            get
            {
                if (this.inner.resource != null)
                    foreach (var resource in this.inner.resource)
                    {
                        yield return new StatDetail(this.inner.location, resource);
                    }
            }
        }
    }
}
