using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class FileUploadInfo
    {
        public int Id { get; set; }
        public DateTime UploadDate { get; set; }
        public string Filename { get; set; }
        public int? CakeAdvertiserId { get; set; }
    }
}