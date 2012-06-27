CREATE DEFAULT [dbo].[Campaign_ad_manager_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Campaign_ad_manager_id_DF]', @objname = N'[dbo].[Campaign].[ad_manager_id]';

