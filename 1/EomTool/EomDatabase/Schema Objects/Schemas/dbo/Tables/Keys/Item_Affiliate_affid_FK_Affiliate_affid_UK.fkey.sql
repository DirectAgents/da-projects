ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_Affiliate_affid_FK_Affiliate_affid_UK] FOREIGN KEY ([affid]) REFERENCES [dbo].[Affiliate] ([affid]) ON DELETE NO ACTION ON UPDATE NO ACTION;

