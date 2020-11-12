DECLARE @SynchDate DATE = (SELECT MAX([date]) FROM [dbo].[carhartt_paid_branded_sb_brand_sv])

DELETE FROM [dbo].[carhartt_paid_branded_sb_brand_sv]
WHERE [date] >= @SynchDate

INSERT INTO [dbo].[carhartt_paid_branded_sb_brand_sv]
SELECT 
    [sbh].[phrase]              AS [search_term],
    [sbh].[date]                AS [date],
    [sbh].[HSAsponsor]          AS [brand],
    SUM([sbh].[Impressions])    AS [search_volume]
FROM [dbo].[carhartt_paid_branded_sb_hourly_scraperApi] sbh
WHERE [Datetime] >= @SynchDate
GROUP BY    [sbh].[phrase],
            [sbh].[date],
            [sbh].[HSAsponsor]
GO

DECLARE @SynchDate DATE = (SELECT MAX([date]) FROM [dbo].[carhartt_paid_branded_sb_search_term_sv])

DELETE FROM [dbo].[carhartt_paid_branded_sb_search_term_sv]
WHERE [date] >= @SynchDate

INSERT INTO [dbo].[carhartt_paid_branded_sb_search_term_sv]
SELECT 
    [sbh].[phrase]              AS [search_term],
    [sbh].[date]                AS [date],
    SUM([sbh].[Impressions])    AS [search_volume]
FROM [dbo].[carhartt_paid_branded_sb_hourly_scraperApi] sbh
WHERE [Datetime] >= @SynchDate
GROUP BY    [sbh].[phrase],
            [sbh].[date]
GO