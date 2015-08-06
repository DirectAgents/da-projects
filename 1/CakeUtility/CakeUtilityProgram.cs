using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DirectAgents.Common;

namespace CakeUtility
{
    public partial class CakeUtilityProgram : ConsoleApplication
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //System.Console.Clear();
            bool hasRun = false;
            try
            {
                Run<CakeUtilityProgram>(args);
                hasRun = true;
            }
            catch (Exception e)
            {
                if (hasRun)
                {
                    LogException(e);
                }
                else
                {
                    var sb = new StringBuilder();
                    var Console = new StringWriter(sb);
                    Console.WriteLine("------------------------------");
                    Console.WriteLine(e.Message);
                    Console.WriteLine("------------------------------");
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine("------------------------------");
                    if (e.InnerException != null)
                    {
                        Console.WriteLine("------------------------------");
                        Console.WriteLine(e.InnerException.Message);
                        Console.WriteLine("------------------------------");
                        Console.WriteLine(e.InnerException.StackTrace);
                        Console.WriteLine("------------------------------");

                        if (e.InnerException.InnerException != null)
                        {
                            Console.WriteLine("------------------------------");
                            Console.WriteLine(e.InnerException.InnerException.Message);
                            Console.WriteLine("------------------------------");
                            Console.WriteLine(e.InnerException.InnerException.StackTrace);
                            Console.WriteLine("------------------------------");
                        }
                    }
                    throw new Exception(sb.ToString());
                }
            }
            if (hasRun)
            {
                var ids = Locator.Get<ICollection<int>>("CakeUtilityErrorIds");
                foreach (var id in ids)
                {
                    Console.WriteLine("Logged exception to CakeUtilityError with Id={0}", id);
                }
            }
        }

        private static Lazy<IExceptionLogger> ExceptionLogger = new Lazy<IExceptionLogger>(() => { return Locator.Get<IExceptionLogger>(); });
        private static void LogException(Exception e)
        {
            IExceptionLogger logger = ExceptionLogger.Value;
            logger.LogException(e);
        }

        public CakeUtilityProgram()
        {
            MapperItem.AddConversion<Int32, Decimal>(c => Convert.ToDecimal(c));
            MapperItem.AddConversion<Int32, String>(c => c.ToString());
            MapperItem.AddConversion<Boolean, String>(c => c.ToString());
            MapperItem.AddConversion<Int16, String>(c => c.ToString());
            MapperItem.AddConversion<DateTime, String>(c => c.ToString());
            MapperItem.AddConversion<String, DateTime>(c => DateTime.Parse(c as string));
            MapperItem.AddConversion<Decimal, String>(c => c.ToString());
            MapperItem.AddConversion<Cake.Data.Wsdl.ExportService.offer_type, String>(c => (c as Cake.Data.Wsdl.ExportService.offer_type).offer_type_name);
            MapperItem.AddConversion<String, Int32>(c => Int32.Parse(c.ToString()));
            MapperItem.AddConversion<Cake.Data.Wsdl.ExportService.currency, String>(c => (c as Cake.Data.Wsdl.ExportService.currency).currency_abbr);
            MapperItem.AddConversion<String[], String>(c => String.Join(",", c));
        }

        #region Contacts
        // NOTE: currently there is no staging table for Cake Contacts.

        //[Command]
        //public void ExtractCakeContacts()
        //{
        //    try
        //    {
        //        Locator.Get<ICommand>("ScreenScrapeCakeContacts").Execute();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //}

        //[Command]
        //public void LoadCakeContacts()
        //{
        //    MergeUtility.Merge<
        //        Cake.Data.Csv.Contacts,
        //        Cake.Model.CakeContact,
        //        Cake.Model.CakeEntities>();
        //}

        //[Command]
        //public void RefreshCakeContacts()
        //{
        //    this.ExtractCakeContacts();
        //    this.LoadCakeContacts();
        //} 
        #endregion

        [Command]
        public void ReCreateCakeStagingDatabase()
        {
            using (var context = new Cake.Model.Staging.CakeStagingEntities())
            {
                if (context.DatabaseExists())
                {
                    context.DeleteDatabase();
                }
                context.CreateDatabase();
                Console.WriteLine("Database created");
            }
        }

        [Command]
        public void ResetStagingTables() // Deletes all from staging.CakeAdvertisers and staging.CakeOffers
        {
            using (var context = new Cake.Model.Staging.CakeStagingEntities())
            {
                context.ExecuteStoreCommand("ResetStagingTables");
                Console.WriteLine("Staging tables reset");
            }
        }

        [Command]
        public void RefreshStagingTables()
        {
            ResetStagingTables();
            ExtractAndStageCakeAdvertisers();
            ExtractAndStageCakeOffers();
        }

        #region Advertisers
        [Command]
        public void ExtractAndStageCakeAdvertisers()
        {
            Merge<Cake.Data.Wsdl.ExportService.advertiser, Cake.Model.Staging.CakeAdvertiser, Cake.Model.Staging.CakeStagingEntities>();
        }

        [Command]
        public void LoadCakeAdvertisers()
        {
            MergeUtility.Merge(
                GetInstance<EntitySource<Cake.Model.Staging.CakeAdvertiser, Cake.Model.Staging.CakeStagingEntities>>(),
                GetInstance<EntityTarget<Cake.Model.CakeAdvertiser, Cake.Model.CakeEntities>>());
        }
        #endregion

        #region Offers
        [Command]
        public void ExtractAndStageCakeOffers()
        {
            MergeUtility.Merge(
                GetInstance<Source<Cake.Data.Wsdl.ExportService.offer1>>(),
                GetInstance<EntityTarget<Cake.Model.Staging.CakeOffer, Cake.Model.Staging.CakeStagingEntities>>());
        }

        [Command]
        public void LoadCakeOffers()
        {
            MergeUtility.Merge(
                GetInstance<EntitySource<Cake.Model.Staging.CakeOffer, Cake.Model.Staging.CakeStagingEntities>>(),
                GetInstance<EntityTarget<Cake.Model.CakeOffer, Cake.Model.CakeEntities>>());
        }

        [Command]
        public void RefreshCakeOffers()
        {
            this.ExtractAndStageCakeOffers();
            this.LoadCakeOffers();
        }
        #endregion

        #region Affiliates
        [Command]
        public void RefreshCakeAffiliates()
        {
            this.ExtractAndStageCakeAffiliates();
            this.LoadCakeAffiliates();
        }

        [Command]
        public void ExtractAndStageCakeAffiliates()
        {
            MergeUtility.Merge(
                Locator.Get<Source<Cake.Data.Wsdl.ExportService.affiliate>>(),
                Locator.Get<EntityTarget<Cake.Model.Staging.CakeAffiliate, Cake.Model.Staging.CakeStagingEntities>>());
        }

        [Command]
        public void LoadCakeAffiliates()
        {
            MergeUtility.Merge(
                Locator.Get<EntitySource<Cake.Model.Staging.CakeAffiliate, Cake.Model.Staging.CakeStagingEntities>>(),
                Locator.Get<EntityTarget<Cake.Model.CakeAffiliate, Cake.Model.CakeEntities>>());
        }
        #endregion

        #region Conversions
        [Command]
        public void ExtractAndStageCakeConversions()
        {
            int year = 2012;
            int month = 9;
            int startDay = 2;
            int endDay = 2;

            for (int day = startDay; day <= endDay; day++)
            {
                DateTime extractDate = new DateTime(year, month, day);

                Console.WriteLine(extractDate);

                MergeUtility.Merge<
                    Cake.Data.Wsdl.ReportsService.conversion,
                    Cake.Model.Staging.CakeConversion,
                    Cake.Model.Staging.CakeStagingEntities>();
            }
        }

        [Command]
        public void LoadCakeConversions()
        {
            MergeUtility.Merge(
                GetInstance<EntitySource<Cake.Model.Staging.CakeConversion, Cake.Model.Staging.CakeStagingEntities>>(),
                GetInstance<EntityTarget<Cake.Model.CakeConversion, Cake.Model.CakeEntities>>());
        }
        #endregion

        #region Campaigns
        [Command]
        public void ExtractAndLoadCakeCampaigns()
        {
            X.DoIt();
        }
        #endregion

        private static T GetInstance<T>()
        {
            return Locator.Get<T>();
        }

        private void Merge<TSource, TTarget, TContainer>()
            where TSource : class
            where TTarget : class, new()
            where TContainer : IDisposable
        {
            Locator.Get<IMerger<TSource, TTarget, TContainer>>().Execute();
        }
    }
}
