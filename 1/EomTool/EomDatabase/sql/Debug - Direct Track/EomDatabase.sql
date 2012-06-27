/*
Deployment script for DADatabaseJune2012
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "DADatabaseJune2012"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.DA\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.DA\MSSQL\DATA\"

GO
:on error exit
GO
USE [master]
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO

IF NOT EXISTS (SELECT 1 FROM [master].[dbo].[sysdatabases] WHERE [name] = N'$(DatabaseName)')
BEGIN
    RAISERROR(N'You cannot deploy this update script to target BIZ2\DA. The database for which this script was built, DADatabaseJune2012, does not exist on this server.', 16, 127) WITH NOWAIT
    RETURN
END

GO

IF (@@servername != 'BIZ2\DA')
BEGIN
    RAISERROR(N'The server name in the build script %s does not match the name of the target server %s. Verify whether your database project settings are correct and whether your build script is up to date.', 16, 127,N'BIZ2\DA',@@servername) WITH NOWAIT
    RETURN
END

GO

IF CAST(DATABASEPROPERTY(N'$(DatabaseName)','IsReadOnly') as bit) = 1
BEGIN
    RAISERROR(N'You cannot deploy this update script because the database for which it was built, %s , is set to READ_ONLY.', 16, 127, N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
USE [$(DatabaseName)]
GO
PRINT N'Creating [dbo].[AccountingView1]...';


GO
CREATE VIEW [dbo].[AccountingView1]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) AS [Rev/Unit USD], 
                      dbo.Item.cost_per_unit AS [Cost/Unit], dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) AS [Cost/Unit USD], SUM(dbo.Item.num_units) AS Units, 
                      dbo.UnitType.name AS [Unit Type], SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) 
                      * dbo.Item.num_units) AS [Revenue USD], SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) 
                      * dbo.Item.num_units) AS [Cost USD], SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], dbo.AdManager.name AS [Ad Manager], 
                      dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, dbo.ItemAccountingStatus.name AS [Accounting Status], 
                      dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name AS [Aff Pay Method], dbo.Affiliate.currency_id, Currency_2.name AS Expr1, 
                      Currency_2.name AS [Pub Pay Curr], [Source].name [Source]
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id INNER JOIN
                      dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id INNER JOIN
                      dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id INNER JOIN
					  dbo.[Source] AS [Source] ON dbo.Item.source_id = [Source].id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit), dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit), 
                      dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name, dbo.Affiliate.currency_id, Currency_2.name, [Source].name
HAVING      (dbo.CampaignStatus.name = 'Verified')
ORDER BY Publisher
GO
PRINT N'Creating [dbo].[AccountingView2]...';


GO
CREATE VIEW AccountingView2 AS
SELECT     dbo.AccountingView1.Publisher, dbo.AccountingView1.Advertiser, dbo.AccountingView1.[Campaign Number], dbo.AccountingView1.[Campaign Name], 
                      dbo.AccountingView1.[Rev Currency], dbo.AccountingView1.[Cost Currency], dbo.AccountingView1.[Rev/Unit], dbo.AccountingView1.[Rev/Unit USD], 
                      dbo.AccountingView1.[Cost/Unit], dbo.AccountingView1.[Cost/Unit USD], dbo.AccountingView1.Units, dbo.AccountingView1.[Unit Type], dbo.AccountingView1.Revenue, 
                      dbo.AccountingView1.[Revenue USD], dbo.AccountingView1.Cost, dbo.AccountingView1.[Cost USD], 
                      dbo.AccountingView1.[Revenue USD] - dbo.AccountingView1.[Cost USD] AS Margin, (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) 
                      / [Revenue USD] END) * 100 AS [%Margin], dbo.AccountingView1.[Media Buyer], dbo.AccountingView1.[Ad Manager], dbo.AccountingView1.[Account Manager], 
                      dbo.AccountingView1.Status, dbo.AccountingView1.[Accounting Status], dbo.AccountingView1.name, dbo.AccountingView1.[Aff Pay Method], 
                      dbo.AccountingView1.[Pub Pay Curr], dbo.AccountingView1.[Cost USD] / dbo.Currency.to_usd_multiplier AS [Pub Payout], 
                      dbo.GetCampaignNotes(dbo.AccountingView1.[Campaign Number]) AS CampaignNotes, dbo.AccountingView1.[Source]
FROM         dbo.AccountingView1 INNER JOIN
                      dbo.Currency ON dbo.AccountingView1.currency_id = dbo.Currency.id
GO
PRINT N'Creating [dbo].[AccountManagerView]...';


GO
CREATE VIEW [dbo].[AccountManagerView]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.Item.cost_per_unit AS [Cost/Unit], SUM(dbo.Item.num_units) AS Units, dbo.UnitType.name AS [Unit Type], 
                      SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], 
                      dbo.AdManager.name AS [Ad Manager], dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, 
                      dbo.ItemAccountingStatus.name AS [Accounting Status], dbo.[Source].name AS [Source]
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
					  dbo.[Source] ON dbo.Item.source_id = dbo.Source.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, dbo.[Source].name
ORDER BY Publisher
GO
PRINT N'Creating [dbo].[AdvertiserView1]...';


GO
CREATE view [dbo].[AdvertiserView1] as
select 
	A.id			AdvertiserID,
	A.name			AdvertiserName
from Advertiser A
GO
PRINT N'Creating [dbo].[Affiliate_Default_Payout_View_1]...';


GO
CREATE VIEW [dbo].[Affiliate_Default_Payout_View_1]
AS
SELECT     TOP (100) PERCENT D.pid, P.lead, P.currency_id
FROM         dbo.Payout AS P INNER JOIN
                          (SELECT     pid, MAX(effective_date) AS maxd, payout_type, affid
                            FROM          dbo.Payout
                            WHERE      (payout_type = 'affiliate') AND (affid = 0)
                            GROUP BY pid, payout_type, affid) AS D ON P.pid = D.pid AND P.effective_date = D.maxd AND P.payout_type = D.payout_type AND P.affid = D.affid
ORDER BY D.pid
GO
PRINT N'Creating [dbo].[AffiliatePayoutView1]...';


GO
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
GO
PRINT N'Creating [dbo].[AffiliateView1]...';


GO
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
GO
PRINT N'Creating [dbo].[AffiliateView1Details1]...';


GO
create view [dbo].[AffiliateView1Details1] as
select 
	A.id			AffiateID,
	A.affid			AffilaiteAffilaiteID,
	A.name			AffiliateName,
	A.email			AffiliateEmail,
	A.add_code		AffiliateAddCode,
	N.name			AffiliateNetTerm,
	P.name			AffiliatePaymentMethod,
	C.name			AffiliatePaymentCurrency,
	MB.name			MediaBuyerName
from Affiliate A
inner join MediaBuyer MB on A.media_buyer_id = MB.id
inner join Currency C on A.currency_id = C.id
inner join NetTermType N on A.net_term_type_id = N.id
inner join AffiliatePaymentMethod P on A.payment_method_id = P.id
GO
PRINT N'Creating [dbo].[AverageMarginRpt]...';


GO
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
GO
PRINT N'Creating [dbo].[Campaign_Default_Payout_View_1]...';


GO
CREATE VIEW [dbo].[Campaign_Default_Payout_View_1]
AS
SELECT     TOP (100) PERCENT D.pid, P.lead, P.currency_id
FROM         dbo.Payout AS P INNER JOIN
                          (SELECT     pid, MAX(effective_date) AS maxd, payout_type, affid
                            FROM          dbo.Payout
                            WHERE      (payout_type = 'campaign') AND (affid = 0)
                            GROUP BY pid, payout_type, affid) AS D ON P.pid = D.pid AND P.effective_date = D.maxd AND P.payout_type = D.payout_type AND P.affid = D.affid
ORDER BY D.pid
GO
PRINT N'Creating [dbo].[CampaignPayoutView1]...';


GO
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
GO
PRINT N'Creating [dbo].[CampaignsMargin]...';


GO
CREATE VIEW [dbo].[CampaignsMargin]
AS
SELECT     [Campaign Name], [Rev Currency], [Cost Currency], SUM(Revenue) AS TotalRevenue, SUM(Cost) AS TotalCost, AVG([%Margin]) AS AvgPercentMargin, 
                      dbo.SumKeys([%Margin]) AS Margins
FROM         dbo.AccountingView2
GROUP BY [Campaign Name], [Rev Currency], [Cost Currency]
GO
PRINT N'Creating [dbo].[CampaignsPublisherReportDetails]...';


GO
CREATE VIEW [dbo].[CampaignsPublisherReportDetails]
AS

SELECT *, B.[ToBePaid] + B.[Paid] AS Total
FROM
(
	SELECT
		 ItemIDs
		,CampaignStatusName AS CampaignStatus
		,PublisherName AS Publisher
		,AffiliateAddCode AS AddCode
		,CampaignName AS CampaignName
		,NumUnits AS NumUnits
		,CostPerUnit AS CostPerUnit
		,NetTermTypeName AS NetTerms
		,AffiliateCurrencyName AS PayCurrency
		,IsCPM AS [IsCPM]
		,MediaBuyer
		,COALESCE(COALESCE([default],0)	+ COALESCE([Verified], 0) + COALESCE([Approved], 0), 0) AS ToBePaid
		,COALESCE([Check Signed and Paid], 0) AS [Paid]
	FROM
	(
		SELECT
			  COALESCE(Publisher.name, Affiliate.name) AS PublisherName
			 ,NetTermType.name AS NetTermTypeName
			 ,AffiliateCurrency.name AS AffiliateCurrencyName
			 ,SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS SumItemTotalCost
			 ,ItemAccountingStatus.name AS ItemAccountingStatusName
			 ,CampaignStatus.name AS CampaignStatusName
			 ,Campaign.campaign_name AS CampaignName
			 ,SUM(Item.num_units) AS NumUnits
			 ,Item.cost_per_unit * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier AS CostPerUnit
			 ,Affiliate.add_code AS AffiliateAddCode
			 ,CASE WHEN UnitType.name = 'CPM' THEN 'Yes' ELSE 'No' END AS IsCPM
			 ,MediaBuyer.name AS MediaBuyer
			 ,dbo.SumKeys(Item.id) AS ItemIDs
		FROM
			Item
			INNER JOIN UnitType ON Item.unit_type_id = UnitType.id
			INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
			INNER JOIN Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id
			INNER JOIN Affiliate ON Item.affid = Affiliate.affid
			INNER JOIN MediaBuyer ON Affiliate.media_buyer_id = MediaBuyer.id
			INNER JOIN NetTermType ON Affiliate.net_term_type_id = NetTermType.id
			INNER JOIN Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id
			INNER JOIN Campaign ON Item.pid = Campaign.pid
			INNER JOIN CampaignStatus ON Campaign.campaign_status_id = CampaignStatus.id
			LEFT OUTER JOIN Publisher ON Affiliate.affid = Publisher.affid
		GROUP BY
			 COALESCE(Publisher.name, Affiliate.name)
			,NetTermType.name
			,AffiliateCurrency.name
			,ItemAccountingStatus.name
			,CampaignStatus.name
			,Campaign.campaign_name
			,Affiliate.add_code
			,UnitType.name
			,MediaBuyer.name
			,Item.cost_per_unit * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier
	) AS A 
	PIVOT
	(
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Verified], [Approved], [Check Signed and Paid])
	) AS P
) AS B
GO
PRINT N'Creating [dbo].[CampaignsPublisherReportSummary]...';


GO
CREATE VIEW [dbo].[CampaignsPublisherReportSummary]
AS
SELECT 
	 *
	,B.[Unverified] + B.[Verified] + B.[Approved] AS [ToBePaid]
	,B.[Unverified] + B.[Verified] + B.[Approved] + B.[Paid] AS [Total]
FROM
(
	SELECT
		 CampaignStatusName AS [CampaignStatus]
		,PublisherName AS [Publisher]
		,NetTermTypeName AS [NetTerms]
		,AffiliateCurrencyName AS [PayCurrency]
		,COALESCE([default], 0) AS [Unverified]
		,COALESCE([Verified], 0) AS [Verified]
		,COALESCE([Approved], 0) AS [Approved]
		,COALESCE([Check Signed and Paid], 0) AS [Paid]
	FROM
	(
		SELECT
			  COALESCE(Publisher.name, Affiliate.name) AS PublisherName
			 ,NetTermType.name AS NetTermTypeName
			 ,AffiliateCurrency.name AS AffiliateCurrencyName
			 ,SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS SumItemTotalCost
			 ,ItemAccountingStatus.name AS ItemAccountingStatusName
			 ,CampaignStatus.name AS CampaignStatusName
		FROM
			Item
			INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
			INNER JOIN Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id
			INNER JOIN Affiliate ON Item.affid = Affiliate.affid
			INNER JOIN NetTermType ON Affiliate.net_term_type_id = NetTermType.id
			INNER JOIN Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id
			INNER JOIN Campaign ON Item.pid = Campaign.pid
			INNER JOIN CampaignStatus ON Campaign.campaign_status_id = CampaignStatus.id
			LEFT OUTER JOIN Publisher ON Affiliate.affid = Publisher.affid
		GROUP BY
			 COALESCE(Publisher.name, Affiliate.name)
			,NetTermType.name
			,AffiliateCurrency.name
			,ItemAccountingStatus.name
			,CampaignStatus.name
	) AS A
	PIVOT
	(
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Verified], [Approved], [Check Signed and Paid], [Hold])
	) AS P
) AS B
GO
PRINT N'Creating [dbo].[CampaignView1]...';


GO
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
GO
PRINT N'Creating [dbo].[CampaignView1Details1]...';


GO
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
GO
PRINT N'Creating [dbo].[DA_Wiki_View_1]...';


GO
CREATE VIEW [dbo].[DA_Wiki_View_1]
AS
SELECT     dbo.Campaign.pid AS PID, dbo.Campaign.campaign_name AS Campaign, dbo.Campaign.campaign_type AS Type, dbo.Advertiser.name AS Advertiser, 
                      dbo.DTCampaignStatus.name AS Status, dbo.AccountManager.name AS AM, dbo.AdManager.name AS AD, dbo.Campaign.notes AS Notes, 
                      dbo.Campaign.dt_campaign_url AS URL, dbo.Campaign.dt_allowed_country_names AS Countries, dbo.Campaign.is_email AS Email, 
                      dbo.Campaign.is_search AS Search, dbo.Campaign.is_display AS Display, dbo.Campaign.is_coreg AS CoReg, dbo.Campaign.max_scrub AS [Max Scrub], 
                      dbo.Campaign.modified AS Modified, dbo.Campaign.created AS Created
FROM         dbo.Campaign INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.DTCampaignStatus ON dbo.Campaign.dt_campaign_status = dbo.DTCampaignStatus.name
GO
PRINT N'Creating [dbo].[EarlyPublisherList]...';


GO
CREATE VIEW EarlyPublisherList AS
SELECT     TOP (100) PERCENT dbo.NetTermType.name, dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.Item.cost_per_unit AS [Cost/Unit], dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) AS [Cost/UnitUSD], 
                      SUM(dbo.Item.num_units) AS Units, dbo.UnitType.name AS [Unit Type], SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.Item.total_cost) AS Cost, 
                      dbo.tousd3(dbo.Item.cost_currency_id, SUM(dbo.Item.total_cost)) AS CostUSD, SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], 
                      dbo.AdManager.name AS [Ad Manager], dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, 
                      dbo.ItemAccountingStatus.name AS [Accounting Status], dbo.Source.name AS Source
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id INNER JOIN
                      dbo.Source ON dbo.Item.source_id = dbo.Source.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, dbo.NetTermType.name, dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit), dbo.Item.cost_currency_id, 
                      dbo.Source.name
ORDER BY Publisher
GO
PRINT N'Creating [dbo].[Eddy_Accounting_View_1]...';


GO
CREATE VIEW [dbo].[Eddy_Accounting_View_1]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) AS [Rev/Unit USD], 
                      dbo.Item.cost_per_unit AS [Cost/Unit], dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) AS [Cost/Unit USD], SUM(dbo.Item.num_units) AS Units, 
                      dbo.UnitType.name AS [Unit Type], SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) 
                      * dbo.Item.num_units) AS [Revenue USD], SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) 
                      * dbo.Item.num_units) AS [Cost USD], SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], dbo.AdManager.name AS [Ad Manager], 
                      dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, dbo.ItemAccountingStatus.name AS [Accounting Status], 
                      dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name AS [Aff Pay Method], dbo.Affiliate.currency_id, Currency_2.name AS Expr1, 
                      Currency_2.name AS [Pub Pay Curr]
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id INNER JOIN
                      dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id INNER JOIN
                      dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit), dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit), 
                      dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name, dbo.Affiliate.currency_id, Currency_2.name
HAVING      (dbo.CampaignStatus.name = 'Verified')
ORDER BY Publisher
GO
PRINT N'Creating [dbo].[Eddy_Accounting_View_2]...';


GO
CREATE VIEW [dbo].[Eddy_Accounting_View_2]
AS
SELECT     dbo.Eddy_Accounting_View_1.Publisher, dbo.Eddy_Accounting_View_1.Advertiser, dbo.Eddy_Accounting_View_1.[Campaign Number], 
                      dbo.Eddy_Accounting_View_1.[Campaign Name], dbo.Eddy_Accounting_View_1.[Rev Currency], dbo.Eddy_Accounting_View_1.[Cost Currency], 
                      dbo.Eddy_Accounting_View_1.[Rev/Unit], dbo.Eddy_Accounting_View_1.[Rev/Unit USD], dbo.Eddy_Accounting_View_1.[Cost/Unit], 
                      dbo.Eddy_Accounting_View_1.[Cost/Unit USD], dbo.Eddy_Accounting_View_1.Units, dbo.Eddy_Accounting_View_1.[Unit Type], dbo.Eddy_Accounting_View_1.Revenue, 
                      dbo.Eddy_Accounting_View_1.[Revenue USD], dbo.Eddy_Accounting_View_1.Cost, dbo.Eddy_Accounting_View_1.[Cost USD], 
                      dbo.Eddy_Accounting_View_1.[Revenue USD] - dbo.Eddy_Accounting_View_1.[Cost USD] AS Margin, 
                      (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) / [Revenue USD] END) * 100 AS [%Margin], dbo.Eddy_Accounting_View_1.[Media Buyer], 
                      dbo.Eddy_Accounting_View_1.[Ad Manager], dbo.Eddy_Accounting_View_1.[Account Manager], dbo.Eddy_Accounting_View_1.Status, 
                      dbo.Eddy_Accounting_View_1.[Accounting Status], dbo.Eddy_Accounting_View_1.name, dbo.Eddy_Accounting_View_1.[Aff Pay Method], 
                      dbo.Eddy_Accounting_View_1.[Pub Pay Curr], dbo.Eddy_Accounting_View_1.[Cost USD] / dbo.Currency.to_usd_multiplier AS [Pub Payout], 
                      dbo.GetCampaignNotes(dbo.Eddy_Accounting_View_1.[Campaign Number]) AS CampaignNotes
FROM         dbo.Eddy_Accounting_View_1 INNER JOIN
                      dbo.Currency ON dbo.Eddy_Accounting_View_1.currency_id = dbo.Currency.id
GO
PRINT N'Creating [dbo].[FakePublishers]...';


GO
CREATE VIEW [dbo].[FakePublishers]
AS
SELECT DISTINCT name
FROM         dbo.Affiliate
WHERE     (name IN ('DAS', 'DA- Marchex', 'Direct Agents Search Team', 'Extra', 'Direct Agents Creative Servcies'))
GO
PRINT N'Creating [dbo].[Item_To_USD_View_1]...';


GO
CREATE VIEW [dbo].[Item_To_USD_View_1]
AS
SELECT     dbo.Item.id, dbo.Item.total_revenue * Currency_1.to_usd_multiplier AS total_revenue_usd, 
                      dbo.Item.total_cost * dbo.Currency.to_usd_multiplier AS total_cost_usd
FROM         dbo.Item INNER JOIN
                      dbo.Currency ON dbo.Item.cost_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.revenue_currency_id = Currency_1.id
GO
PRINT N'Creating [dbo].[Jan11_AccountManager_View_1]...';


GO
CREATE VIEW [dbo].[Jan11_AccountManager_View_1]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.Item.cost_per_unit AS [Cost/Unit], SUM(dbo.Item.num_units) AS Units, dbo.UnitType.name AS [Unit Type], 
                      SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], 
                      dbo.AdManager.name AS [Ad Manager], dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, 
                      dbo.ItemAccountingStatus.name AS [Accounting Status]
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name
ORDER BY Publisher
GO
PRINT N'Creating [dbo].[LatestCampaignNote]...';


GO
CREATE VIEW [dbo].[LatestCampaignNote] AS

with T as 
(
	select id, campaign_id, note,
	row_number() over(partition by campaign_id order by id desc) as rn
	from CampaignNotes
)
select campaign_id, note from T
where rn=1
GO
PRINT N'Creating [dbo].[MB_CPA_Ranked_Intl_View_1]...';


GO
CREATE VIEW [dbo].[MB_CPA_Ranked_Intl_View_1]
AS
SELECT     TOP (15) [Campaign Name], [Campaign Number], Revenue
FROM         dbo.Jan11_AccountManager_View_1
GROUP BY [Campaign Name], [Campaign Number], Revenue
HAVING      (NOT ([Campaign Name] LIKE 'US%'))
ORDER BY SUM(Revenue) DESC
GO
PRINT N'Creating [dbo].[MB_Ranked_CPM_1]...';


GO
CREATE VIEW [dbo].[MB_Ranked_CPM_1]
AS
SELECT     TOP (20) [Campaign Name], [Campaign Number]
FROM         dbo.Jan11_AccountManager_View_1
GROUP BY [Campaign Name], [Campaign Number]
HAVING      ([Campaign Name] LIKE '%CPM%')
ORDER BY SUM(Revenue) DESC
GO
PRINT N'Creating [dbo].[MB_Top_20_CPA_View_1]...';


GO
CREATE VIEW [dbo].[MB_Top_20_CPA_View_1]
AS
SELECT     TOP (20) [Campaign Name], [Campaign Number]
FROM         dbo.Jan11_AccountManager_View_1
GROUP BY [Campaign Name], [Campaign Number]
HAVING      ([Campaign Name] LIKE 'US%') AND ([Campaign Name] NOT LIKE '%CPM%')
ORDER BY SUM(Revenue) DESC
GO
PRINT N'Creating [dbo].[MB_View_Support_PID_SUM_Revenue]...';


GO
CREATE VIEW [dbo].[MB_View_Support_PID_SUM_Revenue]
AS
SELECT     dbo.Item.pid, SUM(dbo.Item.total_revenue * dbo.Currency.to_usd_multiplier) AS SumRevenueUSD, dbo.Campaign.campaign_name
FROM         dbo.Item INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id
GROUP BY dbo.Item.pid, dbo.Campaign.campaign_name
GO
PRINT N'Creating [dbo].[PayoutView1]...';


GO
--<?xml version="1.0" encoding="utf-8"?>
--<payout xmlns="http://www.digitalriver.com/directtrack/api/payout/v1_0" 
--xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
--xsi:schemaLocation="http://www.digitalriver.com/directtrack/api/payout/v1_0 payout.xsd"
--location="https://da-tracking.com/apifleet/rest/1137/payout/c3954">
--<payoutType>campaign</payoutType>
--<campaignResourceURL location="../campaign/1654"/>
--<affiliate allAffiliates="1"/>
--<impression>0</impression>
--<click>0</click>
--<lead>0</lead>
--<percentSale>0</percentSale>
--<flatSale>0</flatSale>
--<percentSubSale>0</percentSubSale>
--<flatSubSale>0</flatSubSale>
--<currency>USD</currency>
--</payout>

CREATE view [dbo].[PayoutView1] as
select 
	*
from Payout P
GO
PRINT N'Creating [dbo].[PayoutView1Details1]...';


GO
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
GO
PRINT N'Creating [dbo].[Publisher Report Approval]...';


GO
CREATE VIEW [dbo].[Publisher Report Approval]
AS
SELECT     *, B.[ToBePaid] + B.[Paid] AS Total, ' ' AS [Media Buyer Comment]
FROM         (SELECT    CampaignStatusName AS CampaignStatus, PublisherName AS Publisher, AffiliateAddCode AS AddCode, CampaignName AS CampaignName, 
                                              NumUnits AS NumUnits, CostPerUnit AS CostPerUnit, NetTermTypeName AS NetTerms, AffiliateCurrencyName AS PayCurrency, IsCPM AS [IsCPM], 
                                              MediaBuyer, COALESCE (COALESCE ([default], 0) + COALESCE ([Verified], 0) + COALESCE ([Approved], 0), 0) AS ToBePaid, 
                                              COALESCE ([Check Signed and Paid], 0) AS [Paid]
                       FROM          (SELECT     COALESCE (Publisher.name, Affiliate.name) AS PublisherName, NetTermType.name AS NetTermTypeName, 
                                                                      AffiliateCurrency.name AS AffiliateCurrencyName, SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) 
                                                                      AS SumItemTotalCost, ItemAccountingStatus.name AS ItemAccountingStatusName, CampaignStatus.name AS CampaignStatusName, 
                                                                      Campaign.campaign_name AS CampaignName, SUM(Item.num_units) AS NumUnits, 
                                                                      Item.cost_per_unit * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier AS CostPerUnit, Affiliate.add_code AS AffiliateAddCode, 
                                                                      CASE WHEN UnitType.name = 'CPM' THEN 'Yes' ELSE 'No' END AS IsCPM, MediaBuyer.name AS MediaBuyer, dbo.SumKeys(Item.id) 
                                                                      AS ItemIDs
                                               FROM          Item INNER JOIN
                                                                      UnitType ON Item.unit_type_id = UnitType.id INNER JOIN
                                                                      ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id INNER JOIN
                                                                      Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id INNER JOIN
                                                                      Affiliate ON Item.affid = Affiliate.affid INNER JOIN
                                                                      MediaBuyer ON Affiliate.media_buyer_id = MediaBuyer.id INNER JOIN
                                                                      NetTermType ON Affiliate.net_term_type_id = NetTermType.id INNER JOIN
                                                                      Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id INNER JOIN
                                                                      Campaign ON Item.pid = Campaign.pid INNER JOIN
                                                                      CampaignStatus ON Campaign.campaign_status_id = CampaignStatus.id LEFT OUTER JOIN
                                                                      Publisher ON Affiliate.affid = Publisher.affid
                                               GROUP BY COALESCE (Publisher.name, Affiliate.name), NetTermType.name, AffiliateCurrency.name, ItemAccountingStatus.name, CampaignStatus.name, 
                                                                      Campaign.campaign_name, Affiliate.add_code, UnitType.name, MediaBuyer.name, 
                                                                      Item.cost_per_unit * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS A PIVOT (SUM(SumItemTotalCost) FOR 
                                              ItemAccountingStatusName IN ([default], [Verified], [Approved], [Check Signed and Paid])) AS P) AS B
Where CampaignStatus='Verified'
and Publisher not in (select name from FakePublishers)
GO
PRINT N'Creating [dbo].[PublisherReportDetails]...';


GO
CREATE VIEW [dbo].[PublisherReportDetails]
AS
SELECT
	  COALESCE(Publisher.name, Affiliate.name) AS PublisherName
	 ,NetTermType.name AS NetTermTypeName
	 ,AffiliateCurrency.name AS AffiliateCurrencyName
	 ,SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS SumItemTotalCost
	 ,ItemAccountingStatus.name AS ItemAccountingStatusName
	 ,CampaignStatus.name AS CampaignStatusName
	 ,Campaign.campaign_name AS CampaignName
	 ,SUM(Item.num_units) AS NumUnits
	 ,SUM(Item.cost_per_unit * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS CostPerUnit
	 ,Affiliate.add_code AS AffiliateAddCode
	 ,CASE WHEN UnitType.name = 'CPM' THEN 'Yes' ELSE 'No' END AS IsCPM
	 ,MediaBuyer.name AS MediaBuyer
	 ,dbo.SumKeys(Item.id) AS ItemIDs
FROM
	Item
	INNER JOIN UnitType ON Item.unit_type_id = UnitType.id
	INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
	INNER JOIN Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id
	INNER JOIN Affiliate ON Item.affid = Affiliate.affid
	INNER JOIN MediaBuyer ON Affiliate.media_buyer_id = MediaBuyer.id
	INNER JOIN NetTermType ON Affiliate.net_term_type_id = NetTermType.id
	INNER JOIN Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id
	INNER JOIN Campaign ON Item.pid = Campaign.pid
	INNER JOIN CampaignStatus ON Campaign.campaign_status_id = CampaignStatus.id
	LEFT OUTER JOIN Publisher ON Affiliate.affid = Publisher.affid
GROUP BY
	 COALESCE(Publisher.name, Affiliate.name)
	,NetTermType.name
	,AffiliateCurrency.name
	,ItemAccountingStatus.name
	,CampaignStatus.name
	,Campaign.campaign_name
	,Affiliate.add_code
	,UnitType.name
	,MediaBuyer.name
GO
PRINT N'Creating [dbo].[PublisherReports]...';


GO
CREATE VIEW [dbo].[PublisherReports]
AS
SELECT *, B.[ToBePaid] + B.[Paid] AS Total
FROM
(
	SELECT
		 ItemIDs
		,CampaignStatusName AS CampaignStatus
		,PublisherName AS Publisher
		,AffiliateAddCode AS AddCode
		,CampaignName AS CampaignName
		,NumUnits AS NumUnits
		,CostPerUnit AS CostPerUnit
		,NetTermTypeName AS NetTerms
		,AffiliateCurrencyName AS PayCurrency
		,IsCPM AS [IsCPM]
		,MediaBuyer
		,COALESCE([default], 0) AS ToBePaid
		,COALESCE([Check Signed and Paid], 0) AS [Paid]
	FROM (
		SELECT * FROM PublisherReportDetails ) AS A 
	PIVOT ( 
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Check Signed and Paid])
	) AS P
) AS B
GO
PRINT N'Creating [dbo].[PublisherReportSummary]...';


GO
-- View

CREATE VIEW [dbo].[PublisherReportSummary]
AS
SELECT 
	 *
	,B.[To Be Paid] + B.Paid AS [Total]
FROM
(
	SELECT
		 PublisherName AS Name
		,NetTermTypeName AS [Net Terms]
		,AffiliateCurrencyName AS [Curr]
		,COALESCE([default], 0) AS [To Be Paid]
		,COALESCE([Check Signed and Paid], 0) AS [Paid]
	FROM
	(
		SELECT
			  COALESCE(Publisher.name, Affiliate.name) AS PublisherName
			 ,NetTermType.name AS NetTermTypeName
			 ,AffiliateCurrency.name AS AffiliateCurrencyName
			 ,SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS SumItemTotalCost
			 ,ItemAccountingStatus.name AS ItemAccountingStatusName
		FROM
			Item
			INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
			INNER JOIN Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id
			INNER JOIN Affiliate ON Item.affid = Affiliate.affid
			INNER JOIN NetTermType ON Affiliate.net_term_type_id = NetTermType.id
			INNER JOIN Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id
			INNER JOIN Campaign ON Item.pid = Campaign.pid
			INNER JOIN CampaignStatus ON Campaign.campaign_status_id = CampaignStatus.id
			LEFT OUTER JOIN Publisher ON Affiliate.affid = Publisher.affid
		WHERE
			CampaignStatus.name = 'Verified'
		GROUP BY
			 COALESCE(Publisher.name, Affiliate.name)
			,NetTermType.name
			,AffiliateCurrency.name
			,ItemAccountingStatus.name
	) AS A
	PIVOT
	(
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Check Signed and Paid])
	) AS P
) AS B
GO
PRINT N'Creating [dbo].[Rehena_Jan11EomSheetIntl]...';


GO
CREATE VIEW [dbo].[Rehena_Jan11EomSheetIntl]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.Item.cost_per_unit AS [Cost/Unit], SUM(dbo.Item.num_units) AS Units, dbo.UnitType.name AS [Unit Type], 
                      SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.Item.total_cost) AS Cost, Currency_2.name AS [Pub Pay Curr], 
                      SUM(dbo.Item.total_cost * Currency_1.to_usd_multiplier / Currency_2.to_usd_multiplier) AS [Pub Cost], SUM(dbo.Item.margin) AS Margin, 
                      dbo.MediaBuyer.name AS [Media Buyer], dbo.AdManager.name AS [Ad Manager], dbo.AccountManager.name AS [Account Manager], 
                      dbo.CampaignStatus.name AS Status, dbo.ItemAccountingStatus.name AS [Accounting Status]
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, Currency_2.name
HAVING      (dbo.CampaignStatus.name = 'Verified') AND (Currency_2.name = 'GBP' OR
                      Currency_2.name = 'EUR')
ORDER BY Publisher
GO
PRINT N'Creating [dbo].[StandardView_Accounting_1]...';


GO
CREATE VIEW [dbo].[StandardView_Accounting_1]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS PublisherName, dbo.Advertiser.name AS AdvertiserName, dbo.Campaign.pid, 
                      dbo.Campaign.campaign_name AS CampaignName, dbo.Currency.name AS RevenueCurrencyName, Currency_1.name AS CostCurrencyName, 
                      dbo.Item.revenue_per_unit AS RevenuePerUnit, dbo.Item.cost_per_unit AS CostPerUnit, SUM(dbo.Item.num_units) AS TotalUnits, dbo.UnitType.name AS UnitType, 
                      SUM(dbo.Item.total_revenue) AS TotalRevenue, SUM(dbo.Item.total_cost) AS TotalCost, SUM(dbo.Item.margin) AS TotalMargin, 
                      dbo.MediaBuyer.name AS MediaBuyerName, dbo.AdManager.name AS AdManagerName, dbo.AccountManager.name AS AccountManagerName, 
                      dbo.CampaignStatus.name AS CampaignStatusName, dbo.ItemAccountingStatus.name AS AccountingStatusName
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name
ORDER BY PublisherName
GO
PRINT N'Creating [dbo].[StandardView_CampaignRevenueTotal]...';


GO
CREATE VIEW [dbo].[StandardView_CampaignRevenueTotal]
AS
SELECT     dbo.Item.pid, dbo.Campaign.campaign_name, dbo.Item.revenue_currency_id, SUM(dbo.Item.total_revenue) AS TotalRevenueNative, 
                      SUM(dbo.Item.total_revenue * dbo.Currency.to_usd_multiplier) AS TotalRevenueUSD
FROM         dbo.Item INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id
GROUP BY dbo.Item.pid, dbo.Campaign.campaign_name, dbo.Item.revenue_currency_id
GO
PRINT N'Creating [dbo].[StandardView_CampaignRevenueTotal_CPA]...';


GO
CREATE VIEW [dbo].[StandardView_CampaignRevenueTotal_CPA]
AS
SELECT     pid, campaign_name, revenue_currency_id, TotalRevenueNative, TotalRevenueUSD
FROM         dbo.StandardView_CampaignRevenueTotal
WHERE     (campaign_name LIKE 'US%') AND (campaign_name NOT LIKE '%CPM%')
GO
PRINT N'Creating [dbo].[StandardView_CampaignRevenueTotal_CPM]...';


GO
CREATE VIEW [dbo].[StandardView_CampaignRevenueTotal_CPM]
AS
SELECT     pid, campaign_name, revenue_currency_id, TotalRevenueNative, TotalRevenueUSD
FROM         dbo.StandardView_CampaignRevenueTotal
WHERE     (campaign_name LIKE '%CPM%')
GO
PRINT N'Creating [dbo].[StandardView_CampaignRevenueTotal_Intl]...';


GO
CREATE VIEW [dbo].[StandardView_CampaignRevenueTotal_Intl]
AS
SELECT     pid, campaign_name, revenue_currency_id, TotalRevenueNative, TotalRevenueUSD
FROM         dbo.StandardView_CampaignRevenueTotal
WHERE     (NOT (campaign_name LIKE 'US%'))
GO
PRINT N'Creating [dbo].[Top45Advertisers]...';


GO
CREATE VIEW [dbo].[Top45Advertisers]
AS
SELECT     TOP (45) Advertiser, [Campaign Number], [Campaign Name], [Rev Currency], [Cost Currency], Revenue, [Revenue USD], Cost, [Cost USD]
FROM         dbo.AccountingView2
ORDER BY Revenue DESC
GO
PRINT N'Creating [dbo].[Top45Advertisers2]...';


GO
CREATE VIEW [dbo].[Top45Advertisers2]
AS
SELECT     TOP (45) Advertiser, [Rev Currency], SUM(Revenue) AS Rev, SUM([Revenue USD]) AS RevUSD
FROM         (SELECT     TOP (100) PERCENT Publisher, Advertiser, [Campaign Number], [Campaign Name], [Rev Currency], [Cost Currency], Revenue, [Revenue USD], Cost, 
                                              [Cost USD]
                       FROM          dbo.AccountingView2
                       ORDER BY Revenue DESC) AS A
GROUP BY Advertiser, [Rev Currency]
GO
PRINT N'Creating [dbo].[vAccounting]...';


GO
CREATE VIEW [dbo].[vAccounting] AS
WITH B AS
(
SELECT     
	  dbo.Affiliate.name2 AS Publisher
	, dbo.Advertiser.name AS Advertiser
	, dbo.Campaign.pid AS [Campaign Number]
	, dbo.Campaign.campaign_name AS [Campaign Name]
	, dbo.Currency.name AS [Rev Currency]
	, Currency_1.name AS [Cost Currency]
	, dbo.Item.revenue_per_unit AS [Rev/Unit]
	, dbo.tousd3(dbo.Item.revenue_currency_id
	, dbo.Item.revenue_per_unit) AS [Rev/Unit USD]
	, dbo.Item.cost_per_unit AS [Cost/Unit]
	, dbo.tousd3(dbo.Item.cost_currency_id
	, dbo.Item.cost_per_unit) AS [Cost/Unit USD]
	, SUM(dbo.Item.num_units) AS Units
	, dbo.UnitType.name AS [Unit Type]
	, SUM(dbo.Item.total_revenue) AS Revenue
	, SUM(dbo.tousd3(dbo.Item.revenue_currency_id
	, dbo.Item.revenue_per_unit) * dbo.Item.num_units) AS [Revenue USD]
	, SUM(dbo.Item.total_cost) AS Cost
	, SUM(dbo.tousd3(dbo.Item.cost_currency_id
	, dbo.Item.cost_per_unit) * dbo.Item.num_units) AS [Cost USD]
	, SUM(dbo.Item.margin) AS Margin
	, dbo.MediaBuyer.name AS [Media Buyer]
	, dbo.AdManager.name AS [Ad Manager]
	, dbo.AccountManager.name AS [Account Manager]
	, dbo.CampaignStatus.name AS [Status]
	, dbo.ItemAccountingStatus.name AS [Accounting Status]
	, dbo.NetTermType.name
	, dbo.AffiliatePaymentMethod.name AS [Aff Pay Method]
	, dbo.Affiliate.currency_id, Currency_2.name AS Expr1
	, Currency_2.name AS [Pub Pay Curr]
	, dbo.GetCampaignNotes(dbo.Campaign.pid) AS CampaignNotes
	, dbo.SumKeys(dbo.Item.id) AS ItemIDs
	, CASE WHEN item.[notes] LIKE '%from dt%' THEN '-' ELSE item.[notes] END AS ItemNotes
FROM
	dbo.Item 
	INNER JOIN dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid 
	INNER JOIN dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid 
	INNER JOIN dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id 
	INNER JOIN dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id
	INNER JOIN dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id 
	INNER JOIN dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id 
	INNER JOIN dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id 
	INNER JOIN dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id 
	INNER JOIN dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id 
	INNER JOIN dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id 
	INNER JOIN dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id 
	INNER JOIN dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id 
	INNER JOIN dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id 
	INNER JOIN dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id 
	LEFT OUTER JOIN dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id
WHERE 
	dbo.CampaignStatus.name = 'Verified'
GROUP BY 
	  dbo.Affiliate.name2
	, dbo.Advertiser.name
	, dbo.Campaign.pid
	, dbo.Campaign.campaign_name
	, dbo.Currency.name
	, Currency_1.name
	, dbo.Item.revenue_per_unit
	, dbo.Item.cost_per_unit
	, dbo.MediaBuyer.name
	, dbo.AdManager.name
	, dbo.AccountManager.name
	, dbo.ItemAccountingStatus.name
	, dbo.UnitType.name
	, dbo.CampaignStatus.name
	, dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit)
	, dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit)
	, dbo.NetTermType.name
	, dbo.AffiliatePaymentMethod.name
	, dbo.Affiliate.currency_id
	, Currency_2.name
	, CASE WHEN [item].[notes] LIKE '%from dt%' THEN '-' ELSE [item].[notes] END
)
SELECT
	  B.[Publisher]
	, B.[Advertiser]
	, B.[Campaign Number]
	, B.[Campaign Name]
	, B.[Rev Currency]
	, B.[Cost Currency]
	, B.[Rev/Unit]
	, B.[Rev/Unit USD]
	, B.[Cost/Unit]
	, B.[Cost/Unit USD]
	, B.[Units]
	, B.[Unit Type]
	, B.[Revenue]
	, B.[Revenue USD]
	, B.[Cost]
	, B.[Cost USD]
	, B.[Revenue USD] - B.[Cost USD] AS [Margin]
	, (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) / [Revenue USD] END) * 100 AS [%Margin]
	, B.[Media Buyer]
	, B.[Ad Manager]
	, B.[Account Manager]
	, B.[Status]
	, B.[Accounting Status]
	, B.[name]
	, B.[Aff Pay Method]
	, B.[Pub Pay Curr]
	, B.[Cost USD] / dbo.[Currency].[to_usd_multiplier] AS [Pub Payout]
	, dbo.[GetCampaignNotes](B.[Campaign Number]) AS [CampaignNotes]
	, B.[ItemNotes]
FROM
	B
	INNER JOIN dbo.Currency ON B.currency_id = dbo.Currency.id
GO
PRINT N'Creating [dbo].[vCampaigns]...';


GO
CREATE VIEW [dbo].[vCampaigns]
AS
SELECT     TOP (100) PERCENT dbo.Campaign.pid AS PID, dbo.Campaign.campaign_name AS Name, COALESCE (dbo.Campaign.campaign_type, 'not set') AS Type, 
                      dbo.CampaignStatus.name AS Status, dbo.AccountManager.name AS AM, dbo.AdManager.name AS AD, dbo.Advertiser.name AS Client
FROM         dbo.Campaign INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id
ORDER BY PID
GO
PRINT N'Creating [dbo].[VerifiedCampaignsPublisherReportSummary]...';


GO
CREATE VIEW [dbo].[VerifiedCampaignsPublisherReportSummary]
AS
SELECT 
	 *
	,B.[To Be Paid] + B.Paid AS [Total]
FROM
(
	SELECT
		 PublisherName AS Name
		,NetTermTypeName AS [Net Terms]
		,AffiliateCurrencyName AS [Curr]
		,COALESCE([default], 0) AS [To Be Paid]
		,COALESCE([Check Signed and Paid], 0) AS [Paid]
	FROM
	(
		SELECT
			  COALESCE(Publisher.name, Affiliate.name) AS PublisherName
			 ,NetTermType.name AS NetTermTypeName
			 ,AffiliateCurrency.name AS AffiliateCurrencyName
			 ,SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS SumItemTotalCost
			 ,ItemAccountingStatus.name AS ItemAccountingStatusName
		FROM
			Item
			INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
			INNER JOIN Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id
			INNER JOIN Affiliate ON Item.affid = Affiliate.affid
			INNER JOIN NetTermType ON Affiliate.net_term_type_id = NetTermType.id
			INNER JOIN Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id
			INNER JOIN Campaign ON Item.pid = Campaign.pid
			INNER JOIN CampaignStatus ON Campaign.campaign_status_id = CampaignStatus.id
			LEFT OUTER JOIN Publisher ON Affiliate.affid = Publisher.affid
		WHERE
			CampaignStatus.name = 'Verified'
		GROUP BY
			 COALESCE(Publisher.name, Affiliate.name)
			,NetTermType.name
			,AffiliateCurrency.name
			,ItemAccountingStatus.name
	) AS A
	PIVOT
	(
		SUM(SumItemTotalCost)
		FOR ItemAccountingStatusName in ([default], [Check Signed and Paid])
	) AS P
) AS B
GO
PRINT N'Creating [dbo].[vExtraItems]...';


GO
CREATE VIEW [dbo].[vExtraItems] AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.Item.cost_per_unit AS [Cost/Unit], SUM(dbo.Item.num_units) AS Units, dbo.UnitType.name AS [Unit Type], 
                      SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], 
                      dbo.AdManager.name AS [Ad Manager], dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, 
                      dbo.ItemAccountingStatus.name AS [Accounting Status]
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id
WHERE dbo.Item.stat_id_n is null
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name
                      
                      
ORDER BY Publisher
GO
PRINT N'Creating [dbo].[vPublisherReportEmails]...';


GO
CREATE VIEW [dbo].[vPublisherReportEmails]
AS
SELECT     dbo.Vendor.name, dbo.PubReportEmail.to_email_address, dbo.PubReportEmail.sent_date, dbo.PubReportEmail.subject, 
                      dbo.PubReportInstance.created_by_user_name, dbo.PubReportInstance.path_to_hard_copy, dbo.PubReportInstance.saved, 
                      dbo.PubReportInstance.email_status_msg
FROM         dbo.PubReportEmail INNER JOIN
                      dbo.PubReportInstance ON dbo.PubReportEmail.pub_report_instance_id = dbo.PubReportInstance.id INNER JOIN
                      dbo.Vendor ON dbo.PubReportInstance.vendor_id = dbo.Vendor.id
GO
PRINT N'Creating [dbo].[vwSub2]...';


GO
CREATE VIEW [dbo].[vwSub2]
AS
SELECT     SUM(dbo.Item.total_revenue) AS rev, dbo.Advertiser.id AS advid
FROM         dbo.Item INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id
GROUP BY dbo.Advertiser.id
GO
PRINT N'Creating [dbo].[CampaignsPublisherReportSummarySummaryByNetTerms]...';


GO
--SELECT Status, USD, EUR, GBP, AUD FROM dbo.CampaignsPublisherReportSummarySummaryByNetTerms('Net 7')
CREATE FUNCTION [dbo].[CampaignsPublisherReportSummarySummaryByNetTerms]
(	
	@p_net_terms varchar(50)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Status], SUM(USD) USD, SUM(EUR) EUR, SUM(GBP) GBP, SUM(AUD) AUD FROM (
		SELECT NetTerms, PayCurrency, [Status], Amount, CampaignStatus
		FROM CampaignsPublisherReportSummary
		UNPIVOT (Amount FOR [Status] IN (Total, ToBePaid, Paid, Approved, Verified, Unverified)) U
		WHERE NetTerms LIKE @p_net_terms
		--WHERE NetTerms LIKE 'Net 7'
		AND CampaignStatus='Verified'
	) A
	PIVOT (SUM(Amount) FOR PayCurrency IN (USD, EUR, GBP, AUD)) P
	GROUP BY [Status]
)
GO
PRINT N'Creating [dbo].[CampaignsPublisherReportSummarySummaryByNetTermsAllRevenue]...';


GO
CREATE FUNCTION [dbo].[CampaignsPublisherReportSummarySummaryByNetTermsAllRevenue]
(	
	@p_net_terms varchar(50)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Status], SUM(USD) USD, SUM(EUR) EUR, SUM(GBP) GBP, SUM(AUD) AUD FROM (
		SELECT NetTerms, PayCurrency, [Status], Amount, CampaignStatus
		FROM CampaignsPublisherReportSummary
		UNPIVOT (Amount FOR [Status] IN (Total, ToBePaid, Paid, Approved, Verified, Unverified)) U
		WHERE NetTerms LIKE @p_net_terms
		--WHERE NetTerms LIKE 'Net 7'
		--AND CampaignStatus='Verified'
	) A
	PIVOT (SUM(Amount) FOR PayCurrency IN (USD, EUR, GBP, AUD)) P
	GROUP BY [Status]
)
GO
PRINT N'Creating [dbo].[GetCampaignsPublisherReportSummaryByNetTerms]...';


GO
CREATE FUNCTION [dbo].[GetCampaignsPublisherReportSummaryByNetTerms](@p_net_terms varchar(50))
RETURNS TABLE AS
RETURN (
	SELECT 'Total' [Status], * 
	FROM(
		SELECT Total,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(Total) FOR PayCurrency in (USD,EUR,GBP,AUD))P
	UNION
	SELECT 'ToBePaid' [Status], * 
	FROM(
		SELECT ToBePaid,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(ToBePaid) FOR PayCurrency in (USD,EUR,GBP,AUD))P
	UNION
	SELECT 'Approved' [Status], * 
	FROM(
		SELECT Approved,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(Approved) FOR PayCurrency in (USD,EUR,GBP,AUD))P
	UNION
	SELECT 'Verified' [Status], * 
	FROM(
		SELECT Verified,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(Verified) FOR PayCurrency in (USD,EUR,GBP,AUD))P
	UNION
	SELECT 'Unverified' [Status], * 
	FROM(
		SELECT Unverified,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(Unverified) FOR PayCurrency in (USD,EUR,GBP,AUD))P
)
GO
PRINT N'Creating [dbo].[PROC_SELECT_Eddy_Accounting_View_2]...';


GO
---------------------------------------------------------------
-- SELECT ALL ROWS from Eddy_Accounting_View_2
--		Campaign id
--		Affiliate name2
---------------------------------------------------------------
CREATE PROCEDURE [dbo].[PROC_SELECT_Eddy_Accounting_View_2]
AS
BEGIN
	SELECT * FROM Eddy_Accounting_View_2
END
GO
PRINT N'Creating [dbo].[Affiliate_net_term_type_id_DF]...';


GO
CREATE DEFAULT [dbo].[Affiliate_net_term_type_id_DF]
    AS (3);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Affiliate_net_term_type_id_DF]', @objname = N'[dbo].[Affiliate].[net_term_type_id]';


GO
PRINT N'Creating [dbo].[Affiliate_payment_method_id_DF]...';


GO
CREATE DEFAULT [dbo].[Affiliate_payment_method_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Affiliate_payment_method_id_DF]', @objname = N'[dbo].[Affiliate].[payment_method_id]';


GO
PRINT N'Creating [dbo].[Campaign_account_manager_id_DF]...';


GO
CREATE DEFAULT [dbo].[Campaign_account_manager_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Campaign_account_manager_id_DF]', @objname = N'[dbo].[Campaign].[account_manager_id]';


GO
PRINT N'Creating [dbo].[Campaign_ad_manager_id_DF]...';


GO
CREATE DEFAULT [dbo].[Campaign_ad_manager_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Campaign_ad_manager_id_DF]', @objname = N'[dbo].[Campaign].[ad_manager_id]';


GO
PRINT N'Creating [dbo].[Campaign_advertiser_id_DF]...';


GO
CREATE DEFAULT [dbo].[Campaign_advertiser_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Campaign_advertiser_id_DF]', @objname = N'[dbo].[Campaign].[advertiser_id]';


GO
PRINT N'Creating [dbo].[Campaign_campaign_status_id_DF]...';


GO
CREATE DEFAULT [dbo].[Campaign_campaign_status_id_DF]
    AS (3);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Campaign_campaign_status_id_DF]', @objname = N'[dbo].[Campaign].[campaign_status_id]';


GO
PRINT N'Creating [dbo].[CampaignNotes_added_by_system_user_DF]...';


GO
CREATE DEFAULT [dbo].[CampaignNotes_added_by_system_user_DF]
    AS suser_sname();


GO
EXECUTE sp_bindefault @defname = N'[dbo].[CampaignNotes_added_by_system_user_DF]', @objname = N'[dbo].[CampaignNotes].[added_by_system_user]';


GO
PRINT N'Creating [dbo].[Item_accounting_notes_DF]...';


GO
CREATE DEFAULT [dbo].[Item_accounting_notes_DF]
    AS N'none';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Item_accounting_notes_DF]', @objname = N'[dbo].[Item].[accounting_notes]';


GO
PRINT N'Creating [dbo].[Item_item_accounting_status_id_DF]...';


GO
CREATE DEFAULT [dbo].[Item_item_accounting_status_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Item_item_accounting_status_id_DF]', @objname = N'[dbo].[Item].[item_accounting_status_id]';


GO
PRINT N'Creating [dbo].[PubReportInstance_email_status_msg_DF]...';


GO
CREATE DEFAULT [dbo].[PubReportInstance_email_status_msg_DF]
    AS 'unsent';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[PubReportInstance_email_status_msg_DF]', @objname = N'[dbo].[PubReportInstance].[email_status_msg]';


GO
PRINT N'Creating [dbo].[PubReportInstance_saved_DF]...';


GO
CREATE DEFAULT [dbo].[PubReportInstance_saved_DF]
    AS getdate();


GO
EXECUTE sp_bindefault @defname = N'[dbo].[PubReportInstance_saved_DF]', @objname = N'[dbo].[PubReportInstance].[saved]';


GO
PRINT N'Creating [dbo].[Stat_clicks_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_clicks_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_leads_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_leads_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_num_post_impression_sales_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_num_post_impression_sales_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_num_post_impression_sub_sales_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_num_post_impression_sub_sales_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_num_sales_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_num_sales_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_num_sub_sales_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_num_sub_sales_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_post_impression_sale_amount_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_post_impression_sale_amount_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_post_impression_sub_sale_amount_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_post_impression_sub_sale_amount_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_sale_amount_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_sale_amount_DF]
    AS (0);


GO
PRINT N'Creating [dbo].[Stat_sub_sale_amount_DF]...';


GO
CREATE DEFAULT [dbo].[Stat_sub_sale_amount_DF]
    AS (0);


GO
PRINT N'Creating [SqlServerProject1].[SqlAssemblyProjectRoot]...';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyProjectRoot', @value = N'C:\zzzNovEOM\vs2\EomApps\SqlServerProject1', @level0type = N'Assembly', @level0name = N'SqlServerProject1';


GO
PRINT N'Creating [dbo].[CampaignsMargin].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[19] 4[42] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "AccountingView2"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 514
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 2520
         Alias = 3810
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'CampaignsMargin';


GO
PRINT N'Creating [dbo].[CampaignsMargin].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'CampaignsMargin';


GO
PRINT N'Creating [dbo].[Top45Advertisers].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[6] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "AccountingView2"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Top45Advertisers';


GO
PRINT N'Creating [dbo].[Top45Advertisers].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Top45Advertisers';


GO
PRINT N'Creating [dbo].[Top45Advertisers2].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "A"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 125
               Right = 230
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Top45Advertisers2';


GO
PRINT N'Creating [dbo].[Top45Advertisers2].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'Top45Advertisers2';


GO
PRINT N'Creating [dbo].[vCampaigns].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Campaign"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 360
               Right = 259
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CampaignStatus"
            Begin Extent = 
               Top = 0
               Left = 546
               Bottom = 89
               Right = 706
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AccountManager"
            Begin Extent = 
               Top = 0
               Left = 369
               Bottom = 89
               Right = 529
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AdManager"
            Begin Extent = 
               Top = 102
               Left = 369
               Bottom = 191
               Right = 529
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Advertiser"
            Begin Extent = 
               Top = 101
               Left = 544
               Bottom = 190
               Right = 704
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 4575
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 4455
         Alia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vCampaigns';


GO
PRINT N'Creating [dbo].[vCampaigns].[MS_DiagramPane2]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane2', @value = N's = 5025
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vCampaigns';


GO
PRINT N'Creating [dbo].[vCampaigns].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 2, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vCampaigns';


GO
PRINT N'Creating [dbo].[vPublisherReportEmails].[MS_DiagramPane1]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPane1', @value = N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[5] 4[44] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PubReportEmail"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 180
               Right = 241
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "PubReportInstance"
            Begin Extent = 
               Top = 6
               Left = 279
               Bottom = 191
               Right = 482
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Vendor"
            Begin Extent = 
               Top = 6
               Left = 520
               Bottom = 95
               Right = 680
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 5490
         Width = 2535
         Width = 1500
         Width = 2145
         Width = 2805
         Width = 2235
         Width = 9555
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vPublisherReportEmails';


GO
PRINT N'Creating [dbo].[vPublisherReportEmails].[MS_DiagramPaneCount]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vPublisherReportEmails';


GO
PRINT N'Creating [dbo].[GetCampaignNotes].[SqlAssemblyFile]...';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'GetCampaignNotes.cs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'GetCampaignNotes';


GO
PRINT N'Creating [dbo].[GetCampaignNotes].[SqlAssemblyFileLine]...';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = N'14', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'GetCampaignNotes';


GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

----insert AdManager rows
--SET IDENTITY_INSERT [$(DatabaseName)].[dbo].[AdManager] ON
--INSERT 
--	[$(DatabaseName)].[dbo].[AdManager] (
--		[id], 
--		[name]
--	) 
--SELECT 
--	*
--FROM 
--	DADatabaseFeb2012.dbo.[AdManager]
--SET IDENTITY_INSERT [$(DatabaseName)].[dbo].[AdManager] OFF
----insert AccountManager rows
--SET IDENTITY_INSERT [$(DatabaseName)].[dbo].[AccountManager] ON
--INSERT 
--	[$(DatabaseName)].[dbo].[AccountManager] (
--		[id], 
--		[name]
--	) 
--SELECT 
--	*
--FROM 
--	DADatabaseFeb2012.dbo.[AccountManager]
--SET IDENTITY_INSERT [$(DatabaseName)].[dbo].[AccountManager] OFF

GO
