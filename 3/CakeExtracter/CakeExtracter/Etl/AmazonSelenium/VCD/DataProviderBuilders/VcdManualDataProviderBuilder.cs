using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using SeleniumDataBrowser.Helpers;
using SeleniumDataBrowser.VCD.DataProviders;
using SeleniumDataBrowser.VCD.Helpers;
using SeleniumDataBrowser.VCD.Helpers.ReportDownloading;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.DataProviderBuilders
{
    public class VcdManualDataProviderBuilder : IVcdDataProviderBuilder
    {
        private readonly int? accountId;
        private readonly Dictionary<ExtAccount, IEnumerable<DirectoryInfo>> daysFoldersByAccounts = new Dictionary<ExtAccount, IEnumerable<DirectoryInfo>>();
        private VcdManualDataProvider vcdDataProvider;

        public VcdManualDataProviderBuilder(int? accountId)
        {
            this.accountId = accountId;
        }

        public IVcdDataProvider BuildDataProvider(SeleniumLogger loggerWithoutAccountId)
        {
            vcdDataProvider = new VcdManualDataProvider(loggerWithoutAccountId);
            return vcdDataProvider;
        }

        public void InitializeReportDownloader(ExtAccount currentAccount, SeleniumLogger loggerWithAccountId)
        {
            var daysFoldersForCurrentAccount = daysFoldersByAccounts[currentAccount];
            var reportDownloader = new VcdFolderReportDownloader();
            vcdDataProvider.SetReportDownloaderCurrentForDataProvider(reportDownloader, daysFoldersForCurrentAccount);
        }

        public List<ExtAccount> GetAccounts()
        {
            var dbAccounts = GetDbAccounts();
            SetDaysFoldersByAccounts(dbAccounts);
            var relatedAccounts = new List<ExtAccount>(daysFoldersByAccounts.Keys);
            return relatedAccounts;
        }

        private IEnumerable<ExtAccount> GetDbAccounts()
        {
            var repository = new PlatformAccountRepository();
            if (!accountId.HasValue)
            {
                var accounts = repository.GetAccountsByPlatformCode(Platform.Code_AraAmazon);
                return accounts.ToList();
            }
            var account = repository.GetAccount(accountId.Value);
            return new List<ExtAccount> { account };
        }

        private void SetDaysFoldersByAccounts(IEnumerable<ExtAccount> dbAccounts)
        {
            const string reportsFolderName = "VcdReports";

            var accountFolders = VcdReportFolderHelper.GetSubdirectories(reportsFolderName);
            accountFolders.ForEach(accountFolder =>
            {
                var account = dbAccounts.FirstOrDefault(
                    a => IsAccountContainFolderName(accountFolder.Name, a.Name) ||
                         IsAccountContainFolderName(accountFolder.Name, a.Id.ToString()));
                if (account != null)
                {
                    var daysFolders = VcdReportFolderHelper.GetSubdirectories(accountFolder);
                    daysFoldersByAccounts.Add(account, daysFolders);
                }
            });
        }

        private bool IsAccountContainFolderName(string accountFolderName, string accountInfo)
        {
            return CultureInfo.InvariantCulture.CompareInfo.IndexOf(accountFolderName, accountInfo, CompareOptions.IgnoreCase) >= 0;
        }
    }
}