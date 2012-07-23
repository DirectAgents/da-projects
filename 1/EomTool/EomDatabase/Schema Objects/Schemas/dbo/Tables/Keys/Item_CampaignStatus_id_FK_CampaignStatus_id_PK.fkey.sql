ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_CampaignStatus_id_FK_CampaignStatus_id_PK] FOREIGN KEY ([campaign_status_id]) REFERENCES [dbo].[CampaignStatus] ([id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

