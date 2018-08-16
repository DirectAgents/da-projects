-- *TODO: SET APPROPRIATE DATABASES!*
USE DADatabaseB...2018

--Copy from previous month; *SET DATABASES!*
exec DAMain1.dbo.EOMcopy 'DADatabaseA...2018','DADatabaseB...2018'
--(also clears affiliate dates and updates campaign statuses)


--TODO: Set from menu... Query -> SQLCMD Mode
--also, make sure correct database is still set, particularly if executing one script at a time

--Initialize audit (changes current db)
:r "G:\GitHub\da-projects\1\SchemaChanges\AuditScripts\autoaudit 2.00h.sql"

USE DADatabaseB...2018

--Add audits
:r "G:\GitHub\da-projects\1\SchemaChanges\AuditScripts\AddAuditTables.sql"

--Audit views
:r "G:\GitHub\da-projects\1\SchemaChanges\AuditScripts\AddAuditViews.sql"
