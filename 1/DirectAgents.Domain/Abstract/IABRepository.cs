using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Domain.Abstract
{
    public interface IABRepository : IDisposable
    {
        void SaveChanges();

        ABClient Client(int id);
        IQueryable<ABClient> Clients();
        bool AddClient(ABClient client);
        void AddClients(IEnumerable<ABClient> clients);
        bool SaveClient(ABClient client);

        ClientBudget ClientBudget(int clientId, DateTime date);

        ClientPayment ClientPayment(int id);
        bool SaveClientPayment(ClientPayment payment);
        void FillExtended(ClientPayment payment);

        ClientPaymentBit ClientPaymentBit(int id);
        IQueryable<ClientPaymentBit> ClientPaymentBits(int? paymentId = null);

        IQueryable<Job> Jobs(int? clientId = null);
        IQueryable<Job> ActiveJobs(int clientId, DateTime? startDate, DateTime? endDate);

        IQueryable<ABExtraItem> ExtraItems(DateTime? startDate, DateTime? endDate, int? jobId = null);

        ClientAccount ClientAccount(int id);
        bool SaveClientAccount(ClientAccount clientAccount);
        AccountBudget AccountBudget(int clientAccountId, DateTime date);
    }
}
