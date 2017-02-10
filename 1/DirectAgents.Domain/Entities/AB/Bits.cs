using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.AB
{
    public class AcctPaymentBit
    {
        public int Id { get; set; }
        public int AcctPaymentId { get; set; }
        [ForeignKey("AcctPaymentId")]
        public virtual AcctPayment Payment { get; set; }
        public decimal Value { get; set; }

        public int? AcctInvoiceBitId { get; set; }
        [ForeignKey("AcctInvoiceBitId")]
        public virtual AcctInvoiceBit InvoiceBit { get; set; } // (optional) the invoicebit this covers
    }

    public class AcctInvoiceBit
    {
        public int Id { get; set; }
        public int AcctInvoiceId { get; set; }
        [ForeignKey("AcctInvoiceId")]
        public virtual AcctInvoice Invoice { get; set; }
        public decimal Value { get; set; }

        public virtual ICollection<AcctPaymentBit> PaymentBits { get; set; } // the payment(s) received to cover this invoicebit

        public int? AcctSpendBitId { get; set; }
        [ForeignKey("AcctSpendBitId")]
        public virtual AcctSpendBit SpendBit { get; set; } // (optional) the spendbit this covers
    }

    public class AcctSpendBit
    {
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public virtual Period Period { get; set; }
        public int AcctSpendBucketId { get; set; }
        [ForeignKey("AcctSpendBucketId")]
        public virtual AcctSpendBucket SpendBucket { get; set; }

        public virtual ICollection<AcctInvoiceBit> InvoiceBits { get; set; } // the invoicebit(s) that cover this spendbit

        // Quantity/NumUnits ? + calculate unitprice or total rev..
        public decimal Revenue { get; set; }
        public string Desc { get; set; }
    }
}
