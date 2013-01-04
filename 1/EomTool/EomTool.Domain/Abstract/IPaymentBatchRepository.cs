﻿using System.Linq;
using EomTool.Domain.Entities;

namespace EomTool.Domain.Abstract
{
    public interface IPaymentBatchRepository
    {
        IQueryable<PaymentBatch> PaymentBatches { get; }
        IQueryable<PaymentBatch> PaymentBatchesForUser(string identity, bool sentOnly);

        IQueryable<PublisherPayment> PublisherPayments { get; }

        void SetAccountingStatus(int[] itemIds, int accountingStatus);
    }
}
