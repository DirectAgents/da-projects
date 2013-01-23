using System;
using System.Data;
using System.IO;
using AccountingBackupWeb.Models.AccountingBackup;
using DAgents.Common;
using Microsoft.Practices.Unity;
using QuickBooksLibrary;

namespace QuickBooksService
{
    public class MyDatabaseLogger : ILogger
    {
        public void Log(string message)
        {
            AccountingBackupWeb.Models.AccountingBackup.Log.Write(message, eEntryTypes.Default);
        }

        public void LogError(string message)
        {
            AccountingBackupWeb.Models.AccountingBackup.Log.Write(message, eEntryTypes.Error);
        }
    }

    public class Boot
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0) // launch Gui
            {
                Gui.Program.Main();
            }
            else
            {
                Main(args, new ConsoleLogger());
            }
        }

        [STAThread]
        public static void Main(string[] args, ILogger logger)
        {
            try
            {
                var program = new Program(logger);
                program.Run(args);
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("stack trace?");
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.KeyChar == 'y')
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine(ex.StackTrace);
                    }
                }
                catch
                {
                    // ignore error
                }
            }
        }
    }

    /// <summary>
    /// Loads QB data into AB database.
    /// 
    /// Designed to be run after EOM data is loaded.
    /// 
    /// Loads QB entities from XML input file:
    ///  - Customers
    ///  - Invoices
    ///  - ReceivedPayments
    /// </summary>
    class Program : ConsoleProgramBase
    {
        string _usage = "usage: QuickBooksService.exe (load|extract) (us|intl) ([Customer][,Invoice][,Payment][,All])";

        public Program() :
            this(null)
        {
        }

        public Program(ILogger logger) :
            base(logger)
        {
        }

        public override void Configure()
        {
            var myContainer = new MyContainer(this.UnityContainer, this.Logger);
            UnityContainer = myContainer.Container;
        }

        public override IUnityContainer UnityContainer { get; set; }

        // TODO: check preconditions, such as companies not being loaded by the other services having been run...
        public void Run(string[] args)
        {
            if (args.Length < 1)
                throw new Exception(_usage);

            if (args[0] == "loadstartingbalances")
            {
                LoadStartingBalances();
            }
            else if (args[0] == "loadcreditlimits")
            {
                LoadCreditLimits(args);
            }
            else if (args[0] == "loadqbschema")
            {
                LoadQuickBooksSchema(args);
            }
            else if (args[0] == "loadquickbooksrecords")
            {
                LoadQuickBooksRecords(args);
            }
            else if (args[0] == "renameadverts")
            {
                RenameAdvertisers();
            }
            else if (args[0] == "query")
            {
                Query(args);
            }
            else
                ETL(args);
        }

        void LoadCreditLimits(string[] args)
        {
            UnityContainer.BuildUp(this);

            UnityContainer
                .RegisterType<ILogger, ConsoleLogger>()
                .RegisterInstance<AccountingBackupWeb.Models.Staging.StagingEntities.IFactory>(
                    new AccountingBackupWeb.Models.Staging.StagingEntitesFactory(
                        (string)Config.DatabaseServer,
                        (string)Config.StagingDatabaseName,
                        (string)Config.DatabaseProvider))
                .RegisterType<CreditLimitLoader.IConvertCreditLimitTextToCreditLimitAmount, ConvertCreditLimitTextToCreditLimitAmount1>("a")
                .RegisterType<CreditLimitLoader.IConvertCreditLimitTextToCreditLimitAmount, ConvertCreditLimitTextToCreditLimitAmount2>("b")
                .RegisterInstance<AccountingBackupEntities.IFactory>(
                        new AccountingBackupEntitiesFactory(
                            (string)Config.DatabaseServer,
                            (string)Config.DatabaseName,
                            (string)Config.DatabaseProvider)
                        )
            ;

            var creditLimitLoader = UnityContainer.Resolve<CreditLimitLoader>();

            creditLimitLoader.Execute();
        }

        void LoadQuickBooksRecords(string[] args)
        {
            var company = Company.ByName(args[1], true);

            UnityContainer.BuildUp(this);

            UnityContainer
                .RegisterInstance<Company>(company)
            ;

            UnityContainer.Resolve<QuickBooksRecordLoader>().Execute();
        }

        void LoadQuickBooksSchema(string[] args)
        {
            var company = Company.ByName(args[1], true);

            UnityContainer.BuildUp(this);

            UnityContainer
                .RegisterInstance<Company>(company)
            ;

            var action = UnityContainer.Resolve<QuickBooksSchemaLoader>();

            action.Execute();
        }

        void Query(string companyName, TextWriter outputWriter, string queryText)
        {
            var qbQuery = UnityContainer.Resolve<QuickBooksCompanyFileQuery>();

            var resultTable = new DataTable("QuickBooksQueryResult");

            qbQuery.Fill(resultTable, queryText);

            if (!string.IsNullOrWhiteSpace(qbQuery.Error))
            {
                Console.WriteLine(qbQuery.Error);
            }
            else
            {
                resultTable.WriteXml(outputWriter);
            }
        }

        void ETL(string[] args)
        {
            if (args.Length != 3)
                throw new Exception(_usage);

            UnityContainer.BuildUp(this);

            IProgramAction programAction = null;

            UnityContainer
                .RegisterInstance<Company>(Company.ByName(args[1], true))
                .RegisterInstance<eTargets>((eTargets)Enum.Parse(typeof(eTargets), args[2]))
            ;

            if (args[0] == "load")
            {
                Logger.Log("Loading data..");
                programAction = ConfigureLoad(args, programAction);
            }
            else if (args[0] == "extract")
            {
                Logger.Log("Extracting data..");
                programAction = ConfigureExtract(programAction);
            }
            else
            {
                throw new Exception("no action");
            }

            if (programAction != null)
            {
                programAction.Execute();
            }
            else
            {
                throw new Exception("program action not set");
            }
        }

        IProgramAction ConfigureExtract(IProgramAction programAction)
        {
            UnityContainer
                .RegisterType<Extracter>(
                    new InjectionConstructor(
                        typeof(Company), typeof(QuickBooksCompanyFileQuery), (DateTime)Config.FromDate, typeof(eTargets)
                    )
                )
            ;

            programAction = UnityContainer.Resolve<Extracter>();

            return programAction;
        }

        IProgramAction ConfigureLoad(string[] args, IProgramAction programAction)
        {
            UnityContainer
                    .RegisterType<ILoader, Loader_Customer>("customer")
                    .RegisterType<ILoader, Loader_Invoice>("invoice")
                    .RegisterType<ILoader, Loader_Payment>("payment")
                    .RegisterInstance<AccountingBackupEntities.IFactory>(
                        new AccountingBackupEntitiesFactory(
                            (string)Config.DatabaseServer,
                            (string)Config.DatabaseName,
                            (string)Config.DatabaseProvider)
                        )
            ;

            if (args[1] == "us")
            {
                UnityContainer
                    .RegisterType<LoaderInputReader>(
                        new InjectionConstructor(
                            (string)Config.ExtractFiles.US, typeof(Company), typeof(eTargets)
                        )
                    )
                ;
            }
            else if (args[1] == "intl")
            {
                UnityContainer
                    .RegisterType<LoaderInputReader>(
                        new InjectionConstructor(
                            (string)Config.ExtractFiles.Intl, typeof(Company), typeof(eTargets)
                        )
                    )
                ;
            }
            else
            {
                throw new Exception("invalid company");
            }

            programAction = UnityContainer.Resolve<LoaderDriver>();

            (programAction as LoaderDriver).LoadComplete += new EventHandler(Program_LoadComplete);

            return programAction;
        }

        void Query(string[] args)
        {
            string companyName = args[1];
            string queryFile = args[2];
            string outputFile = args[3];
            string queryText = File.ReadAllText(queryFile);
            var company = Company.ByName(companyName, true);
            UnityContainer.BuildUp(this);
            UnityContainer
                .RegisterInstance<Company>(company)
            ;
            var qbQuery = UnityContainer.Resolve<QuickBooksCompanyFileQuery>();
            var resultTable = new DataTable("QuickBooksQueryResult");
            qbQuery.Fill(resultTable, queryText);
            if (!string.IsNullOrWhiteSpace(qbQuery.Error))
            {
                Console.WriteLine(qbQuery.Error);
            }
            else
            {
                resultTable.WriteXml(outputFile);
            }
        }

        void RenameAdvertisers()
        {
            UnityContainer.BuildUp(this);
            UnityContainer.RegisterType<ILogger, ConsoleLogger>();
            var startingBalanceLoader = UnityContainer.Resolve<AdvertiserRenamer>();
            startingBalanceLoader.Execute();
        }

        void LoadStartingBalances()
        {
            UnityContainer.BuildUp(this);
            //UnityContainer.RegisterType<ILogger, MyDatabaseLogger>();
            var startingBalanceLoader = UnityContainer.Resolve<StartingBalanceLoader>();
            startingBalanceLoader.Execute();
        }

        void Program_LoadComplete(object sender, EventArgs e)
        {
            //UpdateAdvertiserCurrencies();
            //UpdateAdvertiserTerms();
        }

        static void UpdateAdvertiserCurrencies()
        {
            string sql =
@"
UPDATE Advertisers
SET Currency_Id=B.CurrencyId
FROM vAdvertiserCurrencies B INNER JOIN Advertisers A on ((B.CurrencyId <> A.Currency_Id) and (B.AdvertiserId=A.Id))
";
            ExecuteQuery(sql);
        }

        static void UpdateAdvertiserTerms()
        {
            string sql =
@"
UPDATE Advertisers
SET Term_Id=B.TermsId
FROM vAdvertiserTerms B INNER JOIN Advertisers A on ((B.TermsId <> A.Term_Id) and (B.AdvertiserId=A.Id))
";
            ExecuteQuery(sql);
        }

        static void ExecuteQuery(string sql)
        {
            using (var model = new AccountingBackupEntities())
            {
                Console.WriteLine(sql); // todo: logger

                model.ExecuteStoreCommand(sql);
            }
        }

        eTargets ParseTargets(string input)
        {
            eTargets targets = eTargets.None;
            string[] items = input.Split(',');
            foreach (var item in items)
            {
                eTargets target = eTargets.None;
                if (Enum.TryParse(item, out target))
                {
                    targets |= target;
                }
                else
                {
                    Logger.LogError("invalid target: " + item);
                }
            }
            return targets;
        }

        // really not needed since it reproduces functionality already 
        // in Enum.Parse (a little easier to call but..)
        static T ParseNames<T>(string delimitedNames) where T : struct
        {
            var split = delimitedNames.Split(',');
            var result = 0;
            foreach (var item in split)
            {
                T i;
                if (Enum.TryParse(item, out i))
                {
                    result |= (int)(object)i;
                }
            }
            return (T)(object)result;
        }
    }
}
