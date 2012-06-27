CREATE DEFAULT [dbo].[Campaign_campaign_status_id_DF]
    AS (3);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Campaign_campaign_status_id_DF]', @objname = N'[dbo].[Campaign].[campaign_status_id]';

