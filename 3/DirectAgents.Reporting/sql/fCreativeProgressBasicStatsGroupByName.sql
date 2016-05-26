ALTER FUNCTION td.fCreativeProgressBasicStatsGroupByName(
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
, @KPI varchar(max)
) RETURNS TABLE AS RETURN

WITH dup AS
(
	SELECT *
	, ROW_NUMBER() OVER (PARTITION BY AdName ORDER BY AdId DESC) as RowNum
	FROM td.fCreativeProgressBasicStats(@AdvertiserId, @StartDate, @EndDate, @KPI)
)
SELECT grp.AdName, adId.AdId, adId.ShowClickAndViewConv, COUNT(grp.AdName) AS DupNameCount
	, SUM(grp.MediaSpend) AS MediaSpend
	, SUM(grp.Impressions) AS Impressions
	, SUM(grp.Clicks) AS Clicks
	, CASE WHEN SUM(grp.Impressions) = 0 THEN 0 ELSE SUM(grp.Clicks) / CAST(SUM(grp.Impressions) as float) END AS CTR
	, CASE WHEN SUM(grp.Conversions) = 0 THEN 0 ELSE SUM(grp.MediaSpend) / CAST(SUM(grp.Conversions) as float) END AS eCPA
	, SUM(grp.PostClickConv) AS PostClickConv
	, SUM(grp.PostViewConv) AS PostViewConv
	, SUM(grp.Conversions) AS Conversions
	, CASE WHEN SUM(grp.Clicks) = 0 THEN 0 ELSE SUM(grp.MediaSpend) / CAST(SUM(grp.Clicks) as float) END AS eCPC
	, CASE @KPI
			WHEN 'Impressions' THEN SUM(grp.Impressions)
			WHEN 'Clicks' THEN SUM(grp.Clicks)
			WHEN 'CTR' THEN CASE WHEN SUM(grp.Impressions) = 0 THEN 0 ELSE SUM(grp.Clicks) / CAST(SUM(grp.Impressions) AS float) END
			WHEN 'Conversions' THEN SUM(grp.Conversions)
			WHEN 'CR' THEN CASE WHEN SUM(grp.Clicks) = 0 THEN 0 ELSE SUM(grp.Conversions) / CAST(SUM(grp.Clicks) as float) END
			WHEN 'eCPC' THEN CASE WHEN SUM(grp.Clicks) = 0 THEN 0 ELSE SUM(grp.MediaSpend) / CAST(SUM(grp.Clicks) as float) END
			WHEN 'eCPA' THEN CASE WHEN SUM(grp.Conversions) = 0 THEN 0 ELSE SUM(grp.MediaSpend) / CAST(SUM(grp.Conversions) as float) END
		END AS SumKPI
FROM dup grp
INNER JOIN dup adId
  ON (adId.AdName = grp.AdName)
 AND (adId.RowNum = 1)
GROUP BY grp.AdName, adId.AdId, adId.ShowClickAndViewConv