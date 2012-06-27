ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_Source_id_FK_Source_id_PK] FOREIGN KEY ([source_id]) REFERENCES [dbo].[Source] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

