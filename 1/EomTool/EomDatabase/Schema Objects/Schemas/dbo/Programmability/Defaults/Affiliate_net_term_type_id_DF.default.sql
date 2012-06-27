CREATE DEFAULT [dbo].[Affiliate_net_term_type_id_DF]
    AS (3);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Affiliate_net_term_type_id_DF]', @objname = N'[dbo].[Affiliate].[net_term_type_id]';

