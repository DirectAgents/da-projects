--CampaignWiki ETL (HOURLY)
---------------------------
--Run CakeUtility
C:\Share\CampaignWiki\CakeUtility\CakeUtility.exe RefreshStagingTables

--Run ApiClient
c:\Share\CampaignWiki\ApiClient\ApiClient.exe DailySummaries

--Run CampaignUpdater
c:\Share\CampaignWiki\CampaignUpdater\CampaignUpdater.exe


--syspolicy_purge_history (NIGHTLY at 2:00am)
---------------------------------------------
--Verify that automation is enabled. (t-sql)
IF (msdb.dbo.fn_syspolicy_is_automation_enabled() != 1)
        BEGIN
            RAISERROR(34022, 16, 1)
        END

--Purge history. (t-sql)
EXEC msdb.dbo.sp_syspolicy_purge_history

--Erase Phantom System Health Records. (powershell)
if ('$(ESCAPE_SQUOTE(INST))' -eq 'MSSQLSERVER') {$a = '\DEFAULT'} ELSE {$a = ''};
(Get-Item SQLSERVER:\SQLPolicy\$(ESCAPE_NONE(SRVR))$a).EraseSystemHealthPhantomRecords()


--Backup user databases (NOT ENABLED - last run 6/4/13)
-------------------------------------
--SSIS package to backup user databses.
\Backup all User Databases

--delete files more than 3 days old
(powershell commands)


--Import Tree Leads (NOT ENABLED - last run 1/14/13)
---------------------------------------
--\Import Tree Leads ---SSIS package
