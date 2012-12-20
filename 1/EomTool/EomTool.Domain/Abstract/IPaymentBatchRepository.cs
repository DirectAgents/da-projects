using System.Linq;
using System.Security.Principal;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface IPaymentBatchRepository
    {
        IQueryable<PaymentBatch> PaymentBatches { get; }
        IQueryable<PaymentBatch> PaymentBatchesForUser(IPrincipal user);

        IQueryable<PublisherPayment> PublisherPayments { get; }

        void SetAccountingStatus(int[] itemIds, int accountingStatus);
    }
}
