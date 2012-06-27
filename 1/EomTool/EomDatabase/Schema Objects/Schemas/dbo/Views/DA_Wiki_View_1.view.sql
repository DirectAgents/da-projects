CREATE VIEW [dbo].[DA_Wiki_View_1]
AS
SELECT     dbo.Campaign.pid AS PID, dbo.Campaign.campaign_name AS Campaign, dbo.Campaign.campaign_type AS Type, dbo.Advertiser.name AS Advertiser, 
                      dbo.DTCampaignStatus.name AS Status, dbo.AccountManager.name AS AM, dbo.AdManager.name AS AD, dbo.Campaign.notes AS Notes, 
                      dbo.Campaign.dt_campaign_url AS URL, dbo.Campaign.dt_allowed_country_names AS Countries, dbo.Campaign.is_email AS Email, 
                      dbo.Campaign.is_search AS Search, dbo.Campaign.is_display AS Display, dbo.Campaign.is_coreg AS CoReg, dbo.Campaign.max_scrub AS [Max Scrub], 
                      dbo.Campaign.modified AS Modified, dbo.Campaign.created AS Created
FROM         dbo.Campaign INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.DTCampaignStatus ON dbo.Campaign.dt_campaign_status = dbo.DTCampaignStatus.name
