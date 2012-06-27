CREATE VIEW [dbo].[ab_Advertisers_TotalRevenue]
AS
SELECT     TOP (100) PERCENT dbo.Advertiser.id AS AdvertiserId, dbo.Advertiser.name AS AdvertiserName, dbo.Item.revenue_currency_id AS RevenueCurrencyId, 
                      SUM(dbo.Item.total_revenue) AS TotalRevenue
FROM         dbo.Advertiser INNER JOIN
                      dbo.Campaign ON dbo.Advertiser.id = dbo.Campaign.advertiser_id INNER JOIN
                      dbo.Item ON dbo.Campaign.pid = dbo.Item.pid
GROUP BY dbo.Advertiser.name, dbo.Item.revenue_currency_id, dbo.Advertiser.id
ORDER BY AdvertiserName
