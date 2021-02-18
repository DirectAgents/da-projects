DECLARE @SynchDate DATE = (SELECT MAX(CONVERT(DATE, [datetime])) FROM [dbo].[carhartt_ca_non_branded_sb_sov_brand_sv])

DELETE FROM [dbo].[carhartt_ca_non_branded_sb_sov_brand_sv]
WHERE [date] >= @SynchDate;

INSERT INTO [dbo].[carhartt_ca_non_branded_sb_sov_brand_sv]
SELECT TOP(10)
	[sbh].[phrase]						AS [search_term],
	[sbh].[adtype]						AS [adtype],
	CONVERT(DATE, [sbh].[datetime])		AS [date],
	[sbh].[brand]						AS [brand],
	SUM([st].[search_volume])			AS [search_volume]
FROM [dbo].[carhartt_ca_non_branded_sb_sov_20201020] sbh
LEFT JOIN [dbo].[carhartt_non_branded_searchTerms_20201020] st ON [sbh].[phrase] = [st].[phrase]
WHERE [sbh].[datetime] >= @SynchDate
GROUP BY	[sbh].[phrase],
			[sbh].[adtype],
			CONVERT(DATE, [sbh].[datetime]),
			[sbh].[brand]
GO

DECLARE @SynchDate DATE = (SELECT MAX(CONVERT(DATE, [datetime])) FROM [dbo].[carhartt_ca_non_branded_sb_sov_search_term_sv])

DELETE FROM [dbo].[carhartt_ca_non_branded_sb_sov_search_term_sv]
WHERE [date] >= @SynchDate;

INSERT INTO [dbo].[carhartt_ca_non_branded_sb_sov_search_term_sv]
SELECT
	[sbh].[phrase]						AS [search_term],
	[sbh].[adtype]						AS [adtype],
	CONVERT(DATE, [sbh].[datetime])		AS [date],
	SUM([st].[search_volume])			AS [search_volume]
FROM [dbo].[carhartt_ca_non_branded_sb_sov_20201020] sbh
LEFT JOIN [dbo].[carhartt_non_branded_searchTerms_20201020] st ON [sbh].[phrase] = [st].[phrase]
WHERE [sbh].[datetime] >= @SynchDate
GROUP BY	[sbh].[phrase],
			[sbh].[adtype],
			CONVERT(DATE, [sbh].[datetime])
GO