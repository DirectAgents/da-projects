
declare @startDate       NVARCHAR(MAX) = '01/01/2019'; -- start date
declare @endDate         NVARCHAR(MAX) = '03/31/2019'; -- end date
DECLARE @accId           INT           = 1437;                -- Account ID

SELECT 
	   [AccountId]
	  ,[Date]
      ,[Brand]
      ,[Campaign]
      ,[AdGroup]
	  ,[ASIN]
      
      ,[Impressions]
      ,[Clicks]  as "Clicks"
      ,[Cost]  as "Spend"
      
	  ,[AttributedSales14]     as "14 days total sales"
	  ,[AttributedSalesSameSKU14]  as "14 days total sales same SKU"
	  ,[AttributedSalesOtherSKU14]  as "14 days total sales other SKU"

	  ,[AttributedConversions14] as "14 days total orders"
      ,[AttributedConversionsSameSKU14]  as "14 days total orders same SKU"
      
      ,[AttributedUnitsOrdered14] as "14 days total units"
      ,[AttributedUnitsOrderedOtherSKU14]   as "14 days total units other SKU"
    
  FROM [td].[AmazonAsinAnalytic]
 Where AccountId = @accId  and Date between @startDate and @endDate
