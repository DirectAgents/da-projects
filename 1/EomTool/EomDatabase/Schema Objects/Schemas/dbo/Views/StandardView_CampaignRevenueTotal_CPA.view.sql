CREATE VIEW [dbo].[StandardView_CampaignRevenueTotal_CPA]
AS
SELECT     pid, campaign_name, revenue_currency_id, TotalRevenueNative, TotalRevenueUSD
FROM         dbo.StandardView_CampaignRevenueTotal
WHERE     (campaign_name LIKE 'US%') AND (campaign_name NOT LIKE '%CPM%')
