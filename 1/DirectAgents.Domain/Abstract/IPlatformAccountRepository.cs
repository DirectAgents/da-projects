using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.Abstract
{
    public interface IPlatformAccountRepository
    {
        ExtAccount GetAccount(int accountId);

        IEnumerable<ExtAccount> GetAccountsWithFilledExternalIdByPlatformCode(string platformCode, bool disabledOnly = false);

        IEnumerable<ExtAccount> GetAccountsByPlatformCode(string platformCode, bool disabledOnly = false);
    }
}
