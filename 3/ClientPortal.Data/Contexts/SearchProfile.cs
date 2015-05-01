//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientPortal.Data.Contexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class SearchProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SearchProfile()
        {
            this.StartDayOfWeek = 1;
            this.UserProfiles = new HashSet<UserProfile>();
            this.SearchAccounts = new HashSet<SearchAccount>();
            this.SearchProfileContacts = new HashSet<SearchProfileContact>();
            this.SimpleReports = new HashSet<SimpleReport>();
        }
    
        public int SearchProfileId { get; set; }
        public string SearchProfileName { get; set; }
        public int StartDayOfWeek { get; set; }
        public bool ShowSearchChannels { get; set; }
        public string LCaccid { get; set; }
        public int CallMinSeconds { get; set; }
        public bool ShowRevenue { get; set; }
        public bool UseConvertedClicks { get; set; }
        public bool ShowViewThrus { get; set; }
        public bool ShowCassConvs { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchAccount> SearchAccounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchProfileContact> SearchProfileContacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SimpleReport> SimpleReports { get; set; }
    }
}
