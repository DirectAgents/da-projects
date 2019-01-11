using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models
{
    internal class VcdReportData
    {
        public List<Product> Products
        {
            get;
            set;
        }

        public List<Category> Categories
        {
            get;
            set;
        }

        public List<Subcategory> Subcategories
        {
            get;
            set;
        }
    }
}
