CREATE VIEW [dbo].[vPublisherReportEmails]
AS
SELECT     dbo.Vendor.name, dbo.PubReportEmail.to_email_address, dbo.PubReportEmail.sent_date, dbo.PubReportEmail.subject, 
                      dbo.PubReportInstance.created_by_user_name, dbo.PubReportInstance.path_to_hard_copy, dbo.PubReportInstance.saved, 
                      dbo.PubReportInstance.email_status_msg
FROM         dbo.PubReportEmail INNER JOIN
                      dbo.PubReportInstance ON dbo.PubReportEmail.pub_report_instance_id = dbo.PubReportInstance.id INNER JOIN
                      dbo.Vendor ON dbo.PubReportInstance.vendor_id = dbo.Vendor.id

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
EXECUTE sp_addextendedproperty @name = N'MS_DiagramPaneCount', @value = 1, @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'VIEW', @level1name = N'vPublisherReportEmails';

