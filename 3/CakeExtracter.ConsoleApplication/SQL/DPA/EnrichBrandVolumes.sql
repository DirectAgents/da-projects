INSERT INTO [dbo].[VolumeBrand]
(
    [Search Volume],
    [Phrase],
    [Page],
    [Brand],
    [Datetime]
)
SELECT
    SUM([carh].[Impressions] * [multi].[Multiplier]),
    [org].[phrase],
    [org].[page],
    [org].[brand],
    [org].[datetime]
FROM [dbo].[Organic_newKeywords_130] AS org
LEFT JOIN [dbo].[carhartt_nb_search_terms] AS carh ON [org].[phrase] = [carh].[Search Term]
LEFT JOIN [dbo].[organic_position_page_multiplier] AS multi ON TRY_CAST([org].[index] AS BIGINT) + 1 = [multi].[position]
                                                            AND [org].[page] = [multi].[Page]
WHERE   [org].[spons] != 'Sponsored'
    AND [org].[title] != 'sb'
    AND [org].[index] != 'sb'
    AND [org].[IsBrandExtracted] = 0
GROUP BY [org].[phrase], [org].[page], [org].[brand], [org].[datetime]

UPDATE  [dbo].[Organic_newKeywords_130]
SET     [IsBrandExtracted] = 1
WHERE   [spons] != 'Sponsored'
    AND [title] != 'sb'
    AND [index] != 'sb'
    AND [IsBrandExtracted] = 0