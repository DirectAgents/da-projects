--drop view StatView1 as
--select 
--	S.id		StatID, 
--	RevP.id		StatRevPayoutID, 
--	RevC.id		CostPayoutID,
--	C.id		CampaignID,
--	C.pid		CampaignPID
--from Stat S
--go
--select * from StatView1
--go

--drop view ItemView1 as
--select 
--	I.id ItemID, 
--	S.StatID, 
--	S.RevPayoutID, 
--	S.CostPayoutID
--from Item I
--inner join Campaign C on I.pid = C.pid
--inner join Affiliate A on I.affid = A.affid
--inner join [Source] SR on I.source_id = SR.id
--inner join UnitType UT on I.unit_type_id = UT.id
--inner join ItemAccountingStatus IAS on I.item_accounting_status_id = IAS.id
--inner join ItemReportingStatus IRS on I.item_reporting_status_id = IRS.id
--left outer join StatView1 S on I.stat_id_n = S.StatID
--go
--select * from ItemView1
--go

CREATE view [dbo].[CampaignView1] as
select
	C.id				CampaignID,
	C.pid				CampaignPID,
	C.campaign_name		CampaignName
from Campaign C
inner join AdManager AD on C.ad_manager_id = AD.id
inner join AccountManager AM on C.account_manager_id = AM.id
inner join CampaignStatus CS on C.campaign_status_id = CS.id
inner join Advertiser AV on C.advertiser_id = AV.id
