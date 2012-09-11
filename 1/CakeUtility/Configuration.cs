using System;
using System.Collections.Generic;
using System.Linq;
using DirectAgents.Common;
using Microsoft.Practices.Unity;

namespace CakeUtility
{
    public partial class CakeUtilityProgram : ConsoleApplication
    {
        protected override void Initialize()
        {
            if (!Configuration.IsStaticInitializationDone)
            {
                try
                {
                    Configuration.StaticInitialization();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Static initialization failed: " + e.Message);
                    throw e;
                }
            }
        }
    }

    internal static class Configuration
    {
        internal static bool IsStaticInitializationDone { get; set; }

        internal static void StaticInitialization()
        {
            IUnityContainer container = new UnityContainer()

                .ConfigureInterception()

                .RegisterType(typeof(IMerger<,,>), typeof(Merger<,,>))

                // todo: the way factory is used is overly complicated and should be rethought
                .RegisterType<Factory, ConcreteFactory>(new PerThreadLifetimeManager())

                // Loggers
                .RegisterType<ILogger, DefaultLogger>()
                .RegisterType<IExceptionLogger, DefaultExceptionLogger>()

                // Global errors (todo: useful?)
                .RegisterType<ICollection<int>, List<int>>("CakeUtilityErrorIds", new PerThreadLifetimeManager(), new InjectionConstructor())

                // Cake Service
                .RegisterType<Cake.Data.Wsdl.ICakeService, Cake.Data.Wsdl.CakeService>(new InjectionConstructor("FCjdYAcwQE"))

            #region Affiliates
                // Extract from web service
                .RegisterInstance<Source<Cake.Data.Wsdl.ExportService.affiliate>>(
                    new Source<Cake.Data.Wsdl.ExportService.affiliate>(
                        () => Locator.Get<Cake.Data.Wsdl.ICakeService>().ExportAffiliates(),
                        "affiliate_id"))

                    // Load to staging
                .RegisterInstance<EntityTarget<Cake.Model.Staging.CakeAffiliate, Cake.Model.Staging.CakeStagingEntities>>(
                    new EntityTarget<Cake.Model.Staging.CakeAffiliate, Cake.Model.Staging.CakeStagingEntities>(
                        () => new Cake.Model.Staging.CakeStagingEntities(),
                        "CakeAffiliates",
                        "Affiliate_Id",
                        c => c.SaveChanges()))

                    // Extract from staging
                .RegisterInstance<EntitySource<Cake.Model.Staging.CakeAffiliate, Cake.Model.Staging.CakeStagingEntities>>(
                    new EntitySource<Cake.Model.Staging.CakeAffiliate, Cake.Model.Staging.CakeStagingEntities>(
                        () => new Cake.Model.Staging.CakeStagingEntities(),
                        "CakeAffiliates",
                        "Affiliate_Id"))

                    // Load to production
                .RegisterInstance<EntityTarget<Cake.Model.CakeAffiliate, Cake.Model.CakeEntities>>(
                    new EntityTarget<Cake.Model.CakeAffiliate, Cake.Model.CakeEntities>(() =>
                        new Cake.Model.CakeEntities(),
                        "CakeAffiliates",
                        "Affiliate_Id",
                        c => c.SaveChanges()))
            #endregion

            #region Advertisers

                // Extract from web service
                .RegisterInstance<Source<Cake.Data.Wsdl.ExportService.advertiser>>(
                    new Source<Cake.Data.Wsdl.ExportService.advertiser>(
                        () => Locator.Get<Cake.Data.Wsdl.ICakeService>().ExportAdvertisers(), "advertiser_id"))

                // Load to staging
                .RegisterInstance<EntityTarget<Cake.Model.Staging.CakeAdvertiser, Cake.Model.Staging.CakeStagingEntities>>(
                    new EntityTarget<Cake.Model.Staging.CakeAdvertiser, Cake.Model.Staging.CakeStagingEntities>(
                        () => new Cake.Model.Staging.CakeStagingEntities(), "CakeAdvertisers", "Advertiser_Id", c => c.SaveChanges()))

                // Extract from staging
                .RegisterInstance<EntitySource<Cake.Model.Staging.CakeAdvertiser, Cake.Model.Staging.CakeStagingEntities>>(
                    new EntitySource<Cake.Model.Staging.CakeAdvertiser, Cake.Model.Staging.CakeStagingEntities>(
                        () => new Cake.Model.Staging.CakeStagingEntities(), "CakeAdvertisers", "Advertiser_Id"))

                // Load to production
                .RegisterInstance<EntityTarget<Cake.Model.CakeAdvertiser, Cake.Model.CakeEntities>>(
                    new EntityTarget<Cake.Model.CakeAdvertiser, Cake.Model.CakeEntities>(
                        () => new Cake.Model.CakeEntities(), "CakeAdvertisers", "Advertiser_Id", c => c.SaveChanges()))

            #endregion

            #region Offers
                // Extract from web service
                .RegisterInstance<Source<Cake.Data.Wsdl.ExportService.offer1>>(
                    new Source<Cake.Data.Wsdl.ExportService.offer1>(
                        () => Locator.Get<Cake.Data.Wsdl.ICakeService>().ExportOffers(),
                        "offer_id"))

                // Load to staging
                .RegisterInstance<EntityTarget<Cake.Model.Staging.CakeOffer, Cake.Model.Staging.CakeStagingEntities>>(
                    new EntityTarget<Cake.Model.Staging.CakeOffer, Cake.Model.Staging.CakeStagingEntities>(
                        () => new Cake.Model.Staging.CakeStagingEntities(),
                        "CakeOffers",
                        "Offer_Id",
                        c => c.SaveChanges()))

                // Extract from staging
                .RegisterInstance<EntitySource<Cake.Model.Staging.CakeOffer, Cake.Model.Staging.CakeStagingEntities>>(
                    new EntitySource<Cake.Model.Staging.CakeOffer, Cake.Model.Staging.CakeStagingEntities>(
                        () => new Cake.Model.Staging.CakeStagingEntities(),
                        "CakeOffers",
                        "Offer_Id"))

                // Load to production
                .RegisterInstance<EntityTarget<Cake.Model.CakeOffer, Cake.Model.CakeEntities>>(
                    new EntityTarget<Cake.Model.CakeOffer, Cake.Model.CakeEntities>(
                        () => new Cake.Model.CakeEntities(),
                        "CakeOffers",
                        "Offer_Id",
                        c => c.SaveChanges()))
            #endregion

            #region Conversions
                // Extract from web service
                .RegisterInstance<Source<Cake.Data.Wsdl.ReportsService.conversion>>(new Source<Cake.Data.Wsdl.ReportsService.conversion>(() => Locator.Get<Cake.Data.Wsdl.ICakeService>().Conversions(), "conversion_id"))

                // Load to staging
                .RegisterInstance<EntityTarget<Cake.Model.Staging.CakeConversion, Cake.Model.Staging.CakeStagingEntities>>(
                    new EntityTarget<Cake.Model.Staging.CakeConversion, Cake.Model.Staging.CakeStagingEntities>(
                        () => new Cake.Model.Staging.CakeStagingEntities(),
                        "CakeConversions",
                        "Conversion_Id",
                        c => c.SaveChanges()))

                // Extract from staging
                .RegisterInstance<EntitySource<Cake.Model.Staging.CakeConversion, Cake.Model.Staging.CakeStagingEntities>>(
                    new EntitySource<Cake.Model.Staging.CakeConversion, Cake.Model.Staging.CakeStagingEntities>(
                        () => new Cake.Model.Staging.CakeStagingEntities(),
                        "CakeConversions",
                        "Conversion_Id",
                        conversion =>
                        {
                            using (var db = new Cake.Model.CakeEntities())
                            {
                                if (db.CakeCampaigns.Any(c => c.Campaign_Id == conversion.Campaign_Id))
                                {
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("No exist campaign for conversion: " + conversion.Campaign_Id);
                                    return false;
                                }
                            }
                        }))

                // Load to production
                .RegisterInstance<EntityTarget<Cake.Model.CakeConversion, Cake.Model.CakeEntities>>(
                    new EntityTarget<Cake.Model.CakeConversion, Cake.Model.CakeEntities>(
                        () => new Cake.Model.CakeEntities(),
                        "CakeConversions",
                        "Conversion_Id",
                        c => c.SaveChanges()))
            #endregion

            #region Contacts
                // Screen scrape cake contacts to CSV file
                //.RegisterInstance<string>("CakeContactsCsvFileName", @"C:\DACode\CakeETL\Contacts(8).csv")

                //.RegisterType<DirectAgents.Common.ICommand, Cake.Data.ScreenScrape.Commands.ExportCakeContacts>("ScreenScrapeCakeContacts")

                    // Extract Contacts from CSV file
                //.RegisterInstance<Source<Cake.Data.Csv.Contacts>>(
                //    new Source<Cake.Data.Csv.Contacts>(
                //        () =>
                //        {
                //            FileHelperEngine engine = new FileHelperEngine(typeof(Cake.Data.Csv.Contacts));
                //            //string file = FileNameUtility.Existing(Locator.Get<string>("CakeContactsCsvFileName"));
                //            string file = Locator.Get<string>("CakeContactsCsvFileName");
                //            var contacts = engine.ReadFile(file) as Cake.Data.Csv.Contacts[];
                //            return contacts;
                //        },
                //        "ContactID"))

                    // Load to production
                //.RegisterInstance<EntityTarget<Cake.Model.CakeContact, Cake.Model.CakeEntities>>(
                //   new EntityTarget<Cake.Model.CakeContact, Cake.Model.CakeEntities>(
                //       () => new Cake.Model.CakeEntities(),
                //       "CakeContacts",
                //       "Contact_Id",
                //       c =>
                //       {
                //c.ObjectStateManager
                //    .GetObjectStateEntries(EntityState.Added)
                //    .Select(entry => entry.Entity)
                //    .OfType<Cake.Model.CakeContact>()
                //    .Where(cc => string.IsNullOrWhiteSpace(cc.Name))
                //    .ToList()
                //    .ForEach(item => c.Detach(item));
                //    c.SaveChanges();
                //}))
            #endregion
;

            // Validate registrations.
            container.Registrations.ToList().ForEach(c =>
            {
                if (c.MappedToType.GetCustomAttributes(typeof(NotThreadSafeAttribute), true).Length > 0)
                {
                    if (!(c.LifetimeManager is PerThreadLifetimeManager))
                    {
                        throw new Exception("The type " + c.MappedToType.FullName + " has the attribute "
                            + typeof(NotThreadSafeAttribute).FullName + " applied and therefore must use a "
                            + typeof(PerThreadLifetimeManager).FullName + ", however it uses " + c.LifetimeManagerType.FullName + ".");
                    }
                }
            });

            Locator.Initialize(container);

            IsStaticInitializationDone = true;
        }
    }
}
