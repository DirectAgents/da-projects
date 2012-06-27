ALTER TABLE [dbo].[Affiliate]
    ADD CONSTRAINT [Affiliate_AffiliatePaymentMethod_id_FK_AffiliatePaymentMethod_id_PK] FOREIGN KEY ([payment_method_id]) REFERENCES [dbo].[AffiliatePaymentMethod] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

