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

        public IQueryable<PaymentBatch> PaymentBatchesForUser(IPrincipal user)
        {
            return context.PaymentBatches.Where(b => b.approver_identity == user.Identity.Name);
        }

        public void Approve(int id, string windows_identity)
        {
            var pbatch = context.PaymentBatches.Where(b => b.id == id).SingleOrDefault();
            if (pbatch != null)
            {
                var from_state = pbatch.payment_batch_approval_state_id;

                int to_state = PaymentBatch.Approved;
                pbatch.payment_batch_approval_state_id = to_state;

                var pbUpdate = new PaymentBatchUpdate()
                {
                    payment_batch_id = id,
                    windows_identity = windows_identity,
                    from_payment_batch_approval_state_id = from_state,
                    to_payment_batch_approval_state_id = to_state,
                    //note = "",
                    timestamp = DateTime.Now
                };
                context.PaymentBatchUpdates.AddObject(pbUpdate);

                context.SaveChanges();
            }
        }
    }
}
