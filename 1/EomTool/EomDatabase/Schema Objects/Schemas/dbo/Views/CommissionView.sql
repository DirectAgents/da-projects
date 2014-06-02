CREATE VIEW [dbo].[CommissionView] AS
SELECT     dbo.AccountingView1.Publisher, dbo.AccountingView1.Advertiser,
           dbo.AccountingView1.[Campaign Number], dbo.AccountingView1.[Campaign Name],
           dbo.AccountingView1.[Rev/Unit USD], dbo.AccountingView1.[Cost/Unit USD], dbo.AccountingView1.Units,
           dbo.AccountingView1.[Revenue USD], dbo.AccountingView1.[Cost USD],
           dbo.AccountingView1.[Revenue USD] - dbo.AccountingView1.[Cost USD] AS Margin,
           dbo.AccountingView1.[Media Buyer], '' AS [MB Commission Rate], '' AS [MB Commissions],
           dbo.AccountingView1.[Ad Manager], '' AS [AD Commission Rate], '' AS [AD Commissions],
           dbo.AccountingView1.[Account Manager],
           (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) / [Revenue USD] END) * 100 AS [%Margin],
           dbo.AccountingView1.[Unit Type], dbo.AccountingView1.Status
FROM       dbo.AccountingView1
