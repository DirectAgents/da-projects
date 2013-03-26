using System;

namespace ClientPortal.Web.Models
{
    public class InvoiceModel
    {
        public DateTime TxnDate { get; set; }
        public string ClassName { get; set; }
        public string Currency { get; set; }
        public string RefNumber { get; set; }
        public string Memo { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
    }
}