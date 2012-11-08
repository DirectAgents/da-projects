CREATE VIEW [dbo].[AccountingView1]
AS
SELECT     TOP (100) PERCENT dbo.Affiliate.name2 AS Publisher, dbo.Advertiser.name AS Advertiser, dbo.Campaign.pid AS [Campaign Number], 
                      dbo.Campaign.campaign_name AS [Campaign Name], dbo.Currency.name AS [Rev Currency], Currency_1.name AS [Cost Currency], 
                      dbo.Item.revenue_per_unit AS [Rev/Unit], dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) AS [Rev/Unit USD], 
                      dbo.Item.cost_per_unit AS [Cost/Unit], dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) AS [Cost/Unit USD], SUM(dbo.Item.num_units) AS Units, 
                      dbo.UnitType.name AS [Unit Type], SUM(dbo.Item.total_revenue) AS Revenue, SUM(dbo.tousd3(dbo.Item.revenue_currency_id, dbo.Item.revenue_per_unit) 
                      * dbo.Item.num_units) AS [Revenue USD], SUM(dbo.Item.total_cost) AS Cost, SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) 
                      * dbo.Item.num_units) AS [Cost USD], SUM(dbo.Item.margin) AS Margin, dbo.MediaBuyer.name AS [Media Buyer], dbo.AdManager.name AS [Ad Manager], 
                      dbo.AccountManager.name AS [Account Manager], dbo.CampaignStatus.name AS Status, dbo.MediaBuyerApprovalStatus.name AS [MediaBuyerApproval Status], 
                      dbo.ItemAccountingStatus.name AS [Accounting Status], dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name AS [Aff Pay Method], dbo.Affiliate.currency_id, 
                      Currency_2.name AS Expr1, Currency_2.name AS [Pub Pay Curr], Source.name AS Source
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
                      dbo.NetTermType.name, dbo.AffiliatePaymentMethod.name, dbo.Affiliate.currency_id, Currency_2.name, Source.name, dbo.MediaBuyerApprovalStatus.name
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
         Configuration = "(H (1[47] 4) )"
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
         Begin Table = "Item"
            Begin Extent = 
               Top = 47
               Left = 920
               Bottom = 417
               Right = 1153
            End
            DisplayFlags = 280
            TopColumn = 0
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
            TopColumn = 0
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
           ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AccountingView1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N' DisplayFlags = 280
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
         Begin Table = "MediaBuyerApprovalStatus"
            Begin Extent = 
               Top = 46
               Left = 517
               Bottom = 135
               Right = 693
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
         Column = 1440
         Alias = 2340
         Table = 2175
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AccountingView1'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'AccountingView1'
