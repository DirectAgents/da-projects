CREATE DEFAULT [dbo].[Affiliate_payment_method_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Affiliate_payment_method_id_DF]', @objname = N'[dbo].[Affiliate].[payment_method_id]';

