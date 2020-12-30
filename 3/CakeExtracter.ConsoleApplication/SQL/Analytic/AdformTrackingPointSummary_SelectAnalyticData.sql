DECLARE @AccountId      INT         = @@param_0,
        @StartDate      DATETIME    = '@@param_1',
        @EndDate        DATETIME    = '@@param_2';

SELECT
	tps.[Date]								 AS [Date],
	tps.[Date] -(datepart(dw, tps.Date)+5)%7 AS [StartDayOfWeek],
	c.AccountId								 AS [AccountId],
	tps.TrackingPointId						 AS [TrackingPointId],
	tp.[Name]								 AS [TrackingPointName],
	c.[Name]								 AS [CampaignName],
	tps.LineItemId							 AS [LineItemId],
	li.[Name]								 AS [LineItemName],
	tps.MediaTypeId							 AS [MediaTypeId],
	tps.ClickConversionsConvTypeAll			 AS [ClickConversionsConvTypeAll],
	tps.ClickConversionsConvType1			 AS [ClickConversionsConvType1],
	tps.ClickConversionsConvType2			 AS [ClickConversionsConvType2],
	tps.ClickConversionsConvType3			 AS [ClickConversionsConvType3],
	tps.ImpressionConversionsConvTypeAll	 AS [ImpressionConversionsConvTypeAll],
	tps.ImpressionConversionsConvType1		 AS [ImpressionConversionsConvType1],
	tps.ImpressionConversionsConvType2		 AS [ImpressionConversionsConvType2],
	tps.ImpressionConversionsConvType3		 AS [ImpressionConversionsConvType3],
	tps.ClickSalesConvTypeAll				 AS [ClickSalesConvTypeAll],
	tps.ClickSalesConvType1					 AS [ClickSalesConvType1],
	tps.ClickSalesConvType2					 AS [ClickSalesConvType2],
	tps.ClickSalesConvType3					 AS [ClickSalesConvType3],
	tps.ImpressionSalesConvTypeAll			 AS [ImpressionSalesConvTypeAll],
	tps.ImpressionSalesConvType1			 AS [ImpressionSalesConvType1],
	tps.ImpressionSalesConvType2			 AS [ImpressionSalesConvType2],
	tps.ImpressionSalesConvType3			 AS [ImpressionSalesConvType3]

FROM [td].[AdfTrackingPointSummary] tps
INNER JOIN [td].[AdfTrackingPoint] tp ON tps.TrackingPointId = tp.Id
LEFT JOIN [td].[AdfLineItem] li ON tps.LineItemId = li.Id
INNER JOIN [td].[AdfCampaign] c ON li.CampaignId = c.Id
WHERE c.AccountId = @AccountId
	AND tps.[Date] >= @StartDate
	AND tps.[Date] <= @EndDate
ORDER BY [Date],
		 [TrackingPointId],
		 [LineItemId],
		 [MediaTypeId]