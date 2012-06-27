CREATE VIEW [dbo].[MB_CPA_Ranked_Intl_View_1]
AS
SELECT     TOP (15) [Campaign Name], [Campaign Number], Revenue
FROM         dbo.Jan11_AccountManager_View_1
GROUP BY [Campaign Name], [Campaign Number], Revenue
HAVING      (NOT ([Campaign Name] LIKE 'US%'))
ORDER BY SUM(Revenue) DESC
