DECLARE @SynchDate DATE = (SELECT MAX([date]) FROM [dbo].[carhartt_paid_non_branded_sp_brand_sv])

DELETE FROM [dbo].[carhartt_paid_non_branded_sp_brand_sv]
WHERE [date] >= @SynchDate;

WITH sp_branded_with_rn
AS
(
    SELECT
       [phrase],
       [index],
       [datetime],
       [title],
       [brand], 
       [impressions],
       ROW_NUMBER() OVER (  
                             PARTITION BY  [phrase],
                                           [Datetime]
                             ORDER BY   [phrase] DESC,
                                        [datetime] ASC, 
                                        CAST([index] AS BIGINT) DESC
                         ) AS [rn]
    FROM [dbo].[carhartt_paid_non_branded_sp_hourly_scraperApi]
    WHERE [Datetime] >= @SynchDate
),
sp_branded_with_rn_grouped
AS
(
    SELECT
        [datetime]          AS [datetime],
        [phrase]            AS [phrase],
        SUM(impressions)    AS [impressions],
        SUM(rn)             AS [rn_sum]
    FROM sp_branded_with_rn
    GROUP BY [Datetime], [phrase]
),
sp_branded_with_wieghted_sv
AS
(
    SELECT
        [a].[datetime],
        [a].[brand],
        [a].[phrase],
        CAST([a].[impressions] AS FLOAT) * (CAST([a].[rn] AS FLOAT) / CAST([b].[rn_sum] AS FLOAT)) AS [wieghted_sv]
    FROM sp_branded_with_rn AS a
    LEFT JOIN sp_branded_with_rn_grouped b ON   [a].[datetime] = [b].[datetime]
                                            AND [a].[phrase] = [b].[phrase]
)
INSERT INTO carhartt_paid_non_branded_sp_brand_sv
SELECT
    CAST([datetime] AS DATE)    AS [date],
    [phrase]                    AS [search_term],
    [Brand]                     AS [brand],
    SUM([wieghted_sv])          AS [wieghted_sv]
FROM sp_branded_with_wieghted_sv
GROUP BY    CAST([datetime] AS DATE),
            [phrase],
            [brand]
GO

DECLARE @SynchDate DATE = (SELECT MAX([date]) FROM [dbo].[carhartt_paid_non_branded_sp_search_term_sv])

DELETE FROM [dbo].[carhartt_paid_non_branded_sp_search_term_sv]
WHERE [date] >= @SynchDate;

WITH sp_term_sv
AS
(
    SELECT
        [datetime],
        [phrase],
        [impressions]
    FROM [dbo].[carhartt_paid_non_branded_sp_hourly_scraperApi]
    WHERE [datetime] >= @SynchDate
    GROUP BY    [datetime],
                [phrase],
                [impressions]
)
INSERT INTO carhartt_paid_non_branded_sp_search_term_sv
SELECT
    CAST([datetime] AS DATE)    AS [date],
    [phrase]                    AS [search_term],
    SUM(Impressions)            AS [search_volume]
FROM sp_term_sv
GROUP BY    CAST([datetime] AS DATE),
            [phrase]
GO