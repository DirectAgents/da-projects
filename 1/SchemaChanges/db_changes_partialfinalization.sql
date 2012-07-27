ALTER TABLE [dbo].[Item] ADD
[campaign_status_id] [int] NOT NULL CONSTRAINT [DF_Item_item_campaign_status_id] DEFAULT ((1))
GO
ALTER VIEW [dbo].[CampaignsPublisherReportDetails]
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
GO
ALTER VIEW [dbo].[CampaignsPublisherReportSummary]
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
			INNER JOIN CampaignStatus ON Item.campaign_status_id = CampaignStatus.id
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
GO
ALTER VIEW [dbo].[AccountManagerView]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.Item.cost_per_unit AS [Cost/Unit], SUM(dbo.Item.num_units) AS Units, dbo.UnitType.name AS [Unit Type], 
                      SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], 
                      dbo.AdManager.name AS [Ad Manager], dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, 
                      dbo.ItemAccountingStatus.name AS [Accounting Status], dbo.[Source].name AS [Source]
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Item.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
					  dbo.[Source] ON dbo.Item.source_id = dbo.Source.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, dbo.[Source].name
ORDER BY Publisher
GO
ALTER VIEW [dbo].[AccountingView1]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) AS [Rev/Unit USD], 
                      dbo.Item.cost_per_unit AS [Cost/Unit], dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) AS [Cost/Unit USD], SUM(dbo.Item.num_units) AS Units, 
                      dbo.UnitType.name AS [Unit Type], SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) 
                      * dbo.Item.num_units) AS [Revenue USD], SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) 
                      * dbo.Item.num_units) AS [Cost USD], SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], dbo.AdManager.name AS [Ad Manager], 
                      dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, dbo.ItemAccountingStatus.name AS [Accounting Status], 
                      dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name AS [Aff Pay Method], dbo.Affiliate.currency_id, Currency_2.name AS Expr1, 
                      Currency_2.name AS [Pub Pay Curr], Source.name AS Source
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Item.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.Source AS Source ON dbo.Item.source_id = Source.id LEFT OUTER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id LEFT OUTER JOIN
                      dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id LEFT OUTER JOIN
                      dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id LEFT OUTER JOIN
                      dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit), dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit), 
                      dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name, dbo.Affiliate.currency_id, Currency_2.name, Source.name
HAVING      (dbo.CampaignStatus.name = 'Verified')
ORDER BY Publisher
GO
UPDATE dbo.Item
SET dbo.Item.campaign_status_id = dbo.Campaign.campaign_status_id
FROM dbo.Item
INNER JOIN dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid
GO
