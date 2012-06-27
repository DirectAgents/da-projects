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
