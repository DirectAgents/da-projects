﻿using System.ComponentModel.DataAnnotations;
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
    }
}
