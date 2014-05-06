using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EomTool.Domain.Entities
{
    public partial class Invoice
    {
        [NotMapped]
        public Advertiser Advertiser { get; set; }

        //[NotMapped]
        public List<InvoiceLineItem> LineItems = new List<InvoiceLineItem>();
    }

    // intended for one campaign (pid)
    public class InvoiceLineItem
    {
        public IEnumerable<InvoiceItem> SubItems { get; set; }

        public Campaign Campaign { get; set; }
        public Currency Currency { get; set; }
        
        public decimal Amount
        {
            get { return SubItems.Sum(i => i.amount); }
        }

        public int NumUnits
        {
            get { return SubItems.Sum(i => i.num_units); }
        }
    }
}
