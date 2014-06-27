CREATE VIEW [dbo].[AdvertiserPaymentStatus] AS
SELECT
dbo.AccountingView1.Publisher,
dbo.AccountingView1.[Publisher Status],
dbo.AccountingView1.Advertiser,
dbo.Advertiser.[status] AS [Advertiser Status],
dbo.Advertiser.[payment_terms] AS [Advertiser Payment Terms],
CASE WHEN sum([Cost USD]) over(partition by [Campaign Number])=0 THEN 0 ELSE [Cost USD] * 100.0 / sum([Cost USD]) over(partition by [Campaign Number]) END as [Percentage of Traffic Generated],
'' AS [Comments],
'' AS [Previous Periods Open Balance],
dbo.Advertiser.[invoicing_status] AS [Current Period Invoicing Status],
CASE WHEN dbo.AccountingView1.[Status]='default' THEN 'Unfinalized' ELSE 'Finalized' END AS [Finalization Status],
dbo.AccountingView1.[Unit Type],
dbo.AccountingView1.[Campaign Name],
dbo.AccountingView1.[Rev Currency],
dbo.AccountingView1.[Revenue],
dbo.AccountingView1.[Revenue USD],
dbo.AccountingView1.[Pub Pay Curr],
dbo.AccountingView1.[Cost USD] / dbo.Currency.to_usd_multiplier AS [Pub Payout],
dbo.AccountingView1.[Media Buyer],
dbo.AccountingView1.[Ad Manager],
dbo.AccountingView1.[Account Manager]
FROM dbo.AccountingView1
	 INNER JOIN dbo.Currency ON dbo.AccountingView1.currency_id = dbo.Currency.id
	 INNER JOIN dbo.Campaign ON dbo.AccountingView1.[Campaign Number] = dbo.Campaign.pid
	 INNER JOIN dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id
WHERE        dbo.AccountingView1.Revenue<>0 OR dbo.AccountingView1.Cost<>0

GO