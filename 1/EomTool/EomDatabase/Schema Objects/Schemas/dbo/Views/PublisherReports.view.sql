CREATE VIEW [dbo].[PublisherReports]
AS
SELECT *, B.[ToBePaid] + B.[Paid] AS Total
FROM
(
	SELECT
		 ItemIDs
		,CampaignStatusName AS CampaignStatus
		,PublisherName AS Publisher
		,AffiliateAddCode AS AddCode
		,CampaignName AS CampaignName
		,NumUnits AS NumUnits
		,CostPerUnit AS CostPerUnit
		,NetTermTypeName AS NetTerms
		,AffiliateCurrencyName AS PayCurrency
		,IsCPM AS [IsCPM]
		,MediaBuyer
		,COALESCE([default], 0) AS ToBePaid
		,COALESCE([Check Signed and Paid], 0) AS [Paid]
	FROM (
		SELECT * FROM PublisherReportDetails ) AS A 
	PIVOT ( 
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Check Signed and Paid])
	) AS P
) AS B
