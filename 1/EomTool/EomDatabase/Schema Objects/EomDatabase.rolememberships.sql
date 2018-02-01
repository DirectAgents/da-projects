EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'accountManagersOnly';
GO
EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'accountManagersOnly';
GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'accounting';
GO
