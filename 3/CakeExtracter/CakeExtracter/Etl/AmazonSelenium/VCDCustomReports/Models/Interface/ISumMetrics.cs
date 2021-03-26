using System.Collections.Generic;
using CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Base;

namespace CakeExtracter.Etl.AmazonSelenium.VCDCustomReports.Models.Interface
{
    public interface ISumMetrics<in TProduct>
        where TProduct : VcdCustomProduct
    {
        void SetMetrics(IEnumerable<TProduct> items);
    }
}