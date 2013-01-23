using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccountingBackupWeb.Models.Staging
{
    public partial class CustomerStartingBalance
    {
        public static List<CustomerStartingBalance> GetAll()
        {
            using (var model = new StagingEntities())
            {
                return model.CustomerStartingBalances.ToList();
            }
        }
    }
}
