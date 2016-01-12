--insert into td.SiteSummary
select DATEADD(day, 1, SiteSummary.Date) AS Date, SiteSummary.SiteId, AdRollAccount.Id AS AccountId, SiteSummary.Impressions, SiteSummary.Clicks, SiteSummary.PostClickConv, SiteSummary.PostViewConv, SiteSummary.Cost
--, AdRollAccount.Name AS AccountName
from td.SiteSummary
inner join td.Account DBMAccount ON DBMAccount.Id = SiteSummary.AccountId
inner join td.Account AdRollAccount ON AdRollAccount.Name = DBMAccount.Name + ' (AdRoll dummy)'

--insert into td.Account (PlatformId, ExternalId, Name, CampaignId)
select distinct 1 AS PlatformId, NULL AS ExternalId, Account.Name + ' (AdRoll dummy)' AS Name, Account.CampaignId
from td.SiteSummary
inner join td.Account ON Account.Id = SiteSummary.AccountId

inner join td.Platform ON Platform.Id = Account.PlatformId
inner join td.PlatformBudgetInfo ON PlatformBudgetInfo.PlatformId = Platform.Id
inner join td.Campaign ON Campaign.Id = PlatformBudgetInfo.CampaignId
inner join td.Advertiser ON Advertiser.Id = Campaign.AdvertiserId
where (Advertiser.Name = 'Childfund')

--insert into td.PlatformBudgetInfo
select PlatformBudgetInfo.CampaignId, AdRollPlatform.Id AS PlatformId, DATEADD(day, 1, PlatformBudgetInfo.Date) AS Date, PlatformBudgetInfo.MediaSpend, PlatformBudgetInfo.MgmtFeePct, PlatformBudgetInfo.MarginPct
from td.PlatformBudgetInfo
inner join td.Platform DBMPlatform
   ON (DBMPlatform.Id = PlatformBudgetInfo.PlatformId)
  AND (DBMPlatform.Name = 'DBM')
inner join td.Campaign ON Campaign.Id = PlatformBudgetInfo.CampaignId
inner join td.Advertiser ON Advertiser.Id = Campaign.AdvertiserId
inner join td.Platform AdRollPlatform ON AdRollPlatform.Name = 'AdRoll'
where (Advertiser.Name = 'Childfund')
