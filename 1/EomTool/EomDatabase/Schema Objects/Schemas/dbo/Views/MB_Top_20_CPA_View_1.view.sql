CREATE VIEW [dbo].[MB_Top_20_CPA_View_1]
AS
SELECT     TOP (20) [Campaign Name], [Campaign Number]
FROM         dbo.Jan11_AccountManager_View_1
GROUP BY [Campaign Name], [Campaign Number]
HAVING      ([Campaign Name] LIKE 'US%') AND ([Campaign Name] NOT LIKE '%CPM%')
ORDER BY SUM(Revenue) DESC
