CREATE VIEW [dbo].[VerifiedItemsCampaignRevenueSummary]
AS
SELECT     dbo.Item.pid, dbo.Campaign.campaign_name AS CampaignName, SUM(dbo.Item.total_revenue) AS TotalRevenue, dbo.Currency.name AS CurrencyName, 
                      dbo.SumKeys(dbo.Item.id) AS ItemIds, dbo.Advertiser.name AS AdvertiserName
FROM         dbo.Campaign INNER JOIN
                      dbo.Item ON dbo.Campaign.pid = dbo.Item.pid INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Item.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id
GROUP BY dbo.Currency.name, dbo.Item.pid, dbo.Campaign.campaign_name, dbo.CampaignStatus.name, dbo.Advertiser.name
HAVING      (dbo.CampaignStatus.name = 'Verified')
