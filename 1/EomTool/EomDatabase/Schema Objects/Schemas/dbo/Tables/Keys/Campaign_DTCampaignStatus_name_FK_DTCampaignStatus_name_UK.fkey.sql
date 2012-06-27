ALTER TABLE [dbo].[Campaign]  WITH CHECK ADD  CONSTRAINT [Campaign_DTCampaignStatus_name_FK_DTCampaignStatus_name_UK] FOREIGN KEY([dt_campaign_status])
REFERENCES [dbo].[DTCampaignStatus] ([name])


GO
ALTER TABLE [dbo].[Campaign] CHECK CONSTRAINT [Campaign_DTCampaignStatus_name_FK_DTCampaignStatus_name_UK]



