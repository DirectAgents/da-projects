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
    }
}
