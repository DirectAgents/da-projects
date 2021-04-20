using System;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public class VendorMarketBasketAnalysisProduct : BaseVendorEntity
    {
        public string Asin
        {
            get;
            set;
        }

        public string N1PurchasedAsin
        {
            get;
            set;
        }

        public string N1PurchasedTitle
        {
            get;
            set;
        }

        public decimal N1Combination
        {
            get;
            set;
        }

        public string N2PurchasedAsin
        {
            get;
            set;
        }

        public string N2PurchasedTitle
        {
            get;
            set;
        }

        public decimal N2Combination
        {
            get;
            set;
        }

        public string N3PurchasedAsin
        {
            get;
            set;
        }

        public string N3PurchasedTitle
        {
            get;
            set;
        }

        public decimal N3Combination
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
