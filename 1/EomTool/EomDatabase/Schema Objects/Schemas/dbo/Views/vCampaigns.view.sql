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
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
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
         Alia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vCampaigns'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N's = 5025
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
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vCampaigns'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vCampaigns'

