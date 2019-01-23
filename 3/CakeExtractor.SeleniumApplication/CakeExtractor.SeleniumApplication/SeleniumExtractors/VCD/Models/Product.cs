using System;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models
{
    internal class Product : ShippingItem
    {
        public string Asin
        {
            get;
            set;
        }

        public string ParentAsin
        {
            get;
            set;
        }

        public string Category
        {
            get;
            set;
        }

        public string Subcategory
        {
            get;
            set;
        }

        public string Ean
        {
            get;
            set;
        }

        public string Upc
        {
            get;
            set;
        }

        public string Brand
        {
            get;
            set;
        }

        public string ApparelSize
        {
            get;
            set;
        }

        public string ApparelSizeWidth
        {
            get;
            set;
        }

        public string Binding
        {
            get;
            set;
        }

        public string Color
        {
            get;
            set;
        }

        public string ModelStyleNumber
        {
            get;
            set;
        }

        public DateTime ReleaseDate
        {
            get;
            set;
        }
    }
}
