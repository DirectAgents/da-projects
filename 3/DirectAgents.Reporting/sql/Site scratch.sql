select distinct Site.Name, SiteSummary.Date, Account.Name AS Account
, Advertiser.Id AS AdvertiserId, Advertiser.Name AS Advertiser
, Platform.Name AS Platform, Platform.Id As PlatformId
, pbiAdvertiser.Name AS PBIAdvertiser, pbiAdvertiser.Id AS PBIAdvertiserId
--, exAdvertiser.Name AS ExtraItemAdvertiser
--, *
from td.SiteSummary
inner join td.Site ON Site.Id = SiteSummary.SiteId
inner join td.Account ON SiteSummary.AccountId = Account.Id

left outer join td.Platform ON Platform.Id = Account.PlatformId
left outer join td.PlatformBudgetInfo ON PlatformBudgetInfo.PlatformId = Platform.Id
left outer join td.Campaign pbiCampaign ON pbiCampaign.Id = PlatformBudgetInfo.CampaignId
left outer join td.Advertiser pbiAdvertiser ON pbiAdvertiser.Id = pbiCampaign.AdvertiserId

left outer join td.Campaign ON Campaign.Id = Account.CampaignId
left outer join td.Advertiser ON Advertiser.Id = Campaign.AdvertiserId

--left outer join td.ExtraItem ON ExtraItem.PlatformId = Platform.Id
--left outer join td.Campaign exCampaign ON exCampaign.Id = ExtraItem.CampaignId
--left outer join td.Advertiser exAdvertiser ON exAdvertiser.Id = exCampaign.AdvertiserId


select distinct Platform.Name
from fBudgetInfo(3, default, default) budgetInfo
	INNER JOIN td.SiteSummary ON td.SiteSummary.AccountId = budgetInfo.AccountId
	INNER JOIN td.Site ON td.Site.Id = td.SiteSummary.SiteId
	LEFT OUTER JOIN td.Platform ON Platform.Id = budgetInfo.PlatformId

select count(DISTINCT SiteId) AS DistinctSites, PlatformName
from td.fSiteSummaryBasicStats(3, '1/1/2016', '1/11/2016')
group by PlatformName

select *
from td.fSiteSummaryBasicStats(3, '1/1/2016', '1/11/2016')
order by PlatformName, CTR DESC

