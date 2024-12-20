﻿using System;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Domain.Entities.CPProg.Vendor.NetPpm
{
    public abstract class VendorNetPpmProduct : BaseVendorEntity, IVendorProductDateRange
    {
        public string Asin
        {
            get;
            set;
        }

        public string Subcategory
        {
            get;
            set;
        }

        public decimal NetPpm
        {
            get;
            set;
        }

        public decimal NetPpmPriorYearPercentChange
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
