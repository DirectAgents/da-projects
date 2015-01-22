using System;
using System.Data;
using System.Linq;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.AdRoll;
using ClientPortal.Data.Entities.TD.DBM;

namespace ClientPortal.Data.Services
{
    public partial class TDRepository : ITDRepository, IDisposable
    {
        private TDContext context;

        public TDRepository(TDContext context)
        {
            this.context = context;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        // ---

        public IQueryable<AdRollProfile> AdRollProfiles()
        {
            var profiles = context.AdRollProfiles;
            return profiles;
        }

        public IQueryable<InsertionOrder> InsertionOrders()
        {
            var insertionOrders = context.InsertionOrders;
            return insertionOrders;
        }

        public InsertionOrder GetInsertionOrder(int insertionOrderID)
        {
            var insertionOrder = context.InsertionOrders.Find(insertionOrderID);
            return insertionOrder;
        }

        public bool CreateAccountForInsertionOrder(int insertionOrderID)
        {
            var insertionOrder = context.InsertionOrders.Find(insertionOrderID);
            if (insertionOrder == null)
                return false;

            var tdAccount = NewTradingDeskAccount();
            insertionOrder.TradingDeskAccount = tdAccount;
            context.SaveChanges();
            return true;
        }

        private TradingDeskAccount NewTradingDeskAccount()
        {
            return new TradingDeskAccount();
        }

        public IQueryable<TradingDeskAccount> TradingDeskAccounts()
        {
            var tdAccounts = context.TradingDeskAccounts;
            return tdAccounts;
        }

        public TradingDeskAccount GetTradingDeskAccount(int tradingDeskAccountId)
        {
            var tdAccount = context.TradingDeskAccounts.Find(tradingDeskAccountId);
            return tdAccount;
        }

        public void SaveTradingDeskAccount(TradingDeskAccount tdAccount)
        {
            var entry = context.Entry(tdAccount);
            entry.State = EntityState.Modified;
            context.SaveChanges();
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
