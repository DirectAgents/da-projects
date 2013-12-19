using System.Collections.Generic;
using System.Linq;
using CakeExtracter.CakeMarketingApi.Entities;
using CakeExtracter.Common;
using MoreLinq;

namespace CakeExtracter.Etl.CakeMarketing.Loaders
{
    public class AdvertisersLoader : Loader<Advertiser>
    {
        private readonly bool includeContacts;

        public AdvertisersLoader(bool includeContacts)
        {
            this.includeContacts = includeContacts;
        }

        protected override int Load(List<Advertiser> items)
        {
            Logger.Info("Synching {0} advertisers...", items.Count);
            using (var db = new ClientPortal.Data.Contexts.ClientPortalContext())
            {
                var newCakeRoles = new List<ClientPortal.Data.Contexts.CakeRole>();

                foreach (var item in items)
                {
                    var advertiser = db.Advertisers
                        .Where(a => a.AdvertiserId == item.AdvertiserId)
                        .SingleOrFallback(() =>
                        {
                            var newAdvertiser = new ClientPortal.Data.Contexts.Advertiser
                            {
                                AdvertiserId = item.AdvertiserId,
                                Culture = "en-US",
                                ShowCPMRep = false,
                                ShowConversionData = false,
                                ConversionValueName = null,
                                ConversionValueIsNumber = false,
                                HasSearch = false
                            };
                            db.Advertisers.Add(newAdvertiser);
                            return newAdvertiser;
                        });

                    advertiser.AdvertiserName = item.AdvertiserName;

                    if (includeContacts)
                    {
                        Logger.Info("Synching {0} contacts...", item.Contacts.Count);
                        foreach (var ci in item.Contacts)
                        {
                            var cakeRole = db.CakeRoles.Where(cr => cr.CakeRoleId == ci.Role.RoleId)
                                .SingleOrFallback(() =>
                                {
                                    var newCakeRole = newCakeRoles.SingleOrDefault(cr => cr.CakeRoleId == ci.Role.RoleId);
                                    if (newCakeRole == null)
                                    {
                                        Logger.Info("Adding new CakeRole: {0} ({1})", ci.Role.RoleName, ci.Role.RoleId);
                                        newCakeRole = new ClientPortal.Data.Contexts.CakeRole
                                        {
                                            CakeRoleId = ci.Role.RoleId,
                                            RoleName = ci.Role.RoleName
                                        };
                                        newCakeRoles.Add(newCakeRole);
                                    }
                                    return newCakeRole;
                                });

                            var cakeContact = db.CakeContacts
                                .Where(c => c.CakeContactId == ci.ContactId)
                                .SingleOrFallback(() =>
                                {
                                    var newCakeContact = new ClientPortal.Data.Contexts.CakeContact
                                    {
                                        CakeContactId = ci.ContactId,
                                        CakeRole = cakeRole
                                    };
                                    db.CakeContacts.Add(newCakeContact);
                                    return newCakeContact;
                                });

                            cakeContact.CakeRole = cakeRole;
                            cakeContact.FirstName = ci.FirstName;
                            cakeContact.LastName = ci.LastName;
                            cakeContact.EmailAddress = ci.EmailAddress;
                            cakeContact.Title = ci.Title;
                            cakeContact.PhoneWork = ci.PhoneWork;
                            cakeContact.PhoneCell = ci.PhoneCell;
                            cakeContact.PhoneFax = ci.PhoneFax;
                        }
                    }
                }
                Logger.Info("Advertisers/CakeContacts/CakeRoles: " + db.ChangeCountsAsString());
                db.SaveChanges();
            }
            return items.Count;
        }
    }
}
