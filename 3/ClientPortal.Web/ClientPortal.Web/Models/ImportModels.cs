using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class ScooterRow
    {
        public string TransactionId { get; set; }
        public int FTNSO1 { get; set; }
    }

    public class TreeRow
    {
        public string QFormUID { get; set; }
        public string CPA { get; set; }
    }
}