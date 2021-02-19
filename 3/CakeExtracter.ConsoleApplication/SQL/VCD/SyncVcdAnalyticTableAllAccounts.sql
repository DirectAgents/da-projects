TRUNCATE TABLE td.VcdAnalytic

INSERT INTO td.VcdAnalytic
(
    [Date],
    [BrandName],
    [CategoryName],
    [SubcategoryName],
    [ReleaseDate],
    [Asin],
    [ParentProductAsin],
    [Name],
    [AccountId],
    [Ean],
    [Upc],
    [ApparelSize],
    [ApparelSizeWidth],
    [Binding],
    [Color],
    [ModelStyleNumber],
    [ShippedRevenue],
    [ShippedUnits],
    [OrderedUnits],
    [ShippedCOGS],
    [FreeReplacements],
    [CustomerReturns],
    [OrderedRevenue],
    [GlanceViews],
    [SalesRank],
    [AverageSalesPrice],
    [SellableOnHandUnits],
    [NumberOfCustomerReviews],
    [NumberOfCustomerReviewsLifeToDate],
    [AverageCustomerRating],
    [FiveStars],
    [FourStars],
    [ThreeStars],
    [TwoStars],
    [OneStar]
)
SELECT
    summarymetrics.[Date]       AS 'Date - PRODUCT',
    brand.[Name]                AS 'Brand',
    category.[Name]             AS 'Category',
    subcategory.[Name]          AS 'Subcategory',
    product.[ReleaseDate],
    product.[Asin]              AS 'ASIN',
    parent.[Asin]               AS 'Parent ASIN',
    product.[Name]              AS 'Product Title',
    product.[AccountId],
    product.[Ean],
    product.[Upc],
    product.[ApparelSize],
    product.[ApparelSizeWidth],
    product.[Binding],
    product.[Color],
    product.[ModelStyleNumber],
    summaryMetrics.[52]         AS 'Shipped Revenue',
    summaryMetrics.[54]         AS 'Shipped Units',
    summaryMetrics.[53]         AS 'Ordered Units',
    summaryMetrics.[55]         AS 'Shipped COGS',
    summaryMetrics.[56]         AS 'Free Replacements',
    summaryMetrics.[57]         AS 'Customer Returns',
    summaryMetrics.[58]         AS 'Ordered Revenue',
    summaryMetrics.[77]         AS 'Glance Views',
    summaryMetrics.[78]         AS 'Sales Rank',
    summaryMetrics.[79]         AS 'Average Sales Price',
    summaryMetrics.[80]         AS 'Sellable On Hand Units',
    summaryMetrics.[81]         AS 'Number Of Customer Reviews',
    summaryMetrics.[82]         AS 'Number Of Customer Reviews Life To Date',
    summaryMetrics.[83]         AS 'Average Customer Rating',
    summaryMetrics.[84]         AS '5 Stars',
    summaryMetrics.[85]         AS '4 Stars',
    summaryMetrics.[86]         AS '3 Stars',
    summaryMetrics.[87]         AS '2 Stars',
    summaryMetrics.[88]         AS '1 Star'
FROM
(
    SELECT
        productSummaryMetric.Date,
        productSummaryMetric.ProductId,
        productSummaryMetric.MetricTypeId,
        productSummaryMetric.Value
    FROM td.VProductSummaryMetric productSummaryMetric
) AS metrics
PIVOT
(
    AVG(metrics.Value)
    FOR metrics.MetricTypeId IN ([52], [54], [53], [55], [56], [57], [58], [77], [78], [79], [80], [81], [82], [83], [84], [85], [86], [87], [88])
) AS summaryMetrics
INNER JOIN td.VProduct product ON summaryMetrics.ProductId = product.Id
INNER JOIN td.VSubcategory subcategory ON product.SubcategoryId = subcategory.Id
INNER JOIN td.VCategory category ON subcategory.CategoryId = category.Id
INNER JOIN td.VParentProduct parent ON product.ParentProductId = parent.Id
INNER JOIN td.VBrand brand ON product.BrandId = brand.Id
ORDER BY summaryMetrics.Date DESC;