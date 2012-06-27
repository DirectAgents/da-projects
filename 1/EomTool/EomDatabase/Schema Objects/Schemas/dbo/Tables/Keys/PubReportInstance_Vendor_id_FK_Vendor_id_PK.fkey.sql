ALTER TABLE [dbo].[PubReportInstance]
    ADD CONSTRAINT [PubReportInstance_Vendor_id_FK_Vendor_id_PK] FOREIGN KEY ([vendor_id]) REFERENCES [dbo].[Vendor] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

