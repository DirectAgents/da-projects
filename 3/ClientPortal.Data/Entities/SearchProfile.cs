using System;
using System.Collections.Generic;
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
        public bool UseAnalytics // Google Analytics, that is
        {
            get { return false; } //TODO: move column from Advertiser to SearchProfile
        }
        [NotMapped]
        public bool UseConvertedClicks // as opposed to "Conversions"; for AdWords only
        {
            get { return true; } //TODO: add column, admin UI
        }
        [NotMapped]
        public bool ShowViewThrus // (View-Through Conversions)
        {
            get { return true; } //TODO: add column, admin UI
        }

        [NotMapped]
        public bool ShowCalls
        {
            get { return !String.IsNullOrWhiteSpace(this.LCaccid); }
        }

        [NotMapped]
        public IOrderedEnumerable<SearchProfileContact> SearchProfileContactsOrdered
        {
            get { return this.SearchProfileContacts.OrderBy(sc => sc.Order); }
        }

        [NotMapped]
        public IEnumerable<SearchAccount> GoogleSearchAccounts
        {
            get { return this.SearchAccounts == null ? null : this.SearchAccounts.Where(sa => sa.Channel == "Google"); }
        }
        [NotMapped]
        public IEnumerable<SearchAccount> BingSearchAccounts
        {
            get { return this.SearchAccounts == null ? null : this.SearchAccounts.Where(sa => sa.Channel == "Bing"); }
        }
        [NotMapped]
        public IEnumerable<SearchAccount> CriteoSearchAccounts
        {
            get { return this.SearchAccounts == null ? null : this.SearchAccounts.Where(sa => sa.Channel == "Criteo"); }
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
