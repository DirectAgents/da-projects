
CREATE VIEW [dbo].[CommissionView] AS
SELECT     dbo.AccountingView1.Publisher,
           CASE WHEN [Adv Alt Name] <> '' THEN [Adv Alt Name] ELSE dbo.AccountingView1.Advertiser END AS [Advertiser],
           dbo.AccountingView1.[Campaign Number], dbo.AccountingView1.[Campaign Name],
           dbo.AccountingView1.[Rev/Unit USD], dbo.AccountingView1.[Cost/Unit USD], dbo.AccountingView1.Units,
           dbo.AccountingView1.[Revenue USD], dbo.AccountingView1.[Cost USD],
           dbo.AccountingView1.[Revenue USD] - dbo.AccountingView1.[Cost USD] AS Margin,
           dbo.AccountingView1.[Media Buyer], '' AS [MB CommRate], '' AS [MB Commissions],
           dbo.AccountingView1.Analysts AS OldAnalysts,
           ISNULL(dbo.AccountingView1.Analyst,'') AS Analyst,
           AccountingView1.[AN CommRate],
           CASE WHEN [AN CommRate] IS NULL THEN 0 ELSE [AN CommRate] * Margin END AS [AN Commissions],
	       ISNULL(dbo.AccountingView1.[Analyst Manager],'') AS [Analyst Manager],
           dbo.AccountingView1.[ANMGR CommRate],
           CASE WHEN [ANMGR CommRate] IS NULL THEN 0 ELSE [ANMGR CommRate] * Margin END AS [ANMGR Commissions],
           ISNULL(dbo.AccountingView1.[Strategist],'') AS Strategist,
           dbo.AccountingView1.[STR CommRate], CASE WHEN [STR CommRate] IS NULL THEN 0 ELSE [STR CommRate] * Margin END AS [STR Commissions],
           dbo.AccountingView1.[Ad Manager], '' AS [AD CommRate], '' AS [AD Commissions],
           dbo.AccountingView1.[Account Manager], '' AS [AM CommRate], '' AS [AM Commissions],
           (CASE WHEN [Revenue USD] = 0 THEN 0 ELSE ([Revenue USD] - [Cost USD]) / [Revenue USD] END) * 100 AS [%Margin],
           dbo.AccountingView1.[Unit Type], dbo.AccountingView1.Status
FROM       dbo.AccountingView1
