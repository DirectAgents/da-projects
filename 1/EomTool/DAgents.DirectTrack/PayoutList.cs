using System.Collections.Generic;
using DirectTrack.Rest;
namespace DAgents.Synch
{
    class PayoutList : RestEntity<resourceList>
    {
        private PayoutList(string xml)
            : base(xml)
        {
        }
        public static PayoutList FromPID(int pid)
        {
            return new PayoutList(XmlGetter.ListPayouts(pid));
        }
        public IEnumerable<PayoutItem> PayoutItems
        {
            get
            {
                if (this.inner.resourceURL != null)
                    foreach (var item in this.inner.resourceURL)
                        yield return new PayoutItem(
                            item.metaData1,
                            item.metaData2,
                            item.metaData3);
            }
        }
    }
}
