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
    
    public partial class CreativeStat
    {
        public int CreativeStatId { get; set; }
        public int CampaignDropId { get; set; }
        public string Name { get; set; }
        public Nullable<int> Clicks { get; set; }
        public Nullable<int> Leads { get; set; }
    
        public virtual CampaignDrop CampaignDrop { get; set; }
    }
}
