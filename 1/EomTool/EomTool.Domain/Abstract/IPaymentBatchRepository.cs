﻿using System.Linq;
using System.Security.Principal;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface IPaymentBatchRepository
    {
        IQueryable<PaymentBatch> PaymentBatches { get; }
        IQueryable<PaymentBatch> PaymentBatchesForUser(IPrincipal user);
        void Approve(int id, string windows_identity);
    }
}
