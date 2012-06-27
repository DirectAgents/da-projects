CREATE DEFAULT [dbo].[Campaign_account_manager_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Campaign_account_manager_id_DF]', @objname = N'[dbo].[Campaign].[account_manager_id]';

