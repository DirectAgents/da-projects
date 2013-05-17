using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web
{
    public class Seeder
    {
        public static void Seed(IClientPortalRepository cpRepo)
        {
            if (!cpRepo.Contacts.Any())
            {
                List<Contact> contacts = new List<Contact>
                {
                    new Contact() { FirstName = "Adam", LastName = "Lobelson", Title = "Digital Account Executive", Email = "adam@directagents.com" },
                    new Contact() { FirstName = "Sadie", LastName = "Culbreth", Title = "Senior Account Manager", Email = "sadie@directagents.com" },
                    new Contact() { FirstName = "Jennifer", LastName = "Volkerts", Title = "Account Manager", Email = "jennifer@directagents.com" },
                    new Contact() { FirstName = "Lyle", LastName = "Srebnick", Title = "SVP", Email = "lyles@directagents.com" }
                };
                foreach (var contact in contacts)
                    cpRepo.AddContact(contact);

                cpRepo.SaveChanges();
            }
            if (!cpRepo.Advertisers.Any())
            {
                var adam = cpRepo.GetContact("Lobelson");
                var sadie = cpRepo.GetContact("Culbreth");
                var jenv = cpRepo.GetContact("Volkerts");
                var lyle = cpRepo.GetContact("Srebnick");

                var sm = new Advertiser() { AdvertiserId = 207, AdvertiserName = "ServiceMaster", LogoFilename = "logoAHS.png" };
                sm.AdvertiserContacts.Add(new AdvertiserContact() { Contact = lyle, Order = 1 });
                sm.AdvertiserContacts.Add(new AdvertiserContact() { Contact = jenv, Order = 2 });
                var itt = new Advertiser() { AdvertiserId = 250, AdvertiserName = "ITT", LogoFilename = "logoITT.png" };
                itt.AdvertiserContacts.Add(new AdvertiserContact() { Contact = adam, Order = 1 });
                itt.AdvertiserContacts.Add(new AdvertiserContact() { Contact = sadie, Order = 2 });
                var tree = new Advertiser() { AdvertiserId = 278, AdvertiserName = "Lending Tree", LogoFilename = "logoLT.png" };
                tree.AdvertiserContacts.Add(new AdvertiserContact() { Contact = lyle, Order = 1 });
                tree.AdvertiserContacts.Add(new AdvertiserContact() { Contact = jenv, Order = 2 });

                cpRepo.AddAdvertiser(sm);
                cpRepo.AddAdvertiser(itt);
                cpRepo.AddAdvertiser(tree);

                cpRepo.SaveChanges();
            }
        }
    }
}