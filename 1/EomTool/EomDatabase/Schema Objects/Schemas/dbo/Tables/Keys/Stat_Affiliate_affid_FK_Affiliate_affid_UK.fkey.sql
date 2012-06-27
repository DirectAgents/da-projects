ALTER TABLE [dbo].[Stat]
    ADD CONSTRAINT [Stat_Affiliate_affid_FK_Affiliate_affid_UK] FOREIGN KEY ([affid]) REFERENCES [dbo].[Affiliate] ([affid]) ON DELETE NO ACTION ON UPDATE NO ACTION;

