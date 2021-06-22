using System;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public abstract class VendorRepeatPurchaseBehaviorProduct : BaseVendorEntity, IVendorProductDateRange
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
