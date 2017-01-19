-- *TODO: SET APPROPRIATE DATABASES!*
USE DADatabaseB...2017

--Disable triggers
DISABLE TRIGGER tr_Affiliate_IU ON Affiliate

--Copy from previous month; *SET DATABASES!*
exec DAMain1.dbo.EOMcopy 'DADatabaseA...2017','DADatabaseB...2017'

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

--Initialize audit (changes current db)
:r "C:\GitHub\da-projects-kevin\1\SchemaChanges\AuditScripts\autoaudit 2.00h.sql"

USE DADatabaseB...2017

--Add audits
:r "C:\GitHub\da-projects-kevin\1\SchemaChanges\AuditScripts\AddAuditTables.sql"

--Audit views
:r "C:\GitHub\da-projects-kevin\1\SchemaChanges\AuditScripts\CampaignFinalizationAudit.view.sql"
