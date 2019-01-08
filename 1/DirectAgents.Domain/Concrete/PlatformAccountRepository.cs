using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DirectAgents.Domain.Concrete
{
    public class PlatformAccountRepository : IPlatformAccountRepository
    {
        public ExtAccount GetAccount(int accountId)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.ExtAccounts.Find(accountId);
            }
        }

        public IEnumerable<ExtAccount> GetNotDisabledAccounts(string platformCode)
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = GetAccounts(db, platformCode, false);
                return accounts.ToList();
            }
        }

        public IEnumerable<ExtAccount> GetAccounts(string platformCode, bool disabledOnly)
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = GetAccounts(db, platformCode, disabledOnly);
                return accounts.ToList();
            }
        }

        private IQueryable<ExtAccount> GetAccounts(ClientPortalProgContext db, string platformCode, bool disabledOnly)
        {
            var accounts = db.ExtAccounts.Where(x => x.Platform.Code == platformCode);
            var accountsWithExternalId = accounts.Where(x => x.ExternalId != null && x.ExternalId != string.Empty);
            var accountsWithStatus = accountsWithExternalId.Where(x => x.Disabled == disabledOnly);
            return accountsWithStatus;
        }
    }
}
