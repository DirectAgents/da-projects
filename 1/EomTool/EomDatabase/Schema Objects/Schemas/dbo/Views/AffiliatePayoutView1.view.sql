CREATE view [dbo].[AffiliatePayoutView1] as
select 
	P.id			AffiliatePayoutID,
	P.payout_id		AffiliatePayoutPayoutID,
	P.pid			AffiliatePayoutCampaignPID,
	P.affid			AffiliatePayoutAffiliateAffid,
	C.name			AffiliatePayoutCurrency,
	CASE WHEN P.affid = 0 THEN 'Y' ELSE 'N' END AffiliatePayoutIsDefault
from Payout P
inner join Currency C on P.currency_id = C.id
where payout_type = 'affiliate'
