DECLARE @accountId INT = @@param_0;
DECLARE @startDate DATETIME = '@@param_1';
DECLARE @endDate DATETIME = '@@param_2';

DELETE FROM td.VcdAnalytic
WHERE accountid = @accountId AND date BETWEEN @startDate AND @endDate;

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
    [LBB],
    [RepOos],
    [RepOosPercentOfTotal],
    [RepOosPriorPeriodPercentChange],
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
    summarymetrics.[Date]                                   AS 'Date - PRODUCT',
    brand.[Name]                                            AS 'Brand',
    category.[Name]                                         AS 'Category',
    subcategory.[Name]                                      AS 'Subcategory',
    product.[ReleaseDate],
    product.[Asin]                                          AS 'ASIN',
    parent.[Asin]                                           AS 'Parent ASIN',
    product.[Name]                                          AS 'Product Title',
    product.[AccountId],
    product.[Ean],
    product.[Upc],
    product.[ApparelSize],
    product.[ApparelSizeWidth],
    product.[Binding],
    product.[Color],
    product.[ModelStyleNumber],
    summarymetrics.[vendorShippedRevenue]                   AS 'Shipped Revenue',
    summarymetrics.[vendorShippedUnits]                     AS 'Shipped Units',
    summarymetrics.[vendorOrderedUnits]                     AS 'Ordered Units',
    summarymetrics.[vendorShippedCogs]                      AS 'Shipped COGS',
    summarymetrics.[vendorFreeReplacement]                  AS 'Free Replacements',
    summarymetrics.[vendorCustomerReturns]                  AS 'Customer Returns',
    summarymetrics.[vendorOrderedRevenue]                   AS 'Ordered Revenue',
    summaryMetrics.[vendorLostBuyBox]                       AS 'LBB',
    summaryMetrics.[vendorRepOos]                           AS 'Rep OOS',
    summaryMetrics.[vendorRepOosPercentOfTotal]             AS 'Rep OOS - % Of Total',
    summaryMetrics.[vendorRepOosPriorPeriodPercentChange]   AS 'Rep OOS - Prior Period',
	summaryMetrics.[gvcurrenttotal]                         AS 'Glance Views',
    summaryMetrics.[subcategoryrank]                        AS 'Sales Rank',
    summaryMetrics.[averagesellingpriceshippedunits]        AS 'Average Sales Price',
    summaryMetrics.[sellableonhandunits]                    AS 'Sellable On Hand Units',
    summaryMetrics.[numberofcustomerreviews]                AS 'Number Of Customer Reviews',
    summaryMetrics.[numberofcustomerreviewslifetodate]      AS 'Number Of Customer Reviews Life To Date',
    summaryMetrics.[averagecustomerrating]                  AS 'Average Customer Rating',
    summaryMetrics.[fivestars]                              AS '5 Stars',
    summaryMetrics.[fourstars]                              AS '4 Stars',
    summaryMetrics.[threestars]                             AS '3 Stars',
    summaryMetrics.[twostars]                               AS '2 Stars',
    summaryMetrics.[onestar]                                AS '1 Star'
FROM
(
    SELECT
        productSummaryMetric.[Date]         AS 'Date',
        productSummaryMetric.[ProductId]    AS 'ProductId',
        metricType.[Name]                   AS 'MetricTypeName',
        productSummaryMetric.[Value]        AS 'Value'
    FROM td.VProductSummaryMetric productSummaryMetric
    INNER JOIN td.MetricType metricType ON productSummaryMetric.MetricTypeId = metricType.Id 
    WHERE productSummaryMetric.date BETWEEN @startDate AND @endDate
) AS metrics
PIVOT
(
    AVG(metrics.value)
    FOR metrics.MetricTypeName IN
    (
        [vendorShippedRevenue],
        [vendorShippedUnits],
        [vendorOrderedUnits],
        [vendorShippedCogs],
        [vendorFreeReplacement],
        [vendorCustomerReturns],
        [vendorOrderedRevenue],
        [vendorLostBuyBox],
        [vendorRepOos],
        [vendorRepOosPercentOfTotal],
        [vendorRepOosPriorPeriodPercentChange],
		[gvcurrenttotal],
        [subcategoryrank],
        [averagesellingpriceshippedunits],
        [sellableonhandunits],
        [numberofcustomerreviews],
        [numberofcustomerreviewslifetodate],
        [averagecustomerrating],
        [fivestars],
        [fourstars],
        [threestars],
        [twostars],
        [onestar]
    )
) AS summarymetrics
INNER JOIN td.VProduct product ON summarymetrics.productid = product.id
INNER JOIN td.VSubcategory subcategory ON product.subcategoryid = subcategory.id
INNER JOIN td.VCategory category ON subcategory.categoryid = category.id
INNER JOIN td.VParentProduct parent ON product.parentproductid = parent.id
INNER JOIN td.VBrand brand ON product.brandid = brand.id AND brand.accountid = @accountId
ORDER BY summarymetrics.date DESC;