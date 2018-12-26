USE [master];
GO

IF (DB_ID(N'{%OLD_DATABASE_NAME%}') IS NULL) 
BEGIN
    raiserror(N'Source database [{%OLD_DATABASE_NAME%}] does not exists. Script execution will be aborted', 20, -1) with log
END
GO

IF (DB_ID(N'{%NEW_DATABASE_NAME%}') IS NULL) 
BEGIN
    raiserror(N'Target database [{%NEW_DATABASE_NAME%}] does not exists. Script execution will be aborted', 20, -1) with log
END
GO

-- *TODO: SET APPROPRIATE DATABASES!*
USE [{%NEW_DATABASE_NAME%}]

--Copy from previous month; *SET DATABASES!*
exec [{%COMMON_DATABASE_NAME%}].dbo.EOMcopy '{%OLD_DATABASE_NAME%}','{%NEW_DATABASE_NAME%}'
--(also clears affiliate dates and updates campaign statuses)


--TODO: Set from menu... Query -> SQLCMD Mode
--also, make sure correct database is still set, particularly if executing one script at a time

--Initialize audit (changes current db)
:r "{%AUDIT_PATH%}\autoaudit 2.00h.sql"

USE [{%NEW_DATABASE_NAME%}]

--Add audits
:r "{%AUDIT_PATH%}\AddAuditTables.sql"

--Audit views
:r "{%AUDIT_PATH%}\AddAuditViews.sql"
