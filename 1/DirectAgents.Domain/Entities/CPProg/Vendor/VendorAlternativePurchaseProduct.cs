using System;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public class VendorAlternativePurchaseProduct : BaseVendorEntity, IVendorProductOneDate
    {
        public string Asin
        {
            get;
            set;
        }

        public string No1PurchasedAsin
        {
            get;
            set;
        }

        public string No1PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No1PurchasedPercent
        {
            get;
            set;
        }

        public string No2PurchasedAsin
        {
            get;
            set;
        }

        public string No2PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No2PurchasedPercent
        {
            get;
            set;
        }

        public string No3PurchasedAsin
        {
            get;
            set;
        }

        public string No3PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No3PurchasedPercent
        {
            get;
            set;
        }

        public string No4PurchasedAsin
        {
            get;
            set;
        }

        public string No4PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No4PurchasedPercent
        {
            get;
            set;
        }

        public string No5PurchasedAsin
        {
            get;
            set;
        }

        public string No5PurchasedProductTitle
        {
            get;
            set;
        }

        public decimal No5PurchasedPercent
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }
    }
}
