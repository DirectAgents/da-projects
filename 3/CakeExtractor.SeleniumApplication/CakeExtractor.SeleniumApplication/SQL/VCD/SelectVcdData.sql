﻿declare @startDate       NVARCHAR(MAX) = '01/01/2019'; -- start date
declare @endDate         NVARCHAR(MAX) = '03/31/2019'; -- end date


SELECT TOP(20000) [Id]
      ,[Date]
      ,[Asin]
      ,[CategoryName]
      ,[SubcategoryName]
      ,[ParentProductAsin]
      ,[BrandName]
      ,[Ean]
      ,[Upc]
      ,[ApparelSize]
      ,[ApparelSizeWidth]
      ,[Binding]
      ,[Color]
      ,[ModelStyleNumber]
      ,[ReleaseDate]
      ,[Name]
      ,[AccountId]
      ,[ShippedRevenue]
      ,[ShippedUnits]
      ,[OrderedUnits]
      ,[ShippedCOGS]
      ,[FreeReplacements]
      ,[CustomerReturns]
      ,[OrderedRevenue]
  FROM [td].[VcdAnalytic]
  Where AccountId = 1445 and Date between @startDate and @endDate