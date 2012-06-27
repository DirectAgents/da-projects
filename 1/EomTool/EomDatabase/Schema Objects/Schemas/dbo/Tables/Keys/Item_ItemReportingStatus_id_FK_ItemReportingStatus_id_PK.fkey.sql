ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_ItemReportingStatus_id_FK_ItemReportingStatus_id_PK] FOREIGN KEY ([item_reporting_status_id]) REFERENCES [dbo].[ItemReportingStatus] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

