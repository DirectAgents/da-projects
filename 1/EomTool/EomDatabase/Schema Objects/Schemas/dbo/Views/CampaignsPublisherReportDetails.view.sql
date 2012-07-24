CREATE VIEW [dbo].[CampaignsPublisherReportDetails]
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
		,COALESCE(COALESCE([default],0)	+ COALESCE([Verified], 0) + COALESCE([Approved], 0), 0) AS ToBePaid
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
			 ,Campaign.campaign_name AS CampaignName
			 ,SUM(Item.num_units) AS NumUnits
			 ,Item.cost_per_unit * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier AS CostPerUnit
			 ,Affiliate.add_code AS AffiliateAddCode
			 ,CASE WHEN UnitType.name = 'CPM' THEN 'Yes' ELSE 'No' END AS IsCPM
			 ,MediaBuyer.name AS MediaBuyer
			 ,dbo.SumKeys(Item.id) AS ItemIDs
		FROM
			Item
			INNER JOIN UnitType ON Item.unit_type_id = UnitType.id
			INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
			INNER JOIN Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id
			INNER JOIN Affiliate ON Item.affid = Affiliate.affid
			INNER JOIN MediaBuyer ON Affiliate.media_buyer_id = MediaBuyer.id
			INNER JOIN NetTermType ON Affiliate.net_term_type_id = NetTermType.id
			INNER JOIN Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id
			INNER JOIN Campaign ON Item.pid = Campaign.pid
			INNER JOIN CampaignStatus ON Item.campaign_status_id = CampaignStatus.id
			LEFT OUTER JOIN Publisher ON Affiliate.affid = Publisher.affid
		GROUP BY
			 COALESCE(Publisher.name, Affiliate.name)
			,NetTermType.name
			,AffiliateCurrency.name
			,ItemAccountingStatus.name
			,CampaignStatus.name
			,Campaign.campaign_name
			,Affiliate.add_code
			,UnitType.name
			,MediaBuyer.name
			,Item.cost_per_unit * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier
	) AS A 
	PIVOT
	(
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Verified], [Approved], [Check Signed and Paid])
	) AS P
) AS B
