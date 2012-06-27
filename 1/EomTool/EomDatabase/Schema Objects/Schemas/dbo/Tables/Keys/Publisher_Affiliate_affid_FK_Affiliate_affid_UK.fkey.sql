ALTER TABLE [dbo].[Publisher]
    ADD CONSTRAINT [Publisher_Affiliate_affid_FK_Affiliate_affid_UK] FOREIGN KEY ([affid]) REFERENCES [dbo].[Affiliate] ([affid]) ON DELETE NO ACTION ON UPDATE NO ACTION;

