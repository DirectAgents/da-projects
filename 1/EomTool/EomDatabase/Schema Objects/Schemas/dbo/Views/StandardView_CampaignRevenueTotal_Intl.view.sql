CREATE VIEW [dbo].[StandardView_CampaignRevenueTotal_Intl]
AS
SELECT     pid, campaign_name, revenue_currency_id, TotalRevenueNative, TotalRevenueUSD
FROM         dbo.StandardView_CampaignRevenueTotal
WHERE     (NOT (campaign_name LIKE 'US%'))
