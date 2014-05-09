using EomTool.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace EomToolWeb.Models
{
    public class InvoicesAccounting
    {
        private IQueryable<Invoice> _Invoices { get; set; }

        public InvoicesAccounting(IQueryable<Invoice> invoices)
        {
            this._Invoices = invoices;
        }

        public IEnumerable<Invoice> Invoices(bool complete)
        {
            if (complete)
                return _Invoices.Where(i => i.invoice_status_id == InvoiceStatus.Generated);
            else
                return _Invoices.Where(i => i.invoice_status_id < InvoiceStatus.Generated);
        }

    }
}