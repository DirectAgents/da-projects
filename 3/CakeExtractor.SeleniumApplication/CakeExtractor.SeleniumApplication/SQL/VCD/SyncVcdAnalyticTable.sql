DECLARE @accId INT = @@param_0;                -- Account ID

DELETE FROM td.VcdAnalytic  WHERE AccountId= @accId;

INSERT INTO td.VcdAnalytic (Date, BrandName, CategoryName, SubcategoryName,ReleaseDate, 
							Asin, ParentProductAsin, Name,AccountId, Ean, Upc, ApparelSize, ApparelSizeWidth,
							Binding, Color, ModelStyleNumber,
							 ShippedRevenue,ShippedUnits, OrderedUnits, ShippedCOGS, FreeReplacements, CustomerReturns)
select 
	   summaryMetrics.Date                    as 'Date - PRODUCT',
	   brand.Name                             as 'Brand',
	   category.Name                          as 'Category',
	   subcategory.Name                       as 'Subcategory',
	   product.ReleaseDate,
	   product.Asin                           as 'ASIN', 
	   parent.Asin                            as 'Parent ASIN',
	   product.Name                           as 'Product Title',
	   product.AccountId,
	   product.Ean,
	   product.Upc,
	   product.ApparelSize,
	   product.ApparelSizeWidth,
	   product.Binding,
	   product.Color,
	   product.ModelStyleNumber,

	   summaryMetrics.[52] as 'Shipped Revenue',
	   summaryMetrics.[54] as 'Shipped Units',
	   summaryMetrics.[53] as 'Ordered Units',
	   summaryMetrics.[55] as 'Shipped COGS',
	   summaryMetrics.[56] as 'Free Replacements',
	   summaryMetrics.[57] as 'Customer Returns'
	   
from 
	 (
	 select productSummaryMetric.Date,
			productSummaryMetric.ProductId,
			productSummaryMetric.MetricTypeId,
			productSummaryMetric.Value
     from td.VProductSummaryMetric productSummaryMetric
	 ) as metrics
    pivot (
       avg(metrics.Value)
	 for metrics.MetricTypeId
     in 
	 ( [52], [54], [53], [55], [56], [57]) 
	 ) as summaryMetrics
join td.VProduct product         on summaryMetrics.ProductId = product.Id
join td.VSubcategory subcategory on product.SubcategoryId = subcategory.Id 
                              
join td.VCategory category		 on subcategory.CategoryId = category.Id 
                             
join td.VParentProduct parent    on product.ParentProductId = parent.Id
join td.VBrand brand             on product.BrandId = brand.Id 
                             and brand.AccountId = @accId

order by summaryMetrics.Date desc;