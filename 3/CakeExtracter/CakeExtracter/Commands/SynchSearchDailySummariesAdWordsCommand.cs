using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.SearchMarketing.Extracters;
using CakeExtracter.Etl.SearchMarketing.Loaders;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchSearchDailySummariesAdWordsCommand : ConsoleCommand
    {
        public int? SearchProfileId { get; set; }
        public string ClientId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IncludeClickType { get; set; }
        public string GetClickAssistConvStats { get; set; }

        public override void ResetProperties()
        {
            SearchProfileId = null;
            ClientId = null;
            StartDate = null;
            EndDate = null;
            IncludeClickType = false;
            GetClickAssistConvStats = null;
        }

        public SynchSearchDailySummariesAdWordsCommand()
        {
            IsCommand("synchSearchDailySummariesAdWords", "synch SearchDailySummaries for AdWords");
            HasOption<int>("p|searchProfileId=", "SearchProfile Id (default = all)", c => SearchProfileId = c);
            HasOption<string>("v|clientId=", "Client Id", c => ClientId = c);
            HasOption<DateTime>("s|startDate=", "Start Date (default is 62 days ago)", c => StartDate = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            HasOption<bool>("b|includeClickType=", "Include ClickType (default is false)", c => IncludeClickType = c);
            HasOption<string>("g|getClickAssistConvStats=", "Get click-assisted-conversion stats ('yes' or 'both', default = no)", c => GetClickAssistConvStats = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            var defaultStart = DateTime.Today.AddDays(-62);
            var yesterday = DateTime.Today.AddDays(-1);
            var dateRange = new DateRange(StartDate ?? defaultStart, EndDate ?? yesterday);

            foreach (var searchAccount in GetSearchAccounts())
            {
                if (GetClickAssistConvStats != "yes")
                {
                    var extracter = new AdWordsApiExtracter(searchAccount.AccountCode, dateRange, IncludeClickType, false);
                    var loader = new AdWordsApiLoader(searchAccount.SearchAccountId, searchAccount.UseConvertedClicks, IncludeClickType, false);
                    var extracterThread = extracter.Start();
                    var loaderThread = loader.Start(extracter);
                    extracterThread.Join();
                    loaderThread.Join();
                }
                if (GetClickAssistConvStats == "yes" || GetClickAssistConvStats == "both")
                {
                    var extracter = new AdWordsApiExtracter(searchAccount.AccountCode, dateRange, IncludeClickType, true);
                    var loader = new AdWordsApiLoader(searchAccount.SearchAccountId, searchAccount.UseConvertedClicks, IncludeClickType, true);
                    var extracterThread = extracter.Start();
                    var loaderThread = loader.Start(extracter);
                    extracterThread.Join();
                    loaderThread.Join();
                }
            }
            return 0;
        }

        public IEnumerable<SearchAccount> GetSearchAccounts()
        {
            var searchAccounts = new List<SearchAccount>();

            using (var db = new ClientPortalContext())
            {
                if (this.ClientId == null) // ClientId not specified
                {
                    // Start with all google SearchAccounts with an account code
                    var searchAccountsQ = db.SearchAccounts.Where(sa => sa.Channel == "Google" && !String.IsNullOrEmpty(sa.AccountCode));
                    if (this.SearchProfileId.HasValue)
                        searchAccountsQ = searchAccountsQ.Where(sa => sa.SearchProfileId == this.SearchProfileId.Value); // ...for the specified SearchProfile
                    else
                        searchAccountsQ = searchAccountsQ.Where(sa => sa.SearchProfileId.HasValue); // ...that are children of a SearchProfile

                    searchAccounts = searchAccountsQ.ToList();
                }
                else // ClientId specified
                {
                    var searchAccount = db.SearchAccounts.SingleOrDefault(sa => sa.AccountCode == ClientId && sa.Channel == "Google");
                    if (searchAccount != null)
                    {
                        if (SearchProfileId.HasValue && searchAccount.SearchProfileId != SearchProfileId.Value)
                            Logger.Warn("SearchProfileId does not match that of SearchAccount specified by ClientId");

                        searchAccounts.Add(searchAccount);
                    }
                    else // didn't find a matching SearchAccount; see about creating a new one
                    {
                        if (SearchProfileId.HasValue)
                        {
                            var searchProfile = db.SearchProfiles.Find(SearchProfileId.Value);
                            if (searchProfile != null)
                            {
                                searchAccount = new SearchAccount()
                                {
                                    SearchProfile = searchProfile,
                                    Channel = "Google",
                                    AccountCode = ClientId
                                    // to fill in later: Name, ExternalId
                                };
                                db.SearchAccounts.Add(searchAccount);
                                db.SaveChanges();
                                searchAccounts.Add(searchAccount);
                            }
                            else
                            {
                                Logger.Info("SearchAccount with AccountCode {0} not found and SearchProfileId {1} not found", ClientId, SearchProfileId);
                            }
                        }
                        else
                        {
                            Logger.Info("SearchAccount with AccountCode {0} not found and no SearchProfileId specified", ClientId);
                        }
                    }
                }
                foreach (var searchAccount in searchAccounts)
                {
                    searchAccount.UseConvertedClicks = searchAccount.SearchProfile.UseConvertedClicks;
                    // assign this b/c we're going to detach from the db and we'll need it later
                }
            }
            return searchAccounts;
        }

    }
}
