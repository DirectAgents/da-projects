using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Contexts
{
    public partial class SearchProfile
    {
        [NotMapped]
        public IOrderedEnumerable<SearchProfileContact> SearchProfileContactsOrdered
        {
            get { return this.SearchProfileContacts.OrderBy(sc => sc.Order); }
        }
    }
}
