ALTER TABLE [dbo].[Campaign]  WITH CHECK ADD  CONSTRAINT [Campaign_CampaignStatus_id_FK_CampaignStatus_id_PK] FOREIGN KEY([campaign_status_id])
REFERENCES [dbo].[CampaignStatus] ([id])


GO
ALTER TABLE [dbo].[Campaign] CHECK CONSTRAINT [Campaign_CampaignStatus_id_FK_CampaignStatus_id_PK]



