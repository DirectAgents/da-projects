using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ClientPortal.Data.Contexts
{
    public class SearchProfile_Validation
    {
        [Range(0, 6)]
        public int StartDayOfWeek { get; set; }
    }

    [MetadataType(typeof(SearchProfile_Validation))]
    public partial class SearchProfile
    {
        [NotMapped]
        public IOrderedEnumerable<SearchProfileContact> SearchProfileContactsOrdered
        {
            get { return this.SearchProfileContacts.OrderBy(sc => sc.Order); }
        }

        public DateTime GetNext_WeekStartDate(bool includeToday = false)
        {
            return Common.GetNext_WeekStartDate((DayOfWeek)this.StartDayOfWeek, includeToday);
        }

        public DateTime GetLast_WeekEndDate(bool includeToday = false)
        {
            return Common.GetLast_WeekEndDate((DayOfWeek)this.StartDayOfWeek, includeToday);
        }
    }
}
