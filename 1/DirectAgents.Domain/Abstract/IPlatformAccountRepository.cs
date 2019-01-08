using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.Abstract
{
    public interface IPlatformAccountRepository
    {
        ExtAccount GetAccount(int accountId);
        IEnumerable<ExtAccount> GetNotDisabledAccounts(string platformCode);
        IEnumerable<ExtAccount> GetAccounts(string platformCode, bool disabledOnly);
    }
}
