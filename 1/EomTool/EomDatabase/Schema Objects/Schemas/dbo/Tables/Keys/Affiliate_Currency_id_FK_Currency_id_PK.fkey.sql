ALTER TABLE [dbo].[Affiliate]
    ADD CONSTRAINT [Affiliate_Currency_id_FK_Currency_id_PK] FOREIGN KEY ([currency_id]) REFERENCES [dbo].[Currency] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

