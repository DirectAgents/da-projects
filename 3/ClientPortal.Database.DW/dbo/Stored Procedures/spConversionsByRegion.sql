
CREATE PROCEDURE [dbo].[spConversionsByRegion]
	 @advertiserId int
	,@date1 datetime
	,@date2 datetime
AS
SELECT TOP (100) PERCENT 
	 dbo.DimRegion.RegionCode
	,COUNT(dbo.FactClick.ClickKey) AS ClickCount
FROM 
	dbo.FactClick INNER JOIN
    dbo.DimRegion ON dbo.FactClick.RegionKey = dbo.DimRegion.RegionKey INNER JOIN
    dbo.FactConversion ON dbo.FactClick.ClickKey = dbo.FactConversion.ClickKey
WHERE
	(dbo.FactClick.DateKey BETWEEN @date1 AND @date2) AND 
    (dbo.DimRegion.RegionCode IN (N'AL', N'AK', N'AZ', N'AR', N'CA', N'CO', N'CT', N'DE', N'DC', N'FL', 
								  N'GA', N'HI', N'ID', N'IL', N'IN', N'IA', N'KS', N'KY', N'LA', N'ME', 
								  N'MD', N'MA', N'MI', N'MN', N'MS', N'MO', N'MT', N'NE', N'NV', N'NH', 
								  N'NJ', N'NM', N'NY', N'NC', N'ND', N'OH', N'OK', N'OR', N'PA', N'RI', 
								  N'SC', N'SD', N'TN', N'TX', N'UT', N'VT', N'VA', N'WA', N'WV', N'WI', 
								  N'WY'))
GROUP BY 
	 dbo.FactClick.AdvertiserKey
	,dbo.DimRegion.RegionCode
HAVING
	(dbo.FactClick.AdvertiserKey = @advertiserId)