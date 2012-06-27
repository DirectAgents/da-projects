CREATE VIEW [dbo].[StandardView_Accounting_1]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS PublisherName, dbo.Advertiser.name AS AdvertiserName, dbo.Campaign.pid, 
                      dbo.Campaign.campaign_name AS CampaignName, dbo.Currency.name AS RevenueCurrencyName, Currency_1.name AS CostCurrencyName, 
                      dbo.Item.revenue_per_unit AS RevenuePerUnit, dbo.Item.cost_per_unit AS CostPerUnit, SUM(dbo.Item.num_units) AS TotalUnits, dbo.UnitType.name AS UnitType, 
                      SUM(dbo.Item.total_revenue) AS TotalRevenue, SUM(dbo.Item.total_cost) AS TotalCost, SUM(dbo.Item.margin) AS TotalMargin, 
                      dbo.MediaBuyer.name AS MediaBuyerName, dbo.AdManager.name AS AdManagerName, dbo.AccountManager.name AS AccountManagerName, 
                      dbo.CampaignStatus.name AS CampaignStatusName, dbo.ItemAccountingStatus.name AS AccountingStatusName
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
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name
ORDER BY PublisherName
