using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Contexts
{
    public partial class CPMReport
    {
        [NotMapped]
        public IEnumerable<CampaignDrop> CampaignDropsOrdered
        {
            get { return CampaignDrops.OrderBy(cd => cd.Date); }
            //TODO: what to order on next? (if multiple drops on the same date)
        }

    }
}
