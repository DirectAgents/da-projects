using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DirectAgents.Domain.Concrete
{
    /// <inheritdoc />
    /// <summary>
    /// The class represents methods for retrieving accounts from a database that is available from the ClientPortalProgContext
    /// </summary>
    public class PlatformAccountRepository : IPlatformAccountRepository
    {
        /// <inheritdoc />
        public ExtAccount GetAccount(int accountId)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.ExtAccounts.Find(accountId);
            }
        }

        /// <inheritdoc />
        public ExtAccount GetAccountByExternalId(string externalId, string platformCode)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.ExtAccounts.FirstOrDefault(x =>
                    x.Platform.Code == platformCode && x.ExternalId == externalId);
            }
        }

        /// <inheritdoc />
        public ExtAccount GetAccountWithColumnMapping(int accountId)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.ExtAccounts.Include(x => x.Platform.PlatColMapping).FirstOrDefault(x => x.Id == accountId);
            }
        }

        /// <inheritdoc />
        public IEnumerable<ExtAccount> GetAccountsWithIds(List<int> accountIds)
        {
            using (var db = new ClientPortalProgContext())
            {
                return db.ExtAccounts.Where(account => accountIds.Contains(account.Id)).ToList();
            }
        }

        /// <inheritdoc />
        public IEnumerable<ExtAccount> GetAccountsWithFilledExternalIdByPlatformCode(string platformCode, bool disabledOnly)
        {
            return GetAccountsWithFilledExternalIdByPlatformCode(platformCode, disabledOnly, false);
        }

        /// <inheritdoc />
        public IEnumerable<ExtAccount> GetAccountsWithFilledExternalIdByPlatformCode(string platformCode, bool disabledOnly, bool includeColumnMapping)
        {
            var accounts = GetAccountsByPlatformCode(platformCode, disabledOnly, includeColumnMapping);
            var accountsWithExternalId = accounts.Where(x => !string.IsNullOrEmpty(x.ExternalId));
            return accountsWithExternalId.ToList();
        }

        /// <inheritdoc />
        public IEnumerable<ExtAccount> GetAccountsByPlatformCode(string platformCode)
        {
            return GetAccountsByPlatformCode(platformCode, false, false);
        }

        private IEnumerable<ExtAccount> GetAccountsByPlatformCode(string platformCode, bool disabledOnly, bool includeColumnMapping)
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Where(x => x.Platform.Code == platformCode);
                var accountsWithStatus = accounts.Where(x => x.Disabled == disabledOnly);
                if (includeColumnMapping)
                {
                    accountsWithStatus = accountsWithStatus.Include(x => x.Platform.PlatColMapping);
                }
                return accountsWithStatus.ToList();
            }
        }
    }
}
