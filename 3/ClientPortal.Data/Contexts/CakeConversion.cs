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
    
    public partial class CakeConversion
    {
        public int Conversion_Id { get; set; }
        public Nullable<System.DateTime> ConversionDate { get; set; }
        public Nullable<int> Affiliate_Id { get; set; }
        public Nullable<int> Offer_Id { get; set; }
        public Nullable<int> Advertiser_Id { get; set; }
        public Nullable<int> Campaign_Id { get; set; }
        public Nullable<int> Creative_Id { get; set; }
        public string CreativeName { get; set; }
        public string Subid1 { get; set; }
        public string ConversionType { get; set; }
        public Nullable<decimal> PricePaid { get; set; }
        public Nullable<decimal> PriceReceived { get; set; }
        public string IpAddress { get; set; }
        public Nullable<int> PricePaidCurrencyId { get; set; }
        public string PricePaidFormattedAmount { get; set; }
        public Nullable<int> PriceReceivedCurrencyId { get; set; }
        public string PriceReceivedFormattedAmount { get; set; }
        public bool Deleted { get; set; }
    }
}
