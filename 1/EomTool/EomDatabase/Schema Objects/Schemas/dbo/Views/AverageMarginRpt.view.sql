CREATE view [dbo].[AverageMarginRpt] as
select top(100) percent [Affiliate], avg([%Margin]) [Avg %Margin], avg([Margin USD]) [Avg Margin]
from (
	select top(100) percent
		  A.name2 [Affiliate]
		, I.total_revenue * CUR1.to_usd_multiplier [Revenue USD]
		, I.margin [Margin USD]
		, I.margin  / I.total_revenue * CUR1.to_usd_multiplier [%Margin]
		, AM.name [AM]
	from Item I
		inner join Affiliate A on I.affid=A.affid
		inner join Campaign CP on I.pid=CP.pid
		inner join AccountManager AM on CP.account_manager_id=AM.id
		inner join Currency CUR1 on I.revenue_currency_id=CUR1.id
	where I.total_revenue != 0
) A
group by [Affiliate]
order by [Avg %Margin]


--where I.id in (16153,16164,16172,16181,16195,16201,16203,16219,16228,16231)

--select 
--	  C.campaign_name [Campaign]
--	, A.add_code [CD#]
--	, sum(I.total_revenue * CUR1.to_usd_multiplier) [Revenue USD]
--	, sum(I.margin) [Margin USD]
--	, I.total_revenue * CUR1.to_usd_multiplier / I.margin [%Margin] 
--from Item I
--	inner join Affiliate A on I.affid=A.affid
--	inner join Campaign C on I.pid=C.pid
--	inner join Currency CUR1 on I.revenue_currency_id=CUR1.id
--	left outer join Stat S on I.stat_id_n=S.id
--	left outer join Payout P on S.cost_payout_id=P.id
--group by
--	  C.campaign_name
--	, A.add_code
--	, I.total_revenue * CUR1.to_usd_multiplier / I.margin
--	, I.margin
--having (I.margin != 0)
--order by [%Margin]
