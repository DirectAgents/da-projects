CREATE VIEW [dbo].[WorkflowView] AS
SELECT TOP 100 PERCENT
	  Campaign.id
	, Campaign.pid
	, Campaign.campaign_name
	, Campaign.modified
	, AccountManager.name AS AM
	, Advertiser.name AS Advertiser
	, Currency.name AS Curr
	, SUM(Item.total_revenue) AS Revenue
	, SUM(CASE Item.campaign_status_id WHEN 1 THEN Item.total_revenue ELSE 0 END) AS RevDefault
	, SUM(CASE Item.campaign_status_id WHEN 2 THEN Item.total_revenue ELSE 0 END) AS RevFinalized
	, SUM(CASE Item.campaign_status_id WHEN 4 THEN Item.total_revenue ELSE 0 END) AS RevVerified, CampaignStatus.name AS Status
	, LatestCampaignNote.note AS Note, CampaignStatus_1.name AS ItemCampaignStatus
	, COUNT(DISTINCT Item.affid) AS NumAffiliates
	, COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 1 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet7
	, COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 2 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet15
	, COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 3 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet30
	, COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 4 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet7BiWeekly
	, COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 5 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet15BiWeekly
	, COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 6 THEN Item.affid ELSE NULL END) AS NumAffiliatesWeekly
	, COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 7 THEN Item.affid ELSE NULL END) AS NumAffiliatesBiWeekly
	, COUNT(DISTINCT CASE Item.campaign_status_id WHEN 1 THEN Item.affid ELSE NULL END) AS NumDefault
	, COUNT(DISTINCT CASE Item.campaign_status_id WHEN 2 THEN Item.affid ELSE NULL END) AS NumFinalized
	, COUNT(DISTINCT CASE Item.campaign_status_id WHEN 4 THEN Item.affid ELSE NULL END) AS NumVerified
	, MediaBuyerApprovalStatus.name MediaBuyerApprovalStatus
	, dbo.SumKeys(Item.id) ItemIds
FROM
	Campaign INNER JOIN
	AccountManager ON Campaign.account_manager_id = AccountManager.id INNER JOIN
	Advertiser ON Campaign.advertiser_id = Advertiser.id INNER JOIN
	CampaignStatus ON Campaign.campaign_status_id = CampaignStatus.id INNER JOIN
	Item ON Campaign.pid = Item.pid INNER JOIN
	Currency ON Item.revenue_currency_id = Currency.id INNER JOIN
	CampaignStatus AS CampaignStatus_1 ON Item.campaign_status_id = CampaignStatus_1.id LEFT OUTER JOIN
	LatestCampaignNote ON Campaign.pid = LatestCampaignNote.campaign_id INNER JOIN
	Affiliate ON Item.affid = Affiliate.affid INNER JOIN
	MediaBuyerApprovalStatus ON Item.media_buyer_approval_status_id = MediaBuyerApprovalStatus.id
GROUP BY 
	  Campaign.id
	, Campaign.pid
	, Campaign.campaign_name
	, Campaign.modified
	, AccountManager.name
	, Advertiser.name
	, Currency.name
	, CampaignStatus.name
	, LatestCampaignNote.note
	, CampaignStatus_1.name
	, MediaBuyerApprovalStatus.name
ORDER BY 
	Campaign.campaign_name
