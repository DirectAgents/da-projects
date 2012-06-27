CREATE VIEW [dbo].[StandardView_CampaignRevenueTotal]
AS
SELECT     dbo.Item.pid, dbo.Campaign.campaign_name, dbo.Item.revenue_currency_id, SUM(dbo.Item.total_revenue) AS TotalRevenueNative, 
                      SUM(dbo.Item.total_revenue * dbo.Currency.to_usd_multiplier) AS TotalRevenueUSD
FROM         dbo.Item INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id
GROUP BY dbo.Item.pid, dbo.Campaign.campaign_name, dbo.Item.revenue_currency_id
