//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientPortal.Data.Contexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class Offer
    {
        public Offer()
        {
            this.Goals = new HashSet<Goal>();
            this.Campaigns = new HashSet<Campaign>();
        }
    
        public int OfferId { get; set; }
        public string OfferName { get; set; }
        public Nullable<int> AdvertiserId { get; set; }
        public string DefaultPriceFormat { get; set; }
        public string Currency { get; set; }
    
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
    }
}
