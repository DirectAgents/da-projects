using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public abstract class VendorRepeatPurchaseBehaviorProduct : BaseVendorEntity
    {
        public string Asin
        {
            get;
            set;
        }

        public int UniqueCustomers
        {
            get;
            set;
        }

        public decimal RepeatPurchaseRevenue
        {
            get;
            set;
        }

        public decimal RepeatPurchaseRevenuePriorPeriod
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }
    }
}
