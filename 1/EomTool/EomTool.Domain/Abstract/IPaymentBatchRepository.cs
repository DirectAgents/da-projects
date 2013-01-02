using System.Linq;
using System.Security.Principal;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface IPaymentBatchRepository
    {
        IQueryable<PaymentBatch> PaymentBatches { get; }
        IQueryable<PaymentBatch> PaymentBatchesForUser(string identity, bool sentOnly);

        IQueryable<PublisherPayment> PublisherPayments { get; }

        void SetAccountingStatus(int[] itemIds, int accountingStatus);

        IQueryable<PublisherNote> PublisherNotes { get; }
        IQueryable<PublisherNote> PublisherNotesForPublisher(string pubName);
        void AddPublisherNote(string pubName, string note, string identity);
    }
}
