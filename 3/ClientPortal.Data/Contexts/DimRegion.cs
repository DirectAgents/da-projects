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
    
    public partial class DimRegion
    {
        public DimRegion()
        {
            this.FactClicks = new HashSet<FactClick>();
        }
    
        public int RegionKey { get; set; }
        public string RegionCode { get; set; }
    
        public virtual ICollection<FactClick> FactClicks { get; set; }
    }
}
