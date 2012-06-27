CREATE VIEW [dbo].[StandardView_CampaignRevenueTotal_CPM]
AS
SELECT     pid, campaign_name, revenue_currency_id, TotalRevenueNative, TotalRevenueUSD
FROM         dbo.StandardView_CampaignRevenueTotal
WHERE     (campaign_name LIKE '%CPM%')
