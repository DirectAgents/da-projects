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

        [NotMapped]
        public string CurrencyName
        {
            get
            {
                var currencyNames = InvoiceItems.Select(i => i.CurrencyName).Distinct();

                if (currencyNames.Count() > 1)
                    return "mixed";
                else if (currencyNames.Count() == 1)
                    return currencyNames.First();
                else
                    return null;
            }
        }

        [NotMapped]
        public decimal Total
        {
            get { return InvoiceItems.Sum(i => i.total_amount.HasValue ? i.total_amount.Value : 0); }
        }

        [NotMapped]
        public DateTime DateRequested
        {
            get { return (FirstNote != null) ? FirstNote.created : new DateTime(2000, 1, 1); }
        }

        [NotMapped]
        public InvoiceNote FirstNote
        {
            get
            {
                if (InvoiceNotes.Count == 0)
                    return null;
                else
                    return InvoiceNotes.OrderBy(n => n.created).First();
            }
        }

        [NotMapped]
        public InvoiceNote LatestNote
        {
            get
            {
                if (InvoiceNotes.Count == 0)
                    return null;
                else
                    return InvoiceNotes.OrderBy(n => n.created).Last();
            }
        }
        [NotMapped]
        public string LatestNoteString
        {
            get
            {
                var latestNote = LatestNote;
                return (latestNote == null) ? null : latestNote.note;
            }
        }
    }

    // intended for one campaign (pid)
    public class InvoiceLineItem
    {
        public Campaign Campaign { get; set; }
        public Currency Currency { get; set; }
        public string ItemCode { get; set; }

        public IEnumerable<InvoiceItem> SubItems { get; set; }

        public decimal TotalAmount
        {
            get { return SubItems.Sum(i => i.total_amount.HasValue ? i.total_amount.Value : 0); }
        }
        public int NumUnits
        {
            get { return SubItems.Sum(i => i.num_units); }
        }
        public decimal AmountPerUnit
        {
            get { return TotalAmount / NumUnits; }
        }
    }
}
