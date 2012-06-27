ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_Currency_id_FK_Currency_id_PK1] FOREIGN KEY ([cost_currency_id]) REFERENCES [dbo].[Currency] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

