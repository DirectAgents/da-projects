CREATE VIEW [dbo].[AccountingDetailsWithPercentMargin]
AS
SELECT 
	*,
	CASE WHEN [Rev USD]!=0 THEN 
		([Margin USD]/[Rev USD])*100 
	ELSE 
		0 
	END 
	AS [%Margin]
FROM AccountingDetailsWithMargin
