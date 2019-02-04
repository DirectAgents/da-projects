namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models
{
    internal class ShippingItem
    {
        public string Name
        {
            get;
            set;
        }

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

        public decimal ShippedCogs
        {
            get;
            set;
        }

        public int CustomerReturns
        {
            get;
            set;
        }

        public int FreeReplacements
        {
            get;
            set;
        }
    }
}
