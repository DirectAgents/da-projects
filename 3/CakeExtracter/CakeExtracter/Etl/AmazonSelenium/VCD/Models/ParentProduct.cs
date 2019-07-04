namespace CakeExtracter.Etl.AmazonSelenium.VCD.Models
{
    internal class ParentProduct : ShippingItem
    {
        public string Asin
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

        public string Brand
        {
            get;
            set;
        }
    }
}