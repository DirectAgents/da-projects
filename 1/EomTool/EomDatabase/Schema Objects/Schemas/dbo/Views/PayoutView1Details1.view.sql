CREATE view [dbo].[PayoutView1Details1] as
select	
	id					PayoutID, 
	payout_id			PayoutPayoutID,
	pid					PayoutCampaignPID,
	affid				PayoutAffiliateAffid,
	impression,
	click,
	lead, 
	percent_sale		percentSale, 
	flat_sale			flatSale, 
	percent_sub_sale	percentSubSale, 
	flat_sub_sale		flatSubSale, 
	effective_date		effectiveDate, 
	modify_date			modifyDate, 
	product_id			productId
from
	Payout P
