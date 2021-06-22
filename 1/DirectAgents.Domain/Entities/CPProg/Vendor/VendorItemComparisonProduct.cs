using System;
using DirectAgents.Domain.Abstract;

namespace DirectAgents.Domain.Entities.CPProg.Vendor
{
    public class VendorItemComparisonProduct : BaseVendorEntity, IVendorProductDate
    {
        public string Asin
        {
            get;
            set;
        }

        public string No1ComparedAsin
        {
            get;
            set;
        }

        public string No1ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No1ComparedPercent
        {
            get;
            set;
        }

        public string No2ComparedAsin
        {
            get;
            set;
        }

        public string No2ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No2ComparedPercent
        {
            get;
            set;
        }

        public string No3ComparedAsin
        {
            get;
            set;
        }

        public string No3ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No3ComparedPercent
        {
            get;
            set;
        }

        public string No4ComparedAsin
        {
            get;
            set;
        }

        public string No4ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No4ComparedPercent
        {
            get;
            set;
        }

        public string No5ComparedAsin
        {
            get;
            set;
        }

        public string No5ComparedProductTitle
        {
            get;
            set;
        }

        public decimal No5ComparedPercent
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
