using Accounting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accounting.Domain.Concrete
{
    public class AccountingRepository : IDisposable
    {
        readonly AccountingContext context;
        readonly bool disposeContext;

        public AccountingRepository(AccountingContext context, bool disposeContext = false)
        {
            this.context = context;
            this.disposeContext = disposeContext;
        }

        public CompanyFile CompanyFileByName(string companyName)
        {
            var result = this.context.CompanyFiles.FirstOrDefault(c => c.CompanyName == companyName);
            return result;
        }

        public void Dispose()
        {
            if (disposeContext)
            {
                if (this.context != null)
                {
                    this.context.Dispose();
                }
            }
        }
    }
}
