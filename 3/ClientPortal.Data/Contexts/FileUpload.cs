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
    
    public partial class FileUpload
    {
        public int Id { get; set; }
        public System.DateTime UploadDate { get; set; }
        public string Filename { get; set; }
        public string Text { get; set; }
        public Nullable<int> AdvertiserId { get; set; }
    
        public virtual Advertiser Advertiser { get; set; }
    }
}
