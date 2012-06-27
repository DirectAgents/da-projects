CREATE VIEW [dbo].[Rehena_Jan11EomSheetIntl]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.Item.cost_per_unit AS [Cost/Unit], SUM(dbo.Item.num_units) AS Units, dbo.UnitType.name AS [Unit Type], 
                      SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.Item.total_cost) AS Cost, Currency_2.name AS [Pub Pay Curr], 
                      SUM(dbo.Item.total_cost * Currency_1.to_usd_multiplier / Currency_2.to_usd_multiplier) AS [Pub Cost], SUM(dbo.Item.margin) AS Margin, 
                      dbo.MediaBuyer.name AS [Media Buyer], dbo.AdManager.name AS [Ad Manager], dbo.AccountManager.name AS [Account Manager], 
                      dbo.CampaignStatus.name AS Status, dbo.ItemAccountingStatus.name AS [Accounting Status]
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
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, Currency_2.name
HAVING      (dbo.CampaignStatus.name = 'Verified') AND (Currency_2.name = 'GBP' OR
                      Currency_2.name = 'EUR')
ORDER BY Publisher
