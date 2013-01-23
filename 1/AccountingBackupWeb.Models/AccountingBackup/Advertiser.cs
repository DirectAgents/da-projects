using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class Advertiser
    {
        public static Advertiser ById(int advertiserID)
        {
            using (var model = new AccountingBackupEntities())
            {
                return model.Advertisers.First(c => c.Id == advertiserID);
            }
        }

        public static Advertiser ByCustomerName(AccountingBackupEntities model, string customerName)
        {
            Advertiser advertiser = GetAdvertiserForCustomer(model, customerName);
            UpdateStartingBalance(advertiser);
            return advertiser;
        }

        static void UpdateStartingBalance(Advertiser advertiser)
        {
            // update starting balance if possible
            var advertiserInfo = AdvertiserInfos.FirstOrDefault(c => c.DirectTrackAdvertiserName == advertiser.Name);
            if (advertiserInfo != null)
            {
                advertiser.StartingBalance = advertiserInfo.StartingBalance2012 ?? 0;
            }
        }

        static Advertiser GetAdvertiserForCustomer(AccountingBackupEntities model, string customerName)
        {
            // check the database for an advertiser matching customer name
            var advertiser = model.Advertisers.FirstOrDefault(c => c.Name == customerName);
            if (advertiser == null)
            {
                string advertiserName = "Default Advertiser";
                // check config mappings for matching customer name
                var advertiserInfo = AdvertiserInfos.FirstOrDefault(c => c.QuickBooksCustomerName == customerName);
                if (advertiserInfo != null)
                {
                    advertiserName = advertiserInfo.DirectTrackAdvertiserName ?? advertiserName;
                    Log.Write("Using configured mapping: " + customerName + "->" + advertiserName);
                }
                // check the database for the advertiser
                advertiser = model.Advertisers.FirstOrDefault(c => c.Name == advertiserName);
                if (advertiser == null) // create advertiser if necessary
                {
                    advertiser = new Advertiser {
                        Name = advertiserName,
                        CreditCheck = "-",
                        CreditLimit = 0,
                        Currency = model.Currencies.First(c => c.Name == "USD"),
                        Notes = "-"
                    };
                }
            }
            Log.Write(advertiser.Name + "->" + customerName);
            return advertiser;
        }

        static Config.AdvertiserInfo[] AdvertiserInfos
        {
            get { return Advertiser._AdvertiserInfos.Value; }
        }

        static Lazy<Config.AdvertiserInfo[]> _AdvertiserInfos = new Lazy<Config.AdvertiserInfo[]>(() =>
        {
            using (var model = new Config.ABWebConfig())
            {
                return model.AdvertiserInfos.ToArray();
            }
        });
    }
}
