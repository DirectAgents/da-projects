using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPortal.Data.Services
{
    public class TDRepository : ITDRepository, IDisposable
    {
        private TDContext context;

        public TDRepository(TDContext context)
        {
            this.context = context;
        }

        //public void Test()
        //{
        //    var x = context.InsertionOrders.ToList();
        //}

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
