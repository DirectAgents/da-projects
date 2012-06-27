CREATE DEFAULT [dbo].[Campaign_advertiser_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Campaign_advertiser_id_DF]', @objname = N'[dbo].[Campaign].[advertiser_id]';

