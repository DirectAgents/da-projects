namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models
{
    internal class Product : ShippingItem
    {
        public string Asin
        {
            get;
            set;
        }

        public string Title
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
    }
}
