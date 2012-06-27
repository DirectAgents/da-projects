CREATE VIEW [dbo].[CampaignsPublisherReportSummary]
AS
SELECT 
	 *
	,B.[Unverified] + B.[Verified] + B.[Approved] AS [ToBePaid]
	,B.[Unverified] + B.[Verified] + B.[Approved] + B.[Paid] AS [Total]
FROM
(
	SELECT
		 CampaignStatusName AS [CampaignStatus]
		,PublisherName AS [Publisher]
		,NetTermTypeName AS [NetTerms]
		,AffiliateCurrencyName AS [PayCurrency]
		,COALESCE([default], 0) AS [Unverified]
		,COALESCE([Verified], 0) AS [Verified]
		,COALESCE([Approved], 0) AS [Approved]
		,COALESCE([Check Signed and Paid], 0) AS [Paid]
	FROM
	(
		SELECT
			  COALESCE(Publisher.name, Affiliate.name) AS PublisherName
			 ,NetTermType.name AS NetTermTypeName
			 ,AffiliateCurrency.name AS AffiliateCurrencyName
			 ,SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS SumItemTotalCost
			 ,ItemAccountingStatus.name AS ItemAccountingStatusName
			 ,CampaignStatus.name AS CampaignStatusName
		FROM
			Item
			INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
			INNER JOIN Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id
			INNER JOIN Affiliate ON Item.affid = Affiliate.affid
			INNER JOIN NetTermType ON Affiliate.net_term_type_id = NetTermType.id
			INNER JOIN Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id
			INNER JOIN Campaign ON Item.pid = Campaign.pid
			INNER JOIN CampaignStatus ON Campaign.campaign_status_id = CampaignStatus.id
			LEFT OUTER JOIN Publisher ON Affiliate.affid = Publisher.affid
		GROUP BY
			 COALESCE(Publisher.name, Affiliate.name)
			,NetTermType.name
			,AffiliateCurrency.name
			,ItemAccountingStatus.name
			,CampaignStatus.name
	) AS A
	PIVOT
	(
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Verified], [Approved], [Check Signed and Paid], [Hold])
	) AS P
) AS B
