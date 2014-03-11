--Disable triggers
DISABLE TRIGGER tr_Affiliate_IU ON Affiliate

--Prepare tables
delete from Source
delete from UnitType
delete from ItemAccountingStatus
delete from ItemReportingStatus
delete from Campaign
delete from Affiliate
delete from MediaBuyer
delete from AccountManager
delete from AdManager
delete from Advertiser
delete from Vendor
delete from CampaignStatus
delete from DTCampaignStatus
delete from Currency
delete from NetTermType
delete from AffiliatePaymentMethod

--Copy 1
AccountManager
AdManager
Advertiser
AffiliatePaymentMethod
Currency
CampaignStatus
ItemAccountingStatus
ItemReportingStatus
DTCampaignStatus
MediaBuyer
NetTermType
Source
UnitType
Vendor

--Copy 2
Campaign
Affiliate

--Update campaign status
UPDATE dbo.Campaign
SET    campaign_status_id = 3
WHERE (campaign_status_id = 2) OR (campaign_status_id = 4)

--Enable Triggers
ENABLE TRIGGER tr_Affiliate_IU ON Affiliate

--(In 1/SchemaChanges/AuditScripts...)

--Initialize audit
autoaudit 2.00h.sql

--Add audits
AddAuditTables.sql

--Audit views
CampaignFinalizationAudit.view.sql
