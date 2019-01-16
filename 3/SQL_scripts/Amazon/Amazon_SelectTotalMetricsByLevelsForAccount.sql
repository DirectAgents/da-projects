/*
This script helps to extract account statistics (totals) for the corresponding date range by levels:
campaigns, ad groups, product ads, keywords, search terms.

Before execution, please set the following values:
- accId (ID of the target account in the database)
- startDate (statistics start date for extraction)
- endDate (statistics end date for extraction)
- campaignType (target campaign type for statistics dimensions, you can use constants below as campaign types and summarize them if you need to include many types of campaigns in the results)

NOTE: The Metric types IDs used in this script may vary depending on the target database.
*/

DECLARE @CampaignTypeSp NVARCHAR(MAX) = 'SponsoredProducts';
DECLARE @CampaignTypeSb NVARCHAR(MAX) = 'SponsoredBrands';
DECLARE @CampaignTypePd NVARCHAR(MAX) = 'ProductDisplay';

/* Account IDs:
1362  AvoDerm
1433  Belkin
1437  Carhartt Sportswear - Mens
1438  Carhartt Women's Collection
1434  Linksys Invoices
1439  Living Fresh
1423  Walker & Company
1436  Walker & Company TEST
1435  Wemo Invoice
*/

DECLARE @accId        INT           = 1437;                                                -- Account ID
DECLARE @startDate    NVARCHAR(MAX) = '1/13/2019';                                         -- Metrics start date
DECLARE @endDate      NVARCHAR(MAX) = '1/13/2019';                                         -- Metrics end date
DECLARE @campaignType NVARCHAR(MAX) = @CampaignTypeSp + @CampaignTypeSb + @CampaignTypePd; -- Campaign type

-----------------------------------------------------------------------------------------------------------------------------

/* Metric types IDs (in DB - real name):
1		attributedConversions1d	(Total Orders - 1 day) - for SP
2		attributedConversions7d	(Total Orders - 7 days) - for SP
3		attributedConversions14d (Total Orders - 14 days) - for SP and SB
4		attributedConversions30d (Total Orders - 30 days) - for SP
5		attributedConversionsSameSKU1d (Total Orders same SKU - 1 day) - for SP
6		attributedConversionsSameSKU7d (Total Orders same SKU - 7 days) - for SP
7		attributedConversionsSameSKU14d (Total Orders same SKU - 14 days) - for SP and SB
8		attributedConversionsSameSKU30d (Total Orders same SKU - 30 days) - for SP
9		attributedSales1d (Total Sales - 1 day) - for SP
10		attributedSales7d (Total Sales - 7 days) - for SP
11		attributedSales14d (Total Sales - 14 days) - for SP, SB and PD
12		attributedSales30d (Total Sales - 30 days) - for SP
13		attributedSalesSameSKU1d (Total Sales same SKU - 1 day) - for SP
14		attributedSalesSameSKU7d (Total Sales same SKU - 7 days) - for SP
15		attributedSalesSameSKU14d (Total Sales same SKU - 14 days) - for SP and SB
16		attributedSalesSameSKU30d (Total Sales same SKU - 30 days) - for SP
17		attributedUnitsOrdered1d (Total Units - 1 day) - for SP
18		attributedUnitsOrdered7d (Total Units - 7 days) - for SP
19		attributedUnitsOrdered14d (Total Units - 14 days) - for SP
20		attributedUnitsOrdered30d (Total Units - 30 days) - for SP
47		attributedUnitsOrdered1dOtherSKU (Number of other ASIN (SKU) - 1 day) - for SP, ASIN level
48		attributedUnitsOrdered7dOtherSKU (Number of other ASIN (SKU) - 7 days) - for SP, ASIN level
49		attributedUnitsOrdered14dOtherSKU (Number of other ASIN (SKU) - 14 days) - for SP, ASIN level
50		attributedUnitsOrdered30dOtherSKU (Number of other ASIN (SKU) - 30 days) - for SP, ASIN level
43		attributedSales1dOtherSKU (Total sales for another ASIN (SKU) - 1 day) - for SP, ASIN level
44		attributedSales7dOtherSKU (Total sales for another ASIN (SKU) - 7 days) - for SP, ASIN level
45		attributedSales14dOtherSKU (Total sales for another ASIN (SKU) - 14 days) - for SP, ASIN level
46		attributedSales30dOtherSKU (Total sales for another ASIN (SKU) - 30 days) - for SP, ASIN level
41		DetailPageViews (Detail Page Views) - for PD, more info (?)
42		UnitsSold (Units Ordered - 14 days) - for PD, correspond to attributedUnitsOrdered14dOtherSKU from API (?)
43		Orders (Total Orders - 14 days) - for PD, corresponds to attributedConversions14d
*/

--Strategy
SELECT
  CONCAT(@startDate, ' - ', @endDate) AS 'Date - CAMPAIGNS',
  SUM(Cost) AS 'Total Cost',
  SUM(Impressions) AS 'Total Impressions',
  SUM(Clicks) AS 'Total Clicks',
  SUM([metrics].[3]) AS 'Total Orders (14d)',
  SUM([metrics].[11]) AS 'Total Sales (14d)',
  SUM([metrics].[19]) AS 'Total Units (14d)',
  SUM([metrics].[7]) AS 'Total Orders same SKU (14d)',
  SUM([metrics].[15]) AS 'Total Sales same SKU (14d)',
  SUM([metrics].[41]) AS 'Detail Page Views - PDA',
  SUM([metrics].[42]) AS 'Units Ordered (14d) - PDA',
  SUM([metrics].[43]) AS 'Total Orders (14d) - PDA'
FROM
  [td].[StrategySummary] AS [summaries]
  JOIN [td].[Strategy] AS [items]
	ON [items].[Id] = [summaries].[StrategyId]
  LEFT JOIN (
	SELECT *
	FROM [td].[StrategySummaryMetric] AS [ungrouppedMetrics]
	PIVOT (
	  AVG([ungrouppedMetrics].Value) FOR [ungrouppedMetrics].MetricTypeId in (
		[3], [7], [11], [15], [19], [41], [42], [43]
		)
	) [ungrouppedPivotMetrics]
  ) AS [metrics]
	ON [metrics].[StrategyId] = [summaries].[StrategyId] and [metrics].[Date] = [summaries].[Date]
WHERE
  [AccountId] = @accId and
  [summaries].[Date] >= @startDate and [summaries].[Date] <= @endDate and
  [items].[TypeId] in (
    SELECT
      [type].[Id]
    FROM
      [td].[Type] AS [type]
    WHERE
      @campaignType LIKE '%' + [type].[Name] + '%'
  )

-- AdSet
IF (@campaignType LIKE '%' + @CampaignTypeSp + '%') or (@campaignType LIKE '%' + @CampaignTypeSb + '%')
	SELECT
	  CONCAT(@startDate, ' - ', @endDate) AS 'Date - AD GROUPS',
	  SUM(Cost) AS 'Total Cost',
	  SUM(Impressions) AS 'Total Impressions',
	  SUM(Clicks) AS 'Total Clicks',
	  SUM([metrics].[3]) AS 'Total Orders (14d)',
	  SUM([metrics].[11]) AS 'Total Sales (14d)',
	  SUM([metrics].[19]) AS 'Total Units (14d)',
	  SUM([metrics].[7]) AS 'Total Orders same SKU (14d)',
	  SUM([metrics].[15]) AS 'Total Sales same SKU (14d)'
	FROM
	  [td].[AdSetSummary] AS [summaries]
	  JOIN [td].[AdSet] AS [items]
		ON [items].[Id] = [summaries].[AdSetId]
	  JOIN [td].[Strategy]
		ON [td].[Strategy].[Id] = [items].[StrategyId]
	  LEFT JOIN (
		SELECT *
		FROM [td].[AdSetSummaryMetric] AS [ungrouppedMetrics]
		PIVOT (
		  AVG([ungrouppedMetrics].Value) FOR [ungrouppedMetrics].MetricTypeId in (
			[3], [7], [11], [15], [19]
			)
		) [ungrouppedPivotMetrics]
	  ) AS [metrics]
		ON [metrics].[AdSetId] = [summaries].[AdSetId] and [metrics].[Date] = [summaries].[Date]
	WHERE
	  [items].[AccountId] = @accId and
	  [summaries].[Date] >= @startDate and [summaries].[Date] <= @endDate and
	  [td].[Strategy].[TypeId] in (
		SELECT
		  [types].[Id]
		FROM
		  [td].[Type] AS [types]
		WHERE
		  @campaignType LIKE '%' + [types].[Name] + '%'
	  )

-- Creative (Ad)
IF (@campaignType LIKE '%' + @CampaignTypeSp + '%')
	SELECT
	  CONCAT(@startDate, ' - ', @endDate) AS 'Date - PRODUCT ADS (ASINS)',
	  SUM(Cost) AS 'Total Cost',
	  SUM(Impressions) AS 'Total Impressions',
	  SUM(Clicks) AS 'Total Clicks',
	  SUM([metrics].[3]) AS 'Total Orders (14d)',
	  SUM([metrics].[11]) AS 'Total Sales (14d)',
	  SUM([metrics].[19]) AS 'Total Units (14d)',
	  SUM([metrics].[7]) AS 'Total Orders same SKU (14d)',
	  SUM([metrics].[15]) AS 'Total Sales same SKU (14d)',
	  SUM([metrics].[49]) AS 'Number of other SKU (14d)',
	  SUM([metrics].[45]) AS 'Total sales other SKU (14d)'
	FROM
	  [td].[AdSummary] AS [summaries]
	  JOIN [td].[Ad] AS [items]
		ON [items].[Id] = [summaries].[TDadId]
	  LEFT JOIN (
		SELECT *
		FROM [td].[AdSummaryMetric] AS [ungrouppedMetrics]
		PIVOT (
		  AVG([ungrouppedMetrics].Value) FOR [ungrouppedMetrics].MetricTypeId in (
			[3], [7], [11], [15], [19], [49], [45]
			)
		) [ungrouppedPivotMetrics]
	  ) AS [metrics]
		ON [metrics].[TDadId] = [summaries].[TDadId] and [metrics].[Date] = [summaries].[Date]
	WHERE
	  [items].[AccountId] = @accId and
	  [summaries].[Date] >= @startDate and [summaries].[Date] <= @endDate

-- Keyword
IF (@campaignType LIKE '%' + @CampaignTypeSp + '%') or (@campaignType LIKE '%' + @CampaignTypeSb + '%')
	SELECT
	  CONCAT(@startDate, ' - ', @endDate) AS 'Date - KEYWORDS',
	  SUM(Cost) AS 'Total Cost',
	  SUM(Impressions) AS 'Total Impressions',
	  SUM(Clicks) AS 'Total Clicks',
	  SUM([metrics].[3]) AS 'Total Orders (14d)',
	  SUM([metrics].[11]) AS 'Total Sales (14d)',
	  SUM([metrics].[19]) AS 'Total Units (14d)',
	  SUM([metrics].[7]) AS 'Total Orders same SKU (14d)',
	  SUM([metrics].[15]) AS 'Total Sales same SKU (14d)'
	FROM
	  [td].[KeywordSummary] AS [summaries]
	  JOIN [td].[Keyword] AS [items]
		ON [items].[Id] = [summaries].[KeywordId]
	  JOIN [td].[Strategy]
		ON [td].[Strategy].[Id] = [items].[StrategyId]
	  LEFT JOIN (
		SELECT *
		FROM [td].[KeywordSummaryMetric] AS [ungrouppedMetrics]
		PIVOT (
		  AVG([ungrouppedMetrics].Value) FOR [ungrouppedMetrics].MetricTypeId in (
			[3], [7], [11], [15], [19]
			)
		) [ungrouppedPivotMetrics]
	  ) AS [metrics]
		ON [metrics].[KeywordId] = [summaries].[KeywordId] and [metrics].[Date] = [summaries].[Date]
	WHERE
	  [items].[AccountId] = @accId and
	  [summaries].[Date] >= @startDate and [summaries].[Date] <= @endDate and
	  [td].[Strategy].[TypeId] in (
		SELECT
		  [type].[Id]
		FROM
		  [td].[Type] AS [type]
		WHERE
		  @campaignType LIKE '%' + [type].[Name] + '%'
	  )

-- Searct term
IF (@campaignType LIKE '%' + @CampaignTypeSp + '%')
	SELECT
	  CONCAT(@startDate, ' - ', @endDate) AS 'Date - SEARCH TERMS',
	  SUM(Cost) AS 'Total Cost',
	  SUM(Impressions) AS 'Total Impressions',
	  SUM(Clicks) AS 'Total Clicks',
	  SUM([metrics].[3]) AS 'Total Orders (14d)',
	  SUM([metrics].[11]) AS 'Total Sales (14d)',
	  SUM([metrics].[19]) AS 'Total Units (14d)',
	  SUM([metrics].[7]) AS 'Total Orders same SKU (14d)',
	  SUM([metrics].[15]) AS 'Total Sales same SKU (14d)'
	FROM
	  [td].[SearchTermSummary] AS [summaries]
	  JOIN [td].[SearchTerm] AS [items]
		ON [items].[Id] = [summaries].[SearchTermId]
	  LEFT JOIN (
		SELECT *
		FROM [td].[SearchTermSummaryMetric] AS [ungrouppedMetrics]
		PIVOT (
		  AVG([ungrouppedMetrics].Value) FOR [ungrouppedMetrics].MetricTypeId in (
			[3], [7], [11], [15], [19]
			)
		) [ungrouppedPivotMetrics]
	  ) AS [metrics]
		ON [metrics].[SearchTermId] = [summaries].[SearchTermId] and [metrics].[Date] = [summaries].[Date]
	WHERE
	  [items].[AccountId] = @accId and
	  [summaries].[Date] >= @startDate and [summaries].[Date] <= @endDate