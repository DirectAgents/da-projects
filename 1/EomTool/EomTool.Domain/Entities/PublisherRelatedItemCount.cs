//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EomTool.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class PublisherRelatedItemCount
    {
        public int Id { get; set; }
        public string Publisher { get; set; }
        public Nullable<int> NumNotes { get; set; }
        public Nullable<int> NumAttachments { get; set; }
    }
}
