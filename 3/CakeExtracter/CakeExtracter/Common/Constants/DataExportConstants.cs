using CakeExtracter.Common.MatchingPortal.Models;

namespace CakeExtracter.Common.Constants
{
    public class DataExportConstants
    {
        public static ReportColumnProvider[] FullFrameExportColumns = new[]
        {
            new ReportColumnProvider { ColumnName = "buyma_image_url", ValueExtractor = x => x.ProductImage },
            new ReportColumnProvider { ColumnName = "old_product_title", ValueExtractor = x => x.OldProductTitle },
            new ReportColumnProvider { ColumnName = "new_product_title", ValueExtractor = x => x.NewProductTitle },
            new ReportColumnProvider { ColumnName = "product_description", ValueExtractor = x => x.ProductDescription },
            new ReportColumnProvider { ColumnName = "product_id", ValueExtractor = x => x.ProductId },
            new ReportColumnProvider { ColumnName = "brand", ValueExtractor = x => x.Brand },
            new ReportColumnProvider { ColumnName = "matching_status", ValueExtractor = x => x.MatchedStatus},
            new ReportColumnProvider { ColumnName = "product_updated_date", ValueExtractor = x => x.MatchingDate.ToString("s") },
            new ReportColumnProvider { ColumnName = "url", ValueExtractor = x => x.Url },
        };

        public static ReportColumnProvider[] ClientFrameExportColumns = new[]
        {
            new ReportColumnProvider { ColumnName = "product_id", ValueExtractor = x => x.ProductId },
            new ReportColumnProvider { ColumnName = "title", ValueExtractor = x => x.NewProductTitle },
            new ReportColumnProvider { ColumnName = "description", ValueExtractor = x => x.ProductDescription },
        };
    }
}
