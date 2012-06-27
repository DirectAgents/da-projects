CREATE VIEW [dbo].[vwSub2]
AS
SELECT     SUM(dbo.Item.total_revenue) AS rev, dbo.Advertiser.id AS advid
FROM         dbo.Item INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id
GROUP BY dbo.Advertiser.id
