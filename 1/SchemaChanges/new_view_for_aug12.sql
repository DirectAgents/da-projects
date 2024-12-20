USE [DADatabaseAug2012]
GO
/****** Object:  View [dbo].[VerifiedItemsCampaignRevenueSummary]    Script Date: 09/04/2012 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VerifiedItemsCampaignRevenueSummary]
AS
SELECT     dbo.Item.pid, dbo.Campaign.campaign_name AS CampaignName, SUM(dbo.Item.total_revenue) AS TotalRevenue, dbo.Currency.name AS CurrencyName, 
                      dbo.SumKeys(dbo.Item.id) AS ItemIds, dbo.Advertiser.name AS AdvertiserName
FROM         dbo.Campaign INNER JOIN
                      dbo.Item ON dbo.Campaign.pid = dbo.Item.pid INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Item.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id
GROUP BY dbo.Currency.name, dbo.Item.pid, dbo.Campaign.campaign_name, dbo.CampaignStatus.name, dbo.Advertiser.name
HAVING      (dbo.CampaignStatus.name = 'Verified')
GO
/****** Object:  View [dbo].[CampaignWorkflow]    Script Date: 09/04/2012 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CampaignWorkflow]
AS
SELECT     dbo.Campaign.id, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Campaign.modified, dbo.AccountManager.name AS AM, dbo.Advertiser.name AS Advertiser, 
                      dbo.Currency.name AS Curr, SUM(dbo.Item.total_revenue) AS Revenue, SUM(CASE Item.campaign_status_id WHEN 1 THEN Item.total_revenue ELSE 0 END) 
                      AS RevDefault, SUM(CASE Item.campaign_status_id WHEN 2 THEN Item.total_revenue ELSE 0 END) AS RevFinalized, 
                      SUM(CASE Item.campaign_status_id WHEN 4 THEN Item.total_revenue ELSE 0 END) AS RevVerified, dbo.CampaignStatus.name AS Status, 
                      dbo.LatestCampaignNote.note AS Note, CampaignStatus_1.name AS ItemCampaignStatus, COUNT(DISTINCT dbo.Item.affid) AS NumAffiliates, 
                      COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 1 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet7, 
                      COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 2 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet15, 
                      COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 3 THEN Item.affid ELSE NULL END) AS NumAffiliatesNet30, 
                      COUNT(DISTINCT CASE Affiliate.net_term_type_id WHEN 4 THEN Item.affid ELSE NULL END) AS NumAffiliatesNetBiWeekly, 
                      COUNT(DISTINCT CASE Item.campaign_status_id WHEN 1 THEN Item.affid ELSE NULL END) AS NumDefault, 
                      COUNT(DISTINCT CASE Item.campaign_status_id WHEN 2 THEN Item.affid ELSE NULL END) AS NumFinalized, 
                      COUNT(DISTINCT CASE Item.campaign_status_id WHEN 4 THEN Item.affid ELSE NULL END) AS NumVerified
FROM         dbo.Campaign INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Campaign.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.Item ON dbo.Campaign.pid = dbo.Item.pid INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.CampaignStatus AS CampaignStatus_1 ON dbo.Item.campaign_status_id = CampaignStatus_1.id LEFT OUTER JOIN
                      dbo.LatestCampaignNote ON dbo.Campaign.pid = dbo.LatestCampaignNote.campaign_id INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid
GROUP BY dbo.Campaign.id, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Campaign.modified, dbo.AccountManager.name, dbo.Advertiser.name, dbo.Currency.name,
                       dbo.CampaignStatus.name, dbo.LatestCampaignNote.note, CampaignStatus_1.name
GO
/****** Object:  View [dbo].[AccountingSheet2]    Script Date: 09/04/2012 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AccountingSheet2] AS
WITH T AS (
	SELECT  TOP (100) PERCENT 
		  dbo.SumKeys(item.id) ItemIds
		, dbo.Affiliate.name2 AS Publisher
		, dbo.Advertiser.name AS Advertiser
		, dbo.Campaign.pid AS [Campaign Number]
		, dbo.Campaign.campaign_name AS [Campaign Name]
		, dbo.Currency.name AS [Rev Currency]
		, Currency_1.name AS [Cost Currency]
		, dbo.Item.revenue_per_unit AS [Rev/Unit]
		, dbo.tousd3(dbo.Item.revenue_currency_id
		, dbo.Item.revenue_per_unit) AS [Rev/Unit USD]
		, dbo.Item.cost_per_unit AS [Cost/Unit]
		, dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) AS [Cost/Unit USD]
		, SUM(dbo.Item.num_units) AS Units
		, dbo.UnitType.name AS [Unit Type]
		, SUM(dbo.Item.total_revenue) AS Revenue
		, SUM(dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) * dbo.Item.num_units) AS [Revenue USD]
		, SUM(dbo.Item.total_cost) AS Cost
		, SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) * dbo.Item.num_units) AS [Cost USD]
		, SUM(dbo.Item.margin) AS Margin
		, dbo.MediaBuyer.name AS [Media Buyer]
		, dbo.AdManager.name AS [Ad Manager]
		, dbo.AccountManager.name AS [Account Manager]
		, dbo.CampaignStatus.name AS Status
		, dbo.ItemAccountingStatus.name AS [Accounting Status]
		, dbo.NetTermType.name
		, dbo.AffiliatePaymentMethod.name AS [Aff Pay Method]
		, dbo.Affiliate.currency_id
		, Currency_2.name AS Expr1
		, Currency_2.name AS [Pub Pay Curr]
		, Source.name AS Source
	FROM
		dbo.Item INNER JOIN
		dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
		dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
		dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
		dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
		dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
		dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
		dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
		dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
		dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
		dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
		dbo.CampaignStatus ON dbo.Item.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
		dbo.Source AS Source ON dbo.Item.source_id = Source.id LEFT OUTER JOIN
		dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id LEFT OUTER JOIN
		dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id LEFT OUTER JOIN
		dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id LEFT OUTER JOIN
		dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id
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
		, dbo.tousd3(dbo.Item.revenue_currency_id
		, dbo.Item.revenue_per_unit)
		, dbo.tousd3(dbo.Item.cost_currency_id
		, dbo.Item.cost_per_unit)
		, dbo.NetTermType.name
		, dbo.AffiliatePaymentMethod.name
		, dbo.Affiliate.currency_id
		, Currency_2.name
		, Source.name
	--HAVING      
		--(dbo.CampaignStatus.name = 'Verified')
	ORDER BY 
		Publisher
)
SELECT
	  T.Publisher
	, T.Advertiser
	--, T.[Campaign Number]
	, T.[Campaign Name] [Campaign]
	, T.[Rev Currency] [Rev Curr]
	, T.[Cost Currency] [Cost Curr]
	, T.[Rev/Unit]
	--, T.[Rev/Unit USD]
	, T.[Cost/Unit]
	--, T.[Cost/Unit USD]
	, T.Units
	, T.[Unit Type]
	, T.Revenue [Rev]
	--, T.[Revenue USD]
	, T.Cost
	--, T.[Cost USD]
	--, T.[Revenue USD] - T.[Cost USD] AS Margin
	, (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) / [Revenue USD] END) * 100 AS [%Margin]
	, T.Status [Campaign Status]
	, T.[Accounting Status]
	--, T.[Media Buyer], T.[Ad Manager]
	--, T.[Account Manager]
	---, T.name, T.[Aff Pay Method]
	--, T.[Pub Pay Curr]
	--, T.[Cost USD] / dbo.Currency.to_usd_multiplier AS [Pub Payout]
	--, dbo.GetCampaignNotes(T.[Campaign Number]) AS CampaignNotes
	--, T.[Source]
	, T.ItemIds
FROM 
	T INNER JOIN 
	dbo.Currency ON T.currency_id = dbo.Currency.id
GO
/****** Object:  View [dbo].[PublisherPayoutsSheet]    Script Date: 09/04/2012 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PublisherPayoutsSheet] AS
WITH T AS (
	SELECT  TOP (100) PERCENT 
		  dbo.SumKeys(item.id) ItemIds
		, dbo.Affiliate.affid
		, dbo.Affiliate.name2 AS Publisher
		, dbo.Advertiser.name AS Advertiser
		, dbo.Campaign.pid
		, dbo.Campaign.campaign_name AS [Campaign Name]
		, dbo.Currency.name AS [Rev Currency]
		, Currency_1.name AS [Cost Currency]
		, dbo.Item.revenue_per_unit AS [Rev/Unit]
		, dbo.tousd3(dbo.Item.revenue_currency_id
		, dbo.Item.revenue_per_unit) AS [Rev/Unit USD]
		, dbo.Item.cost_per_unit AS [Cost/Unit]
		, dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) AS [Cost/Unit USD]
		, SUM(dbo.Item.num_units) AS Units
		, dbo.UnitType.name AS [Unit Type]
		, SUM(dbo.Item.total_revenue) AS Revenue
		, SUM(dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) * dbo.Item.num_units) AS [Revenue USD]
		, SUM(dbo.Item.total_cost) AS Cost
		, SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) * dbo.Item.num_units) AS [Cost USD]
		, SUM(dbo.Item.margin) AS Margin
		, dbo.MediaBuyer.name AS [Media Buyer]
		, dbo.AdManager.name AS [Ad Manager]
		, dbo.AccountManager.name AS [Account Manager]
		, dbo.CampaignStatus.name AS Status
		, dbo.ItemAccountingStatus.name AS [Accounting Status]
		, dbo.NetTermType.name
		, dbo.AffiliatePaymentMethod.name AS [Aff Pay Method]
		, dbo.Affiliate.currency_id
		, Currency_2.name AS Expr1
		, Currency_2.name AS [Pub Pay Curr]
		, Source.name AS Source
	FROM
		dbo.Item INNER JOIN
		dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
		dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
		dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
		dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
		dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
		dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
		dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
		dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
		dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
		dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
		dbo.CampaignStatus ON dbo.Item.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
		dbo.Source AS Source ON dbo.Item.source_id = Source.id LEFT OUTER JOIN
		dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id LEFT OUTER JOIN
		dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id LEFT OUTER JOIN
		dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id LEFT OUTER JOIN
		dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id
	GROUP BY 
		  dbo.Affiliate.affid
		, dbo.Affiliate.name2
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
		, dbo.tousd3(dbo.Item.revenue_currency_id
		, dbo.Item.revenue_per_unit)
		, dbo.tousd3(dbo.Item.cost_currency_id
		, dbo.Item.cost_per_unit)
		, dbo.NetTermType.name
		, dbo.AffiliatePaymentMethod.name
		, dbo.Affiliate.currency_id
		, Currency_2.name
		, Source.name
	--HAVING      
		--(dbo.CampaignStatus.name = 'Verified')
	ORDER BY 
		Publisher
)
SELECT
	  T.affid
	, T.Publisher
	, T.Advertiser
	, T.pid
	, T.[Campaign Name] [Campaign]
	, T.[Rev Currency] [Rev Curr]
	, T.[Cost Currency] [Cost Curr]
	, T.[Rev/Unit]
	--, T.[Rev/Unit USD]
	, T.[Cost/Unit]
	--, T.[Cost/Unit USD]
	, T.Units
	, T.[Unit Type]
	, T.Revenue [Rev]
	--, T.[Revenue USD]
	, T.Cost
	--, T.[Cost USD]
	--, T.[Revenue USD] - T.[Cost USD] AS Margin
	, (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) / [Revenue USD] END) * 100 AS [MarginPct]
	, T.Status [Campaign Status]
	, T.[Accounting Status]
	--, T.[Media Buyer], T.[Ad Manager]
	--, T.[Account Manager]
	---, T.name, T.[Aff Pay Method]
	--, T.[Pub Pay Curr]
	--, T.[Cost USD] / dbo.Currency.to_usd_multiplier AS [Pub Payout]
	--, dbo.GetCampaignNotes(T.[Campaign Number]) AS CampaignNotes
	--, T.[Source]
	, T.ItemIds
FROM 
	T INNER JOIN 
	dbo.Currency ON T.currency_id = dbo.Currency.id
GO
/****** Object:  View [dbo].[PublisherPayouts1]    Script Date: 09/04/2012 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PublisherPayouts1]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.affid, dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid, 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) AS [Rev/Unit USD], 
                      dbo.Item.cost_per_unit AS [Cost/Unit], dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) AS [Cost/Unit USD], SUM(dbo.Item.num_units) AS Units, 
                      dbo.UnitType.name AS [Unit Type], SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) 
                      * dbo.Item.num_units) AS [Revenue USD], SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) 
                      * dbo.Item.num_units) AS [Cost USD], SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], dbo.AdManager.name AS [Ad Manager], 
                      dbo.AccountManager.name AS [Account Manager], dbo.Item.campaign_status_id AS status_id, dbo.CampaignStatus.name AS Status, 
                      dbo.Item.item_accounting_status_id AS accounting_status_id, dbo.ItemAccountingStatus.name AS [Accounting Status], dbo.Item.media_buyer_approval_status_id, 
                      dbo.MediaBuyerApprovalStatus.name AS [Media Buyer Approval Status], dbo.NetTermType.name AS [Net Terms], 
                      dbo.AffiliatePaymentMethod.name AS [Aff Pay Method], dbo.Affiliate.currency_id, Currency_2.name AS Expr1, Currency_2.name AS [Pub Pay Curr], 
                      Source.name AS Source, dbo.SumKeys(dbo.Item.id) AS ItemIds, dbo.SumKeys(dbo.Item.batch_id) AS BatchIds
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid INNER JOIN
                      dbo.Advertiser ON dbo.Campaign.advertiser_id = dbo.Advertiser.id INNER JOIN
                      dbo.Currency ON dbo.Item.revenue_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.cost_currency_id = Currency_1.id INNER JOIN
                      dbo.AccountManager ON dbo.Campaign.account_manager_id = dbo.AccountManager.id INNER JOIN
                      dbo.AdManager ON dbo.Campaign.ad_manager_id = dbo.AdManager.id INNER JOIN
                      dbo.ItemReportingStatus ON dbo.Item.item_reporting_status_id = dbo.ItemReportingStatus.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.UnitType ON dbo.Item.unit_type_id = dbo.UnitType.id INNER JOIN
                      dbo.CampaignStatus ON dbo.Item.campaign_status_id = dbo.CampaignStatus.id INNER JOIN
                      dbo.Source AS Source ON dbo.Item.source_id = Source.id INNER JOIN
                      dbo.MediaBuyerApprovalStatus ON dbo.Item.media_buyer_approval_status_id = dbo.MediaBuyerApprovalStatus.id LEFT OUTER JOIN
                      dbo.MediaBuyer ON dbo.Affiliate.media_buyer_id = dbo.MediaBuyer.id LEFT OUTER JOIN
                      dbo.Currency AS Currency_2 ON dbo.Affiliate.currency_id = Currency_2.id LEFT OUTER JOIN
                      dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id LEFT OUTER JOIN
                      dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id
GROUP BY dbo.Affiliate.name2, dbo.Advertiser.name, dbo.Campaign.pid, dbo.Campaign.campaign_name, dbo.Currency.name, Currency_1.name, dbo.Item.revenue_per_unit, 
                      dbo.Item.cost_per_unit, dbo.MediaBuyer.name, dbo.AdManager.name, dbo.AccountManager.name, dbo.ItemAccountingStatus.name, dbo.UnitType.name, 
                      dbo.CampaignStatus.name, dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit), dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit), 
                      dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name, dbo.Affiliate.currency_id, Currency_2.name, Source.name, dbo.Affiliate.affid, 
                      dbo.MediaBuyerApprovalStatus.name, dbo.Item.media_buyer_approval_status_id, dbo.Item.campaign_status_id, dbo.Item.item_accounting_status_id
ORDER BY Publisher
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
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
         Configuration = "(H (1[50] 2[25] 3) )"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1[56] 3) )"
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
         Configuration = "(H (1[50] 4) )"
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
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -122
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Item"
            Begin Extent = 
               Top = 47
               Left = 920
               Bottom = 417
               Right = 1153
            End
            DisplayFlags = 280
            TopColumn = 2
         End
         Begin Table = "Affiliate"
            Begin Extent = 
               Top = 465
               Left = 1014
               Bottom = 763
               Right = 1217
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Campaign"
            Begin Extent = 
               Top = 246
               Left = 38
               Bottom = 365
               Right = 275
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "Advertiser"
            Begin Extent = 
               Top = 126
               Left = 279
               Bottom = 215
               Right = 455
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Currency"
            Begin Extent = 
               Top = 216
               Left = 279
               Bottom = 320
               Right = 465
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Currency_1"
            Begin Extent = 
               Top = 366
               Left = 38
               Bottom = 470
               Right = 224
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AccountManager"
            Begin Extent = 
               Top = 456
               Left = 262
               Bottom = 545
               Right = 438
            End
        ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublisherPayouts1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AdManager"
            Begin Extent = 
               Top = 474
               Left = 38
               Bottom = 563
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ItemReportingStatus"
            Begin Extent = 
               Top = 546
               Left = 252
               Bottom = 635
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ItemAccountingStatus"
            Begin Extent = 
               Top = 564
               Left = 38
               Bottom = 653
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "UnitType"
            Begin Extent = 
               Top = 636
               Left = 252
               Bottom = 725
               Right = 428
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "CampaignStatus"
            Begin Extent = 
               Top = 654
               Left = 38
               Bottom = 743
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Source"
            Begin Extent = 
               Top = 282
               Left = 571
               Bottom = 371
               Right = 747
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MediaBuyerApprovalStatus"
            Begin Extent = 
               Top = 236
               Left = 1191
               Bottom = 325
               Right = 1367
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "MediaBuyer"
            Begin Extent = 
               Top = 432
               Left = 730
               Bottom = 521
               Right = 906
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Currency_2"
            Begin Extent = 
               Top = 521
               Left = 619
               Bottom = 625
               Right = 805
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "NetTermType"
            Begin Extent = 
               Top = 645
               Left = 622
               Bottom = 734
               Right = 798
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "AffiliatePaymentMethod"
            Begin Extent = 
               Top = 745
               Left = 800
               Bottom = 834
               Right = 976
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 2835
         Alias = 2730
         Table = 2190
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublisherPayouts1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublisherPayouts1'
GO
/****** Object:  View [dbo].[CampaignFinalizationAudit]    Script Date: 09/04/2012 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CampaignFinalizationAudit]
AS
SELECT     TOP (100) PERCENT dbo.Campaign.campaign_name AS Campaign, dbo.CampaignStatus.name AS [From], CampaignStatus_1.name AS [To], 
                      dbo.Audit.SysUser AS [User], CONVERT(varchar, dbo.Audit.AuditDate, 101) AS Date, CONVERT(varchar, dbo.Audit.AuditDate, 108) AS Time
FROM         dbo.Item INNER JOIN
                      dbo.Audit ON dbo.Item.id = dbo.Audit.PrimaryKey AND dbo.Audit.TableName = 'dbo.Item' AND dbo.Audit.ColumnName = '[campaign_status_id]' INNER JOIN
                      dbo.CampaignStatus ON dbo.Audit.OldValue = dbo.CampaignStatus.id INNER JOIN
                      dbo.CampaignStatus AS CampaignStatus_1 ON dbo.Audit.NewValue = CampaignStatus_1.id INNER JOIN
                      dbo.Campaign ON dbo.Item.pid = dbo.Campaign.pid
WHERE     (dbo.Audit.Operation = 'u')
GROUP BY dbo.CampaignStatus.name, CampaignStatus_1.name, CONVERT(varchar, dbo.Audit.AuditDate, 101), dbo.Campaign.campaign_name, dbo.Audit.SysUser, 
                      CONVERT(varchar, dbo.Audit.AuditDate, 108)
ORDER BY Date DESC, Time DESC
GO
/****** Object:  View [dbo].[PublisherPayouts]    Script Date: 09/04/2012 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PublisherPayouts]
AS
SELECT     dbo.PublisherPayouts1.affid, dbo.PublisherPayouts1.Publisher, dbo.PublisherPayouts1.Advertiser, dbo.PublisherPayouts1.pid, 
                      dbo.PublisherPayouts1.[Campaign Name], dbo.PublisherPayouts1.[Rev Currency], dbo.PublisherPayouts1.[Cost Currency], dbo.PublisherPayouts1.[Rev/Unit], 
                      dbo.PublisherPayouts1.[Rev/Unit USD], dbo.PublisherPayouts1.[Cost/Unit], dbo.PublisherPayouts1.[Cost/Unit USD], dbo.PublisherPayouts1.Units, 
                      dbo.PublisherPayouts1.[Unit Type], dbo.PublisherPayouts1.Revenue, dbo.PublisherPayouts1.[Revenue USD], dbo.PublisherPayouts1.Cost, 
                      dbo.PublisherPayouts1.[Cost USD], dbo.PublisherPayouts1.[Revenue USD] - dbo.PublisherPayouts1.[Cost USD] AS Margin, 
                      (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) / [Revenue USD] END) * 100 AS MarginPct, dbo.PublisherPayouts1.[Media Buyer], 
                      dbo.PublisherPayouts1.[Ad Manager], dbo.PublisherPayouts1.[Account Manager], dbo.PublisherPayouts1.status_id, dbo.PublisherPayouts1.Status, 
                      dbo.PublisherPayouts1.accounting_status_id, dbo.PublisherPayouts1.[Accounting Status], dbo.PublisherPayouts1.media_buyer_approval_status_id, 
                      dbo.PublisherPayouts1.[Media Buyer Approval Status], dbo.PublisherPayouts1.[Net Terms], dbo.PublisherPayouts1.[Aff Pay Method], 
                      dbo.PublisherPayouts1.[Pub Pay Curr], dbo.PublisherPayouts1.[Cost USD] / dbo.Currency.to_usd_multiplier AS [Pub Payout], 
                      dbo.GetCampaignNotes(dbo.PublisherPayouts1.pid) AS CampaignNotes, dbo.PublisherPayouts1.Source, dbo.PublisherPayouts1.ItemIds, dbo.PublisherPayouts1.BatchIds
FROM         dbo.PublisherPayouts1 INNER JOIN
                      dbo.Currency ON dbo.PublisherPayouts1.currency_id = dbo.Currency.id
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
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
         Configuration = "(H (1[46] 4[28] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1[48] 4) )"
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
      ActivePaneConfig = 9
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "PublisherPayouts1"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 362
               Right = 294
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "Currency"
            Begin Extent = 
               Top = 7
               Left = 376
               Bottom = 111
               Right = 546
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
      PaneHidden = 
   End
   Begin DataPane = 
      PaneHidden = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 3375
         Alias = 900
         Table = 1545
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublisherPayouts'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PublisherPayouts'
GO
/****** Object:  View [dbo].[CostCurrencyDifferentThanRevenueCurrency]    Script Date: 09/04/2012 17:30:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[CostCurrencyDifferentThanRevenueCurrency]
AS
SELECT     dbo.AccountingView2.*
FROM         dbo.AccountingView2
WHERE     ([Rev Currency] <> [Cost Currency])
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[20] 2[6] 3) )"
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
               Bottom = 341
               Right = 214
            End
            DisplayFlags = 280
            TopColumn = 4
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
         Width = 3210
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
         Filter = 3150
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CostCurrencyDifferentThanRevenueCurrency'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'CostCurrencyDifferentThanRevenueCurrency'
GO
