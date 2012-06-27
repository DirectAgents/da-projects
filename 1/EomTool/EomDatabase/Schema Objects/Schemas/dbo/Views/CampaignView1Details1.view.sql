CREATE view [dbo].[CampaignView1Details1] as
select
	C.id				CampaignID,
	C.pid				CampaignPID,
	C.campaign_name		CampaignName,
	AM.name				CampaignAccountManagerName,
	AD.name				CampaignAdManagerName,
	AV.name				CampaignAdvertiserName,
	CS.name				CampaignStatus
from Campaign C
inner join AdManager AD on C.ad_manager_id = AD.id
inner join AccountManager AM on C.account_manager_id = AM.id
inner join CampaignStatus CS on C.campaign_status_id = CS.id
inner join Advertiser AV on C.advertiser_id = AV.id
