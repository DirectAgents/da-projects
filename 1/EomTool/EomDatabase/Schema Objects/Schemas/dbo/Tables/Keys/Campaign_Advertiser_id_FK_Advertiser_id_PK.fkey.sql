ALTER TABLE [dbo].[Campaign]  WITH CHECK ADD  CONSTRAINT [Campaign_Advertiser_id_FK_Advertiser_id_PK] FOREIGN KEY([advertiser_id])
REFERENCES [dbo].[Advertiser] ([id])


GO
ALTER TABLE [dbo].[Campaign] CHECK CONSTRAINT [Campaign_Advertiser_id_FK_Advertiser_id_PK]



