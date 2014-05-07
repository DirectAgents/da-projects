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
//                var currencyNames = LineItems.Select(li => li.Currency.name).Distinct();
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
            //get { return LineItems.Sum(li => li.Amount); }
            get { return InvoiceItems.Sum(i => i.amount); }
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
        public IEnumerable<InvoiceItem> SubItems { get; set; }

        public Campaign Campaign { get; set; }
        public Currency Currency { get; set; }
        
        //todo? compute on demand and save as private variable?
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
