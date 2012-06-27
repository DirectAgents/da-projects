ALTER TABLE [dbo].[Stat]
    ADD CONSTRAINT [Stat_Payout_id_FK_Payout_id_PK1] FOREIGN KEY ([cost_payout_id]) REFERENCES [dbo].[Payout] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

