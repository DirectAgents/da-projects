using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.DSP.Configuration
{
    internal class AmazonDspAccountsProvider
    {
        private PlatformAccountRepository accountsRepository;

        public AmazonDspAccountsProvider()
        {
            accountsRepository = new PlatformAccountRepository();
        }

        public List<ExtAccount> GetAccountsToProcess(int? accountIdFromConfig)
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
                return accountsRepository.GetAccountsByPlatformCode(Platform.Code_DspAmazon).ToList();
            }
        }
    }
}
