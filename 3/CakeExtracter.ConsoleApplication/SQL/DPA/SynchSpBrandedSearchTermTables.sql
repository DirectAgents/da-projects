DECLARE @SynchDate DATE = (SELECT MAX([date]) FROM [dbo].[carhartt_ca_branded_sp_sov_brand_sv])

DELETE FROM [dbo].[carhartt_ca_branded_sp_sov_brand_sv]
WHERE [date] >= @SynchDate;

WITH sp_branded_with_sv
AS
(
	SELECT
		[sph].[phrase],
		[sph].[position],
		[sph].[brand],
		[sph].[datetime],
		[st].[search_volume]
	FROM [dbo].[carhartt_ca_branded_sp_sov_20201020] sph
	LEFT JOIN [dbo].[carhartt_branded_searchTerms_20201020] st ON [sph].[phrase] = [st].[phrase]
),
sp_branded_with_rn
AS
(
    SELECT
		[phrase],
		[position],
		[brand],
		[datetime],
		[search_volume],
       ROW_NUMBER() OVER (  
                             PARTITION BY  [phrase],
                                           [Datetime]
                             ORDER BY   [phrase] DESC,
                                        [datetime] ASC, 
                                        CAST([position] AS BIGINT) DESC
                         ) AS [rn]
    FROM sp_branded_with_sv
    WHERE [datetime] >= @SynchDate
),
sp_branded_with_rn_grouped
AS
(
    SELECT
        [datetime]				AS [datetime],
        [phrase]				AS [phrase],
        SUM(search_volume)		AS [search_volume],
        SUM(rn)					AS [rn_sum]
    FROM sp_branded_with_rn
    GROUP BY [datetime], [phrase]
),
sp_branded_with_wieghted_sv
AS
(
    SELECT
        [a].[datetime],
        [a].[brand],
        [a].[phrase],
        CAST([a].[search_volume] AS FLOAT) * (CAST([a].[rn] AS FLOAT) / CAST([b].[rn_sum] AS FLOAT)) AS [wieghted_sv]
    FROM sp_branded_with_rn AS a
    LEFT JOIN sp_branded_with_rn_grouped b ON   [a].[datetime] = [b].[datetime]
                                            AND [a].[phrase] = [b].[phrase]
)
INSERT INTO [dbo].[carhartt_ca_branded_sp_sov_brand_sv]
SELECT
    [phrase]						AS [search_term],
    CAST([datetime] AS DATE)		AS [date],
    COALESCE([brand], '')			AS [brand],
    SUM([wieghted_sv])				AS [wieghted_sv]
FROM sp_branded_with_wieghted_sv
GROUP BY    CAST([datetime] AS DATE),
            [phrase],
            [brand]
GO

DECLARE @SynchDate DATE = (SELECT MAX([date]) FROM [dbo].[carhartt_ca_branded_sp_sov_search_term_sv])

DELETE FROM [dbo].[carhartt_ca_branded_sp_sov_search_term_sv]
WHERE [date] >= @SynchDate;

WITH sp_term_sv
AS
(
    SELECT
        [sph].[datetime],
        [sph].[phrase],
        [st].[search_volume]
    FROM [dbo].[carhartt_ca_branded_sp_sov_20201020] sph
	LEFT JOIN [dbo].[carhartt_branded_searchTerms_20201020] st ON [sph].[phrase] = [st].[phrase]
    WHERE [datetime] >= @SynchDate
    GROUP BY    [sph].[datetime],
                [sph].[phrase],
                [st].[search_volume]
)
INSERT INTO [dbo].[carhartt_ca_branded_sp_sov_search_term_sv]
SELECT
    CAST([datetime] AS DATE)      AS [date],
    [phrase]                      AS [search_term],
    SUM(search_volume)            AS [search_volume]
FROM sp_term_sv
GROUP BY    CAST([datetime] AS DATE),
            [phrase]
GO