using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EomToolWeb.Models
{
    public class ListViewMode
    {
        public string TemplateName { get; set; }
        public int ItemsPerPage { get; set; }
        public int EditHeight { get; set; }
        public int EditWidth { get; set; }
    }
}