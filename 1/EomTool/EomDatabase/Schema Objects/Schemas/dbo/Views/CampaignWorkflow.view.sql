CREATE VIEW [dbo].[CampaignWorkflow]
AS
SELECT     dbo.Campaign.id, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Campaign.modified, dbo.AccountManager.name AS AM, dbo.Advertiser.name AS Advertiser, 
                      dbo.Currency.name AS Curr, SUM(dbo.Item.total_revenue) AS Revenue, SUM(CASE Item.campaign_status_id WHEN 1 THEN Item.total_revenue ELSE 0 END) 
                      AS RevDefault, SUM(CASE Item.campaign_status_id WHEN 2 THEN Item.total_revenue ELSE 0 END) AS RevFinalized, 
                      SUM(CASE Item.campaign_status_id WHEN 4 THEN Item.total_revenue ELSE 0 END) AS RevVerified, dbo.CampaignStatus.name AS Status, 
                      dbo.LatestCampaignNote.note AS Note, CampaignStatus_1.name AS ItemCampaignStatus, COUNT(DISTINCT dbo.Item.affid) AS NumAffiliates, 
                      COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 1 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet7, 
                      COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 2 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet15, 
                      COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 3 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet30, 
                      COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 4 THEN Item.affid ELSE NULL END) AS NumAffiliatesNetBiWeekly, 
                      COUNT(DISTINCT CASE Item.campaign_status_id WHEN 1 THEN Item.affid ELSE NULL END) AS NumDefault, 
                      COUNT(DISTINCT CASE Item.campaign_status_id WHEN 2 THEN Item.affid ELSE NULL END) AS NumFinalized, 
                      COUNT(DISTINCT CASE Item.campaign_status_id WHEN 4 THEN Item.affid ELSE NULL END) AS NumVerified
FROM         dbo.Campaign INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.Item ON dbo.Campaign.pid = dbo.Item.pid INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.CampaignStatus AS CampaignStatus_1 ON dbo.Item.campaign_status_id = CampaignStatus_1.id LEFT OUTER JOIN
                      dbo.LatestCampaignNote ON dbo.Campaign.pid = dbo.LatestCampaignNote.campaign_id INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.AccountManagerAccessList ON dbo.AccountManager.name = dbo.AccountManagerAccessList.name
GROUP BY dbo.Campaign.id, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Campaign.modified, dbo.AccountManager.name, dbo.Advertiser.name, dbo.Currency.name,
                       dbo.CampaignStatus.name, dbo.LatestCampaignNote.note, CampaignStatus_1.name
