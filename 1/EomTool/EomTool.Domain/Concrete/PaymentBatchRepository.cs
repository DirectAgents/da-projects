using System;
using System.Linq;
using System.Security.Principal;
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

        public IQueryable<PaymentBatch> PaymentBatchesForUser(string identity, bool sentOnly)
        {
            var batches = context.PaymentBatches.Where(b => b.approver_identity == identity);
            if (sentOnly)
                batches = batches.Where(pb => pb.payment_batch_state_id == PaymentBatchState.Sent);
            return batches;
        }

        public IQueryable<PublisherPayment> PublisherPayments
        {
            get { return context.PublisherPayments; }
        }


        public void SetAccountingStatus(int[] itemIds, int accountingStatus)
        {
            var items = context.Items.Where(item => itemIds.Contains(item.id));
            foreach (var item in items)
            {
                item.item_accounting_status_id = accountingStatus;
            }
            context.SaveChanges();
        }

        // --- Notes ---

        public IQueryable<PublisherNote> PublisherNotes
        {
            get { return context.PublisherNotes; }
        }

        public IQueryable<PublisherNote> PublisherNotesForPublisher(string pubName)
        {
            return context.PublisherNotes.Where(n => n.publisher_name == pubName);
        }

        public void AddPublisherNote(string pubName, string note, string identity)
        {
            var pubNote = new PublisherNote()
            {
                note = note,
                added_by_system_user = identity,
                publisher_name = pubName
            };
            context.PublisherNotes.AddObject(pubNote);
            context.SaveChanges();
        }
    }
}
