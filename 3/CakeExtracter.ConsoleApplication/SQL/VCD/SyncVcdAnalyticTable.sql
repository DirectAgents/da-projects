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
             orderedrevenue)
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
       summarymetrics.[52] AS 'Shipped Revenue',
       summarymetrics.[54] AS 'Shipped Units',
       summarymetrics.[53] AS 'Ordered Units',
       summarymetrics.[55] AS 'Shipped COGS',
       summarymetrics.[56] AS 'Free Replacements',
       summarymetrics.[57] AS 'Customer Returns',
       summarymetrics.[58] AS 'Ordered Revenue'
FROM   (SELECT productSummaryMetric.date,
               productSummaryMetric.productid,
               productSummaryMetric.metrictypeid,
               productSummaryMetric.value
        FROM   td.VProductSummaryMetric productSummaryMetric
        WHERE  productSummaryMetric.date BETWEEN @startDate AND @endDate) AS
       metrics
       PIVOT ( Avg(metrics.value)
             FOR metrics.metrictypeid IN ( [52],
                                           [54],
                                           [53],
                                           [55],
                                           [56],
                                           [57],
                                           [58] ) ) AS summarymetrics
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