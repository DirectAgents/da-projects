using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace EomToolWeb.Models
{
    public class BlogPost
    {
        public string Title { get; set; } 
        public DateTime PostedOn { get; set; }
        public string Tags { get; set; }
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string Content { get; set; }
    }
}