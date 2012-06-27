CREATE VIEW [dbo].[LatestCampaignNote] AS

with T as 
(
	select id, campaign_id, note,
	row_number() over(partition by campaign_id order by id desc) as rn
	from CampaignNotes
)
select campaign_id, note from T
where rn=1
