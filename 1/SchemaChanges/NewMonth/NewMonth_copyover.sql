-- *TODO: SET APPROPRIATE DATABASES!*
-- USE...

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

--Copy from previous month; *SET DATABASES!*
exec DAMain1.dbo.EOMcopy 'DADatabaseA...2014','DADatabaseB...2014'

--Clear affiliate dates
UPDATE Affiliate SET date_created=NULL, date_modified=NULL

--Update campaign status
UPDATE dbo.Campaign
SET    campaign_status_id = 3
WHERE (campaign_status_id = 2) OR (campaign_status_id = 4)

--Enable Triggers
ENABLE TRIGGER tr_Affiliate_IU ON Affiliate

--TODO: Set from menu... Query -> SQLCMD Mode
--also, make sure correct database is still set, particularly if executing one script at a time

--Initialize audit
:r "C:\GitHub\da-projects-kevin\1\SchemaChanges\AuditScripts\autoaudit 2.00h.sql"

--Add audits
:r "C:\GitHub\da-projects-kevin\1\SchemaChanges\AuditScripts\AddAuditTables.sql"

--Audit views
:r "C:\GitHub\da-projects-kevin\1\SchemaChanges\AuditScripts\CampaignFinalizationAudit.view.sql"
