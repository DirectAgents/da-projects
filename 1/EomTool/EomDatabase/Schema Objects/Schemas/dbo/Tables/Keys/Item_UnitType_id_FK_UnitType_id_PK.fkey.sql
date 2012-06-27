ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_UnitType_id_FK_UnitType_id_PK] FOREIGN KEY ([unit_type_id]) REFERENCES [dbo].[UnitType] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

