using System.Linq;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Concrete
{
    public class PaymentBatchRepository : IPaymentBatchRepository
    {
        EomEntities context;

        public PaymentBatchRepository(EomEntities context)
        {
            this.context = context;
        }

        public IQueryable<PaymentBatch> PaymentBatches
        {
            get { return context.PaymentBatches; }
        }

        // if identity==null, will return everyone's batches
        public IQueryable<PaymentBatch> PaymentBatchesForUser(string identity, bool sentOnly)
        {
            var batches = context.PaymentBatches as IQueryable<PaymentBatch>;
            if (identity != null)
                batches = batches.Where(b => b.approver_identity == identity);
            if (sentOnly)
                batches = batches.Where(pb => pb.payment_batch_state_id == PaymentBatchState.Sent);
            return batches;
        }

        public IQueryable<PublisherPayment> PublisherPayments
        {
            get { return context.PublisherPayments; }
        }

        public IQueryable<PublisherPayment> PublisherPaymentsForUser(string identity, bool sentOnly)
        {
            var batches = PaymentBatchesForUser(identity, sentOnly);
            var batchIds = batches.Select(b => b.id).ToList();

            return context.PublisherPayments.Where(p => p.PaymentBatchId != null && batchIds.Contains(p.PaymentBatchId.Value));
        }

        // These are separate payouts (by campaign) used in publisher reports...
        public IQueryable<PublisherPayout> PublisherPayouts
        {
            get { return context.PublisherPayouts; }
        }

        // --- Actions ---

        public void SetAccountingStatus(int[] itemIds, int accountingStatus)
        {
            var items = context.Items.Where(item => itemIds.Contains(item.id));
            foreach (var item in items)
            {
                item.item_accounting_status_id = accountingStatus;
            }
            context.SaveChanges();
        }

        public void SetPaymentBatchId(int[] itemIds, int paymentBatchId)
        {
            var items = context.Items.Where(item => itemIds.Contains(item.id));
            foreach (var item in items)
            {
                item.payment_batch_id = paymentBatchId;
            }
            context.SaveChanges();
        }

        // --- Notes ---

        public IQueryable<PubNote> PubNotes
        {
            get { return context.PubNotes; }
        }

        public IQueryable<PubNote> PubNotesForPublisher(string pubName)
        {
            return context.PubNotes.Where(n => n.publisher_name == pubName);
        }

        public void AddPubNote(string pubName, string note, string identity)
        {
            var pubNote = new PubNote()
            {
                note = note,
                added_by_system_user = identity,
                publisher_name = pubName
            };
            context.PubNotes.AddObject(pubNote);
            context.SaveChanges();
        }
    }
}
