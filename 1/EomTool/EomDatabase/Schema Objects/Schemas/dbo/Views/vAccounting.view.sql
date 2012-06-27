CREATE VIEW [dbo].[vAccounting] AS
WITH B AS
(
SELECT     
	  dbo.Affiliate.name2 AS Publisher
	, dbo.Advertiser.name AS Advertiser
	, dbo.Campaign.pid AS [Campaign Number]
	, dbo.Campaign.campaign_name AS [Campaign Name]
	, dbo.Currency.name AS [Rev Currency]
	, Currency_1.name AS [Cost Currency]
	, dbo.Item.revenue_per_unit AS [Rev/Unit]
	, dbo.tousd3(dbo.Item.revenue_currency_id
	, dbo.Item.revenue_per_unit) AS [Rev/Unit USD]
	, dbo.Item.cost_per_unit AS [Cost/Unit]
	, dbo.tousd3(dbo.Item.cost_currency_id
	, dbo.Item.cost_per_unit) AS [Cost/Unit USD]
	, SUM(dbo.Item.num_units) AS Units
	, dbo.UnitType.name AS [Unit Type]
	, SUM(dbo.Item.total_revenue) AS Revenue
	, SUM(dbo.tousd3(dbo.Item.revenue_currency_id
	, dbo.Item.revenue_per_unit) * dbo.Item.num_units) AS [Revenue USD]
	, SUM(dbo.Item.total_cost) AS Cost
	, SUM(dbo.tousd3(dbo.Item.cost_currency_id
	, dbo.Item.cost_per_unit) * dbo.Item.num_units) AS [Cost USD]
	, SUM(dbo.Item.margin) AS Margin
	, dbo.MediaBuyer.name AS [Media Buyer]
	, dbo.AdManager.name AS [Ad Manager]
	, dbo.AccountManager.name AS [Account Manager]
	, dbo.CampaignStatus.name AS [Status]
	, dbo.ItemAccountingStatus.name AS [Accounting Status]
	, dbo.NetTermType.name
	, dbo.AffiliatePaymentMethod.name AS [Aff Pay Method]
	, dbo.Affiliate.currency_id, Currency_2.name AS Expr1
	, Currency_2.name AS [Pub Pay Curr]
	, dbo.GetCampaignNotes(dbo.Campaign.pid) AS CampaignNotes
	, dbo.SumKeys(dbo.Item.id) AS ItemIDs
	, CASE WHEN item.[notes] LIKE '%from dt%' THEN '-' ELSE item.[notes] END AS ItemNotes
FROM
	dbo.Item 
	INNER JOIN dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid 
	INNER JOIN dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid 
	INNER JOIN dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id 
	INNER JOIN dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id
	INNER JOIN dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id 
	INNER JOIN dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id 
	INNER JOIN dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id 
	INNER JOIN dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id 
	INNER JOIN dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id 
	INNER JOIN dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id 
	INNER JOIN dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id 
	INNER JOIN dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id 
	INNER JOIN dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id 
	INNER JOIN dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id 
	LEFT OUTER JOIN dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id
WHERE 
	dbo.CampaignStatus.name = 'Verified'
GROUP BY 
	  dbo.Affiliate.name2
	, dbo.Advertiser.name
	, dbo.Campaign.pid
	, dbo.Campaign.campaign_name
	, dbo.Currency.name
	, Currency_1.name
	, dbo.Item.revenue_per_unit
	, dbo.Item.cost_per_unit
	, dbo.MediaBuyer.name
	, dbo.AdManager.name
	, dbo.AccountManager.name
	, dbo.ItemAccountingStatus.name
	, dbo.UnitType.name
	, dbo.CampaignStatus.name
	, dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit)
	, dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit)
	, dbo.NetTermType.name
	, dbo.AffiliatePaymentMethod.name
	, dbo.Affiliate.currency_id
	, Currency_2.name
	, CASE WHEN [item].[notes] LIKE '%from dt%' THEN '-' ELSE [item].[notes] END
)
SELECT
	  B.[Publisher]
	, B.[Advertiser]
	, B.[Campaign Number]
	, B.[Campaign Name]
	, B.[Rev Currency]
	, B.[Cost Currency]
	, B.[Rev/Unit]
	, B.[Rev/Unit USD]
	, B.[Cost/Unit]
	, B.[Cost/Unit USD]
	, B.[Units]
	, B.[Unit Type]
	, B.[Revenue]
	, B.[Revenue USD]
	, B.[Cost]
	, B.[Cost USD]
	, B.[Revenue USD] - B.[Cost USD] AS [Margin]
	, (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) / [Revenue USD] END) * 100 AS [%Margin]
	, B.[Media Buyer]
	, B.[Ad Manager]
	, B.[Account Manager]
	, B.[Status]
	, B.[Accounting Status]
	, B.[name]
	, B.[Aff Pay Method]
	, B.[Pub Pay Curr]
	, B.[Cost USD] / dbo.[Currency].[to_usd_multiplier] AS [Pub Payout]
	, dbo.[GetCampaignNotes](B.[Campaign Number]) AS [CampaignNotes]
	, B.[ItemNotes]
FROM
	B
	INNER JOIN dbo.Currency ON B.currency_id = dbo.Currency.id
