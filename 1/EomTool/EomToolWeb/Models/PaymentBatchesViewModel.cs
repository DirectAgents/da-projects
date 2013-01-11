using System.Collections.Generic;
using System.Globalization;
using EomTool.Domain.Entities;
using System.Linq;

namespace EomToolWeb.Models
{
    public class PaymentsViewModelBase
    {
        public string Test { get; set; }
        public bool AllowHold { get; set; }

        public PaymentsViewModelBase(string test, string identityName)
        {
            this.Test = test;
            this.AllowHold = (identityName == null || identityName.Contains("jboaz"));
        }
    }

    public class PaymentBatchesViewModel : PaymentsViewModelBase
    {
        public PaymentBatchesViewModel(string test, string identityName) : base(test, identityName) { }

        public IEnumerable<PaymentBatch> Batches { get; set; }
    }

    public class PaymentsViewModel : PaymentsViewModelBase
    {
        public PaymentsViewModel(string test, string identityName) : base(test, identityName) { }

        public IEnumerable<IGrouping<PaymentGroup, PublisherPayment>> PaymentGroups { get; set; }

        public void SetPayments(IEnumerable<PublisherPayment> payments)
        {
            PaymentGroups = payments.GroupBy(p => new PaymentGroup { Currency = p.PubPayCurr, PaymentMethod = p.PaymentMethod, NetTermType = p.NetTermType })
                .OrderBy(g => g.Key.NetTermType)
                .ThenByDescending(g => g.Key.Currency)
                .ThenBy(g => g.Key.PaymentMethod);
        }
    }

    public struct PaymentGroup
    {
        public string Currency;
        public string PaymentMethod;
        public string NetTermType;
    }
}