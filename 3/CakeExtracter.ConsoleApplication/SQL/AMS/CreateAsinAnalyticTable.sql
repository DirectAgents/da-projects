CREATE TABLE [td].[AmazonAsinAnalytic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[ASIN] [nvarchar](20) NULL,
	[Brand] [nvarchar](100) NULL,
	[Campaign] [nvarchar](200) NULL,
	[AdGroup] [nvarchar](200) NULL,
	[AccountId] [int] NULL,
	
	[Impressions] [int]  NULL,
	[Clicks] [int]  NULL,
	[PostClickConv] [int] NULL,
	[PostViewConv] [int] NULL,
	[Cost] [decimal](18, 6) NULL,
	[AllClicks] [int] NULL,
	
	[AttributedConversions1] [decimal](18, 6)  NULL,
	[AttributedConversions7] [decimal](18, 6)  NULL,
	[AttributedConversions14] [decimal](18, 6)  NULL,
	[AttributedConversions30] [decimal](18, 6)  NULL,
	
	[AttributedConversionsSameSKU1] [decimal](18, 6)  NULL,
	[AttributedConversionsSameSKU7] [decimal](18, 6)  NULL,
	[AttributedConversionsSameSKU14] [decimal](18, 6)  NULL,
	[AttributedConversionsSameSKU30] [decimal](18, 6)  NULL,
	
	[AttributedSales1] [decimal](18, 6)  NULL,
	[AttributedSales7] [decimal](18, 6)  NULL,
	[AttributedSales14] [decimal](18, 6)  NULL,
	[AttributedSales30] [decimal](18, 6)  NULL,
	
	[AttributedSalesSameSKU1] [decimal](18, 6)  NULL,
	[AttributedSalesSameSKU7] [decimal](18, 6)  NULL,
	[AttributedSalesSameSKU14] [decimal](18, 6)  NULL,
	[AttributedSalesSameSKU30] [decimal](18, 6)  NULL,
	
	[AttributedUnitsOrdered1] [decimal](18, 6)  NULL,
	[AttributedUnitsOrdered7] [decimal](18, 6)  NULL,
	[AttributedUnitsOrdered14] [decimal](18, 6)  NULL,
	[AttributedUnitsOrdered30] [decimal](18, 6)  NULL,
	
	[AttributedSalesOtherSKU1] [decimal](18, 6)  NULL,
	[AttributedSalesOtherSKU7] [decimal](18, 6)  NULL,
	[AttributedSalesOtherSKU14] [decimal](18, 6)  NULL,
	[AttributedSalesOtherSKU30] [decimal](18, 6)  NULL,
	
	[AttributedUnitsOrderedOtherSKU1] [decimal](18, 6)  NULL,
	[AttributedUnitsOrderedOtherSKU7] [decimal](18, 6)  NULL,
	[AttributedUnitsOrderedOtherSKU14] [decimal](18, 6)  NULL,
	[AttributedUnitsOrderedOtherSKU30] [decimal](18, 6)  NULL
	)

