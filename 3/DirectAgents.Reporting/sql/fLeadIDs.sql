alter FUNCTION [td].[fLeadIDs](
@AdvertiserId int
, @StartDate datetime
, @EndDate datetime
) RETURNS TABLE AS RETURN
SELECT Strategy.Name AS StrategyName
, Strategy.Id AS StrategyId
, Ad.Name AS AdName
, Ad.Id AS AdId
, Conv.Time
, CASE Conv.ConvType WHEN 'c' THEN 'ClickThru' WHEN 'v' THEN 'ViewThru' END AS ConvType
, Conv.ConvVal
, CASE Conv.ExtData WHEN '' THEN NULL ELSE Conv.ExtData END AS LeadID
, ConvCity.Name AS City
, ConvCountry.Name AS Country
, Conv.IP
FROM td.Conv
LEFT OUTER JOIN td.ConvCity ON ConvCity.Id = Conv.CityId
LEFT OUTER JOIN td.ConvCountry ON ConvCountry.Id = ConvCity.CountryId
-- If Conv.StrategyId is not null
LEFT OUTER JOIN td.Strategy ON Strategy.Id = Conv.StrategyId
-- If Conv.TDadId is not null
LEFT OUTER JOIN td.Ad ON Ad.Id = Conv.TDadId
LEFT OUTER JOIN td.Account ON Account.Id IN (Strategy.AccountId, Ad.AccountId)
LEFT OUTER JOIN td.Campaign ON Campaign.Id = Account.CampaignId
LEFT OUTER JOIN td.Advertiser ON Advertiser.Id = Campaign.AdvertiserId
WHERE (Advertiser.Id = @AdvertiserId)
  AND (Conv.Time BETWEEN @StartDate AND @EndDate)
