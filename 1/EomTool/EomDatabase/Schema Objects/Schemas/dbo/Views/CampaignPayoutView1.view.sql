CREATE view [dbo].[CampaignPayoutView1] as
select 
	P.id			CampaignPayoutID,
	P.payout_id		CampaignPayoutPayoutID,
	P.pid			CampaignPayoutCampaignPID,
	P.affid			CampaignPayoutAffiliateAffid,
	C.name			CampaignPayoutCurrency,
	CASE WHEN P.affid = 0 THEN 'Y' ELSE 'N' END CampaignPayoutIsDefault
from Payout P
inner join Currency C on P.currency_id = C.id
where payout_type = 'campaign'
