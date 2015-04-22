using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EomToolWeb.Models
{
    public class IntIdName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class IntValueText
    {
        public int value { get; set; }
        public string text { get; set; }
    }

    public class StringValueText
    {
        public string value { get; set; }
        public string text { get; set; }
    }
}