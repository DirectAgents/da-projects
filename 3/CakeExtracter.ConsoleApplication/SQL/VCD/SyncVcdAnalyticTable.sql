DECLARE @accountId INT = @@param_0;
DECLARE @startDate DATETIME = '@@param_1';
DECLARE @endDate DATETIME = '@@param_2';

DELETE FROM td.VcdAnalytic
WHERE  accountid = @accountId
   AND date BETWEEN @startDate AND @endDate;

INSERT INTO td.VcdAnalytic
            (date,
             brandname,
             categoryname,
             subcategoryname,
             releasedate,
             asin,
             parentproductasin,
             NAME,
             accountid,
             ean,
             upc,
             apparelsize,
             apparelsizewidth,
             binding,
             color,
             modelstylenumber,
             shippedrevenue,
             shippedunits,
             orderedunits,
             shippedcogs,
             freereplacements,
             customerreturns,
             orderedrevenue,
             lbb,
             repoos,
             repoospercentoftotal,
             repoospriorperiodpercentchange)
SELECT summarymetrics.date AS 'Date - PRODUCT',
       brand.NAME          AS 'Brand',
       category.NAME       AS 'Category',
       subcategory.NAME    AS 'Subcategory',
       product.releasedate,
       product.asin        AS 'ASIN',
       parent.asin         AS 'Parent ASIN',
       product.NAME        AS 'Product Title',
       product.accountid,
       product.ean,
       product.upc,
       product.apparelsize,
       product.apparelsizewidth,
       product.binding,
       product.color,
       product.modelstylenumber,
       summarymetrics.[vendorShippedRevenue] AS 'Shipped Revenue',
       summarymetrics.[vendorShippedUnits] AS 'Shipped Units',
       summarymetrics.[vendorOrderedUnits] AS 'Ordered Units',
       summarymetrics.[vendorShippedCogs] AS 'Shipped COGS',
       summarymetrics.[vendorFreeReplacement] AS 'Free Replacements',
       summarymetrics.[vendorCustomerReturns] AS 'Customer Returns',
       summarymetrics.[vendorOrderedRevenue] AS 'Ordered Revenue',
       summaryMetrics.[vendorLostBuyBox] AS 'LBB',
       summaryMetrics.[vendorRepOos] AS 'Rep OOS',
       summaryMetrics.[vendorRepOosPercentOfTotal] AS 'Rep OOS - % Of Total',
       summaryMetrics.[vendorRepOosPriorPeriodPercentChange] AS 'Rep OOS - Prior Period'
FROM   (SELECT productSummaryMetric.date,
               productSummaryMetric.productid,
               metricType.Name AS 'MetricTypeName',
               productSummaryMetric.value
        FROM   td.VProductSummaryMetric productSummaryMetric
        JOIN   td.MetricType metricType
            ON productSummaryMetric.MetricTypeId = metricType.Id 
        WHERE  productSummaryMetric.date BETWEEN @startDate AND @endDate) AS
       metrics
       PIVOT ( Avg(metrics.value)
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
                     [vendorRepOosPriorPeriodPercentChange]
                 )
             ) AS summarymetrics
       JOIN td.VProduct product
         ON summarymetrics.productid = product.id
       JOIN td.VSubcategory subcategory
         ON product.subcategoryid = subcategory.id
       JOIN td.VCategory category
         ON subcategory.categoryid = category.id
       JOIN td.VParentProduct parent
         ON product.parentproductid = parent.id
       JOIN td.VBrand brand
         ON product.brandid = brand.id
            AND brand.accountid = @accountId
ORDER  BY summarymetrics.date DESC;