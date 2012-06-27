CREATE VIEW [dbo].[AccountingDetails]
AS
SELECT     COALESCE (dbo.Publisher.name, dbo.Affiliate.name) AS Publisher, dbo.Affiliate.name AS Affiliate, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid, 
                      dbo.Campaign.campaign_name AS Campaign, dbo.Currency.name AS [Rev Curr], Currency_1.name AS [Cost Curr], Currency_2.name AS [Pay Curr], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.Item.cost_per_unit AS [Cost/Unit], SUM(dbo.Item.num_units) AS Units, dbo.UnitType.name AS [Unit Type], 
                      SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.Item.total_cost) AS Cost, dbo.MediaBuyer.name AS [Media Buyer], dbo.AdManager.name AS [Ad Manager], 
                      dbo.AccountManager.name AS [Account Manager], dbo.ItemAccountingStatus.name AS [Accounting Status], SUM(dbo.tousd3(dbo.Item.revenue_currency_id, 
                      dbo.Item.revenue_per_unit) * dbo.Item.num_units) AS [Rev USD], SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) * dbo.Item.num_units) 
                      AS [Cost USD]
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid LEFT OUTER JOIN
                      dbo.Publisher ON dbo.Item.affid = dbo.Publisher.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id
GROUP BY COALESCE (dbo.Publisher.name, dbo.Affiliate.name), dbo.Affiliate.name, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, 
                      dbo.Currency.name, Currency_1.name, Currency_2.name, dbo.Item.revenue_per_unit, dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, 
                      dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name
