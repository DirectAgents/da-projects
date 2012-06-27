CREATE VIEW [dbo].[MB_View_Support_PID_SUM_Revenue]
AS
SELECT     dbo.Item.pid, SUM(dbo.Item.total_revenue * dbo.Currency.to_usd_multiplier) AS SumRevenueUSD, dbo.Campaign.campaign_name
FROM         dbo.Item INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id
GROUP BY dbo.Item.pid, dbo.Campaign.campaign_name
