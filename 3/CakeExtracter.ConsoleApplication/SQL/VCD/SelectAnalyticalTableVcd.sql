

declare @startDate       NVARCHAR(MAX) = '01/01/2019'; -- start date
declare @endDate         NVARCHAR(MAX) = '03/31/2019'; -- end date
declare @accountId       int = 1445                   -- account id
/*
1451 Belkin Canada
1452 Belkin Inc.
1445 Carhartt Sportswear Mens
1450 Linksys
1444 LINY9-Linksys
1453 Wemo

*/



SELECT [Date]   as 'Date - PRODUCT'
	  ,[BrandName] as 'Brand'
      ,[CategoryName] as 'Category'
      ,[SubcategoryName]  as 'Subcategory'
      ,[ParentProductAsin]
	  ,[Asin] as 'ASIN'
      ,[Ean] 
      ,[Upc]
      ,[ApparelSize]
      ,[ApparelSizeWidth]
      ,[Binding]
      ,[Color]
      ,[ModelStyleNumber]
      ,[ReleaseDate]
      ,[Name]  as 'Product Title'
      
      ,[ShippedRevenue]  as 'Shipped Revenue'
      ,[ShippedUnits] as 'Shipped Units'
      ,[OrderedUnits] as 'Ordered Units'
      ,[ShippedCOGS] as 'Shipped COGS'
      ,[FreeReplacements] as 'Free Replacements'
      ,[CustomerReturns] as 'Customer Returns'
      ,[OrderedRevenue] as 'Ordered Revenue'
  FROM [td].[VcdAnalytic]
  Where AccountId = @accountId and Date between @startDate and @endDate


