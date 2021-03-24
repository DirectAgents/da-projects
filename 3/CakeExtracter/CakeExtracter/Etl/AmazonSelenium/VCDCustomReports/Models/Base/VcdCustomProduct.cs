using System.Collections.Generic;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base
{
    public abstract class VcdCustomProduct
    {
        public string Asin { get; set; }

        public string Name { get; set; }
    }
}