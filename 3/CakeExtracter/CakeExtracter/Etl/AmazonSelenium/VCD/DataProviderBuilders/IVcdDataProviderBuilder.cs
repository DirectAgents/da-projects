using System.Collections.Generic;
using DirectAgents.Domain.Entities.CPProg;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.VCD.DataProviders;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.DataProviderBuilders
{
    public interface IVcdDataProviderBuilder
    {
        IVcdDataProvider BuildDataProvider(SeleniumLogger loggerWithoutAccountId);

        void InitializeReportDownloader(ExtAccount currentAccount, SeleniumLogger loggerWithAccountId);

        List<ExtAccount> GetAccounts();
    }
}