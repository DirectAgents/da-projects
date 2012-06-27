CREATE DEFAULT [dbo].[Item_item_accounting_status_id_DF]
    AS (1);


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Item_item_accounting_status_id_DF]', @objname = N'[dbo].[Item].[item_accounting_status_id]';

