using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using ClientPortal.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientPortal.Web
{
    public class Seeder
    {
        public static void Seed()
        {
            var cpRepo = new ClientPortalRepository(new ClientPortalContext());
            Seed(cpRepo);
        }

        public static void Seed(IClientPortalRepository cpRepo)
        {
            if (!cpRepo.Contacts.Any())
            {
                List<Contact> contacts = new List<Contact>
                {
                    new Contact() { FirstName = "Adam", LastName = "Lobelson", Title = "Digital Account Executive", Email = "adam@directagents.com" },
                    new Contact() { FirstName = "Sadie", LastName = "Culbreth", Title = "Senior Account Manager", Email = "sadie@directagents.com" },
                    new Contact() { FirstName = "Jennifer", LastName = "Volkerts", Title = "Account Manager", Email = "jennifer@directagents.com" },
                    new Contact() { FirstName = "Lyle", LastName = "Srebnick", Title = "SVP", Email = "lyles@directagents.com" },
                    new Contact() { FirstName = "Dinesh", LastName = "Boaz", Title = "Managing Director", Email = "dinesh@directagents.com" },
                    new Contact() { FirstName = "Rachel", LastName = "Nugent", Title = "VP of Client Services", Email = "rachel@directagents.com" },
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
                var dinesh = cpRepo.GetContact("Boaz"); // AA: what if two contacts have same last name?
                var rachel = cpRepo.GetContact("Nugent");

                var selectquote = new Advertiser() { AdvertiserId = 580, AdvertiserName = "SelectQuote" };
                selectquote.AdvertiserContacts.Add(new AdvertiserContact() { Contact = dinesh, Order = 1 });
                selectquote.AdvertiserContacts.Add(new AdvertiserContact() { Contact = rachel, Order = 2 });
                var sm = new Advertiser() { AdvertiserId = 207, AdvertiserName = "ServiceMaster" };
                sm.AdvertiserContacts.Add(new AdvertiserContact() { Contact = lyle, Order = 1 });
                sm.AdvertiserContacts.Add(new AdvertiserContact() { Contact = jenv, Order = 2 });
                var itt = new Advertiser() { AdvertiserId = 250, AdvertiserName = "ITT" };
                itt.AdvertiserContacts.Add(new AdvertiserContact() { Contact = adam, Order = 1 });
                itt.AdvertiserContacts.Add(new AdvertiserContact() { Contact = sadie, Order = 2 });
                var tree = new Advertiser() { AdvertiserId = 278, AdvertiserName = "Lending Tree" };
                tree.AdvertiserContacts.Add(new AdvertiserContact() { Contact = lyle, Order = 1 });
                tree.AdvertiserContacts.Add(new AdvertiserContact() { Contact = jenv, Order = 2 });

                cpRepo.AddAdvertiser(selectquote);
                cpRepo.AddAdvertiser(sm);
                cpRepo.AddAdvertiser(itt);
                cpRepo.AddAdvertiser(tree);

                cpRepo.SaveChanges();
            }
        }
    }
}