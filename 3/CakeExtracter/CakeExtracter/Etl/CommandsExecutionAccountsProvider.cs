using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl
{
    /// <summary>
    /// Execution Accounts provider for commands execution.
    /// </summary>
    internal class CommandsExecutionAccountsProvider
    {
        private PlatformAccountRepository accountsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsExecutionAccountsProvider"/> class.
        /// </summary>
        /// <param name="accountsRepository">The accounts repository.</param>
        public CommandsExecutionAccountsProvider(PlatformAccountRepository accountsRepository)
        {
        }

        /// <summary>
        /// Gets the accounts to process by platform code or id if it was specified.
        /// </summary>
        /// <param name="platformCode">The platform code.</param>
        /// <param name="accountIdFromConfig">The account identifier from configuration.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public List<ExtAccount> GetAccountsToProcess(string platformCode, int? accountIdFromConfig)
        {
            if (accountIdFromConfig.HasValue)
            {
                var account = accountsRepository.GetAccount(accountIdFromConfig.Value);
                if (account != null)
                {
                    return new List<ExtAccount> { account };
                }
                else
                {
                    throw new Exception(string.Format("Account with id {0} was not found", accountIdFromConfig));
                }
            }
            else
            {
                return accountsRepository.GetAccountsByPlatformCode(platformCode).ToList();
            }
        }
    }
}
