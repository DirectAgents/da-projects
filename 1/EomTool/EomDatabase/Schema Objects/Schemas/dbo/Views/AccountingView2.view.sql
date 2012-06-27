﻿CREATE VIEW AccountingView2 AS
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