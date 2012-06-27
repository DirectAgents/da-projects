ALTER TABLE [dbo].[Affiliate]
    ADD CONSTRAINT [Affiliate_MediaBuyer_id_FK_MediaBuyer_id_PK] FOREIGN KEY ([media_buyer_id]) REFERENCES [dbo].[MediaBuyer] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

