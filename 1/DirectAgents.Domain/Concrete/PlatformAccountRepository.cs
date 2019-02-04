using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
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

        public IEnumerable<ExtAccount> GetAccountsWithFilledExternalIdByPlatformCode(string platformCode, bool disabledOnly)
        {
            var accounts = GetAccountsByPlatformCode(platformCode, disabledOnly);
            var accountsWithExternalId = accounts.Where(x => x.ExternalId != null && x.ExternalId != string.Empty);
            return accountsWithExternalId.ToList();
        }

        public IEnumerable<ExtAccount> GetAccountsByPlatformCode(string platformCode, bool disabledOnly = false)
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Where(x => x.Platform.Code == platformCode);
                var accountsWithStatus = accounts.Where(x => x.Disabled == disabledOnly);
                return accountsWithStatus.ToList();
            }
        }
    }
}
