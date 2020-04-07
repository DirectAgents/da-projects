INSERT INTO [dbo].[VolumesProduct]
(
    [phrase],
    [brand],
    [datetime],
    [title],
    [Page],
    [asins],
    [Position],
    [Search Volume]
)
SELECT
    [org].[phrase],
    [org].[brand],
    [org].[datetime],
    [org].[title],
    [org].[page],
    [org].[asins],
    [multi].[Position],
    [carh].[Impressions] * [multi].[Multiplier]
FROM [dbo].[Organic_newKeywords_130] AS org
LEFT JOIN [dbo].[carhartt_nb_search_terms] AS carh ON [org].[phrase] = [carh].[Search Term]
LEFT JOIN [dbo].[organic_position_page_multiplier] AS multi ON TRY_CAST([org].[index] AS BIGINT) + 1 = [multi].[position]
                                                            AND [org].[page] = [multi].[Page]
WHERE   [org].[spons] != 'Sponsored'
    AND [org].[title] != 'sb'
    AND [org].[index] != 'sb'
    AND ([org].[brand] LIKE 'Carhartt%'
       OR [org].[brand] LIKE 'Dickies%'
       OR [org].[brand] LIKE 'Amazon%')
    AND [org].[IsProductExtracted] = 0

UPDATE  [dbo].[Organic_newKeywords_130]
SET     [IsProductExtracted] = 1
WHERE   [spons] != 'Sponsored'
    AND [title] != 'sb'
    AND [index] != 'sb'
    AND ([brand] LIKE 'Carhartt%'
       OR [brand] LIKE 'Dickies%'
       OR [brand] LIKE 'Amazon%')
    AND [IsProductExtracted] = 0