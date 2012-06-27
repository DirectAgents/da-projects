ALTER TABLE [dbo].[ItemVerificationDate]
    ADD CONSTRAINT [FK_ItemVerificationDate_Item] FOREIGN KEY ([item_id]) REFERENCES [dbo].[Item] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

