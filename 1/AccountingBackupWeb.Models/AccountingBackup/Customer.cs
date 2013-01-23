using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.AccountingBackup
{
    public partial class Customer
    {
        public static Customer ByAdvertiserId(int advertiserID)
        {
            using (var model = new AccountingBackupEntities())
            {
                return model.Customers.FirstOrDefault(c => c.Advertiser.Id == advertiserID);
            }
        }
    }
}
