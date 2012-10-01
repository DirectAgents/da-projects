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
