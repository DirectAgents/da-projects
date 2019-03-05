using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.Abstract
{
    /// <summary>
    /// The interface represents methods for retrieving accounts from the repository
    /// </summary>
    public interface IPlatformAccountRepository
    {
        /// <summary>
        /// The method returns an account from repository by ID
        /// </summary>
        /// <param name="accountId">Account ID to retrieve</param>
        /// <returns>Account with filled basic properties</returns>
        ExtAccount GetAccount(int accountId);

        /// <summary>
        /// The method returns an account from repository by ID
        /// </summary>
        /// <param name="accountId">Account ID to retrieve</param>
        /// <returns>Account with filled basic properties and ColMapping property</returns>
        ExtAccount GetAccountWithColumnMapping(int accountId);

        IEnumerable<ExtAccount> GetAccountsWithIds(List<int> accountIds);

        /// <summary>
        /// The method returns platform accounts with non-empty external IDs
        /// </summary>
        /// <param name="platformCode">Code of target platform</param>
        /// <param name="disabledOnly"></param>
        /// <returns>Accounts with filled basic properties</returns>
        IEnumerable<ExtAccount> GetAccountsWithFilledExternalIdByPlatformCode(string platformCode, bool disabledOnly);

        /// <summary>
        /// The method returns platform accounts with non-empty external IDs
        /// </summary>
        /// <param name="platformCode">Code of target platform</param>
        /// <param name="disabledOnly"></param>
        /// <param name="includeColumnMapping"></param>
        /// <returns>Accounts with filled basic properties and ColMapping property</returns>
        IEnumerable<ExtAccount> GetAccountsWithFilledExternalIdByPlatformCode(string platformCode, bool disabledOnly, bool includeColumnMapping);

        /// <summary>
        /// The method returns platform accounts
        /// </summary>
        /// <param name="platformCode">Code of target platform</param>
        /// <returns>Accounts with filled basic properties</returns>
        IEnumerable<ExtAccount> GetAccountsByPlatformCode(string platformCode);
    }
}
