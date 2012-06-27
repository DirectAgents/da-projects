CREATE DEFAULT [dbo].[Item_accounting_notes_DF]
    AS N'none';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[Item_accounting_notes_DF]', @objname = N'[dbo].[Item].[accounting_notes]';

