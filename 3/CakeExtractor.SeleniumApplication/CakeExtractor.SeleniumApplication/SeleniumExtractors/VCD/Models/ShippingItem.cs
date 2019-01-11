namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models
{
    internal class ShippingItem
    {
        public decimal ShippedRevenue
        {
            get;
            set;
        }

        public int ShippedUnits
        {
            get;
            set;
        }

        public int OrderedUnits
        {
            get;
            set;
        }
    }
}
