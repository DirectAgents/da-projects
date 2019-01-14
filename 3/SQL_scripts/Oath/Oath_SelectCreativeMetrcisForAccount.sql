/*
This script helps to extract account Yahoo statistics for the corresponding date range by across the hierarchy: 
date > campaign > line > creative
If a creative record does not have only one line as its parent, then the “Line (Strategy)” column will display the value “- Many Lines -”.
If a creative record does not have only one campaign as its parent, then the “Campaign (AdSet)” column will display the value “- Many Campaigns -”.

Before execution, please set the following values:
- accId (ID of the target account in the database)
- startDate (statistics start date for extraction)
- endDate (statistics end date for extraction)
*/

/* Account IDs (active)
1390	Avoderm
1327	BritBox
1419	Course Hero
1378	Funimation
1420	LendKey
1323	Ouidad
142		Walker and Company
1424	Walker and Company Test
*/
DECLARE @accId INT = 142;												-- Account ID
DECLARE @startDate NVARCHAR(MAX) = '12.1.2018';							-- Metrics start date
DECLARE @endDate NVARCHAR(MAX) = '12.1.2018';							-- Metrics end date

------------------------------------------------------------------------------------------------

DECLARE @yamPlatformId INT = 16;

SELECT
	keyw_s.Date,
	acc.Name AS 'Account',
	CASE WHEN ads.Name != ''
		THEN CONCAT(ads.Name, ' (', ads.ExternalId, ')')
		ELSE '- Many Campaigns -' END
		AS 'Campaign (Adset)',
	CASE WHEN strat.Name != ''
		THEN CONCAT(strat.Name, ' (', strat.ExternalId, ')')
		ELSE '- Many Lines -' END
		AS 'Line (Strategy)',
	CONCAT(keyw.Name, ' (', keyw.ExternalId, ')') AS 'Creative (Keyword)',
	keyw_s.Impressions,
	keyw_s.Clicks,
	keyw_s.Cost AS 'Advertising Spending (Cost)',
	keyw_s.PostClickConv AS 'Click Through Conversion (PostClickConv)',
	keyw_s.PostViewConv AS 'View Through Conversion (PostViewConv)',
	keyw_s.PostClickRev AS 'Conversion (PostClickRev)',
	keyw_s.PostViewRev AS 'Conversion Value (PostViewRev)'
FROM [td].[KeywordSummary] AS keyw_s
LEFT JOIN [td].[Keyword] AS keyw
	ON keyw.Id = keyw_s.KeywordId
LEFT JOIN [td].[Strategy] AS strat
	ON strat.Id = keyw.StrategyId
LEFT JOIN [td].[AdSet] AS ads
	ON ads.Id = keyw.AdSetId
JOIN [td].[Account] AS acc
	ON keyw.AccountId = acc.Id
--WHERE acc.PlatformId = @yamPlatformId and acc.Disabled = 0
WHERE 
	acc.Id = @accId and 
	keyw_s.Date >= @startDate and keyw_s.Date <= @endDate
ORDER BY keyw_s.Date desc, acc.Id, ads.Id, strat.Id

--SELECT
--	ads.*,
--	ads_s.*
--FROM [td].Adset AS ads
--JOIN [td].[Account] AS acc
--	ON ads.AccountId = acc.Id
--left JOIN [td].[AdSetSummary] AS ads_s
--	ON ads_s.AdSetId = ads.Id
--WHERE acc.Id = @accId and Date >= @startDate and Date <= @endDate

--SELECT
--	ads.*,
--	ads_s.*
--FROM [td].Strategy AS ads
--JOIN [td].[Account] AS acc
--	ON ads.AccountId = acc.Id
--left JOIN [td].StrategySummary AS ads_s
--	ON ads_s.StrategyId = ads.Id
--WHERE acc.Id = @accId and Date >= @startDate and Date <= @endDate

--SELECT
--	ads.*,
--	ads_s.*
--FROM [td].Keyword AS ads
--JOIN [td].[Account] AS acc
--	ON ads.AccountId = acc.Id
--left JOIN [td].KeywordSummary AS ads_s
--	ON ads_s.KeywordId = ads.Id
--WHERE acc.Id = @accId and Date >= @startDate and Date <= @endDate
