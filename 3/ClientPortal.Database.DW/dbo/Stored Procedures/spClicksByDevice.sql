
CREATE PROCEDURE dbo.spClicksByDevice
	 @advertiserId int
	,@date1 datetime
	,@date2 datetime
AS
	SELECT TOP (100) PERCENT 
		 dbo.DimDevice.DeviceName
		,COUNT(dbo.FactClick.ClickKey) AS ClickCount
	FROM
		dbo.FactClick INNER JOIN
		dbo.DimDevice ON dbo.FactClick.DeviceKey = dbo.DimDevice.DeviceKey
	WHERE
		(dbo.FactClick.DateKey BETWEEN @date1 AND @date2)
	GROUP BY 
		 dbo.FactClick.AdvertiserKey
		,dbo.DimDevice.DeviceName
	HAVING
		(dbo.FactClick.AdvertiserKey = @advertiserId)