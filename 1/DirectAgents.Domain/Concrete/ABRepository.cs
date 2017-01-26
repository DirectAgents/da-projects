using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.AB;

namespace DirectAgents.Domain.Concrete
{
    public class ABRepository : IABRepository, IDisposable
    {
        private ABContext context;

        public ABRepository(ABContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        // ---

        public ABClient Client(int id)
        {
            return context.ABClients.Find(id);
        }

        public IQueryable<ABClient> Clients()
        {
            return context.ABClients;
        }

        public bool AddClient(ABClient client)
        {
            if (context.ABClients.Any(c => c.Id == client.Id))
                return false;
            context.ABClients.Add(client);
            context.SaveChanges();
            return true;
        }
        public void AddClients(IEnumerable<ABClient> clients)
        {
            context.ABClients.AddRange(clients);
            context.SaveChanges();
        }

        public bool SaveClient(ABClient client)
        {
            if (context.ABClients.Any(c => c.Id == client.Id))
            {
                var entry = context.Entry(client);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public ClientBudget ClientBudget(int clientId, DateTime date)
        {
            return context.ClientBudgets.Find(clientId, date);
        }

        public ClientPayment ClientPayment(int id)
        {
            return context.ClientPayments.Find(id);
        }
        public ClientPaymentBit ClientPaymentBit(int id)
        {
            return context.ClientPaymentBits.Find(id);
        }

        public IQueryable<Job> ActiveJobs(int clientId, DateTime? startDate, DateTime? endDate)
        {
            var jobs = context.Jobs.Where(j => j.ClientId == clientId);
            if (!startDate.HasValue && !endDate.HasValue)
            {   // no daterange specified
                jobs = jobs.Where(j => j.ExtraItems.Any());
            }
            else
            {   // find the jobs that were active within the daterange
                var extraItems = jobs.SelectMany(j => j.ExtraItems);
                if (startDate.HasValue)
                    extraItems = extraItems.Where(x => x.Date >= startDate.Value);
                if (endDate.HasValue)
                    extraItems = extraItems.Where(x => x.Date <= endDate.Value);
                var jobIds = extraItems.Select(x => x.JobId).Distinct().ToArray();
                jobs = jobs.Where(j => jobIds.Contains(j.Id));
            }
            return jobs;
        }

        public IQueryable<ABExtraItem> ExtraItems(DateTime? startDate, DateTime? endDate, int? jobId = null)
        {
            var extraItems = context.ABExtraItems.AsQueryable();
            if (startDate.HasValue)
                extraItems = extraItems.Where(x => x.Date >= startDate.Value);
            if (endDate.HasValue)
                extraItems = extraItems.Where(x => x.Date <= endDate.Value);
            if (jobId.HasValue)
                extraItems = extraItems.Where(x => x.JobId == jobId.Value);
            return extraItems;
        }

        // --- UNUSED: ClientAccount and AccountBudget ---

        public ClientAccount ClientAccount(int id)
        {
            return context.ClientAccounts.Find(id);
        }

        public bool SaveClientAccount(ClientAccount clientAccount)
        {
            if (context.ClientAccounts.Any(ca => ca.Id == clientAccount.Id))
            {
                var entry = context.Entry(clientAccount);
                entry.State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public AccountBudget AccountBudget(int clientAccountId, DateTime date)
        {
            return context.AccountBudgets.Find(clientAccountId, date);
        }

        // ---

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
