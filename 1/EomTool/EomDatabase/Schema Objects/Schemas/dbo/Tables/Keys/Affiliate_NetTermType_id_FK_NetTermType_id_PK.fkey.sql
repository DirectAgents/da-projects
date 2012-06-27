ALTER TABLE [dbo].[Affiliate]
    ADD CONSTRAINT [Affiliate_NetTermType_id_FK_NetTermType_id_PK] FOREIGN KEY ([net_term_type_id]) REFERENCES [dbo].[NetTermType] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

