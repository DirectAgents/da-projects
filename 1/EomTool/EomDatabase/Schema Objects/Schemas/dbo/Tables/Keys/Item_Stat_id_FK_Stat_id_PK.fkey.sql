ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_Stat_id_FK_Stat_id_PK] FOREIGN KEY ([stat_id_n]) REFERENCES [dbo].[Stat] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

