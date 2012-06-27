using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EomApp1.Formss.AB.Data
{
    public class ABItem
    {
        public string Period { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; }
        public string CustomerListId { get; set; }
    }
}
