ALTER TABLE [dbo].[PubReportEmail]
    ADD CONSTRAINT [PubReportEmail_PubReportInstance_id_FK_PubReportInstance_id_PK] FOREIGN KEY ([pub_report_instance_id]) REFERENCES [dbo].[PubReportInstance] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

