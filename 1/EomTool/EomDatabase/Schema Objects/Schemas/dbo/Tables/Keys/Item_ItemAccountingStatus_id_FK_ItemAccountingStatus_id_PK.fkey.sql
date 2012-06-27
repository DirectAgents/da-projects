ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_ItemAccountingStatus_id_FK_ItemAccountingStatus_id_PK] FOREIGN KEY ([item_accounting_status_id]) REFERENCES [dbo].[ItemAccountingStatus] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

