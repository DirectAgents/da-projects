EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'accountManagersOnly';
GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'DIRECTAGENTS\EGil';
GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'DIRECTAGENTS\RAkhtar';
GO
