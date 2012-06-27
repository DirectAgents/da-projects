CREATE view [dbo].[AffiliateView1] as
select 
	A.id			AffiateID,
	A.affid			AffilaiteAffilaiteID,
	A.name			AffiliateName,
	A.email			AffiliateEmail,
	A.add_code		AffiliateAddCode,
	N.name			AffiliateNetTerm,
	P.name			AffiliatePaymentMethod,
	C.name			AffiliatePaymentCurrency,
	MB.name			MediaBuyerName,
    A.date_created  Created
from Affiliate A
inner join MediaBuyer MB on A.media_buyer_id = MB.id
inner join Currency C on A.currency_id = C.id
inner join NetTermType N on A.net_term_type_id = N.id
inner join AffiliatePaymentMethod P on A.payment_method_id = P.id
