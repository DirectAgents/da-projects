using System.Collections.Generic;
using System.Globalization;
using EomTool.Domain.Entities;

namespace EomToolWeb.Models
{
    public class PaymentBatchesViewModel
    {
        public IEnumerable<PaymentBatch> Batches { get; set; }
        public bool AllowHold { get; set; }
    }
}