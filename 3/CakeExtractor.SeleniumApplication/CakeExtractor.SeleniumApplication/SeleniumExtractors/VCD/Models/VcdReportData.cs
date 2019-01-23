using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.Models
{
    internal class VcdReportData
    {
        public List<Brand> Brands
        {
            get;
            set;
        }

        public List<ParentProduct> ParentProducts
        {
            get;
            set;
        }

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
