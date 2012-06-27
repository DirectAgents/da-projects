CREATE VIEW [dbo].[VerifiedCampaignsPublisherReportSummary]
AS
SELECT 
	 *
	,B.[To Be Paid] + B.Paid AS [Total]
FROM
(
	SELECT
		 PublisherName AS Name
		,NetTermTypeName AS [Net Terms]
		,AffiliateCurrencyName AS [Curr]
		,COALESCE([default], 0) AS [To Be Paid]
		,COALESCE([Check Signed and Paid], 0) AS [Paid]
	FROM
	(
		SELECT
			  COALESCE(Publisher.name, Affiliate.name) AS PublisherName
			 ,NetTermType.name AS NetTermTypeName
			 ,AffiliateCurrency.name AS AffiliateCurrencyName
			 ,SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS SumItemTotalCost
			 ,ItemAccountingStatus.name AS ItemAccountingStatusName
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
		WHERE
			CampaignStatus.name = 'Verified'
		GROUP BY
			 COALESCE(Publisher.name, Affiliate.name)
			,NetTermType.name
			,AffiliateCurrency.name
			,ItemAccountingStatus.name
	) AS A
	PIVOT
	(
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Check Signed and Paid])
	) AS P
) AS B
