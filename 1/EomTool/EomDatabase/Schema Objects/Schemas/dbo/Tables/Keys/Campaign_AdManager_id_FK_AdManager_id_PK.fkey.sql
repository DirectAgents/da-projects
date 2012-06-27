ALTER TABLE [dbo].[Campaign]  WITH CHECK ADD  CONSTRAINT [Campaign_AdManager_id_FK_AdManager_id_PK] FOREIGN KEY([ad_manager_id])
REFERENCES [dbo].[AdManager] ([id])


GO
ALTER TABLE [dbo].[Campaign] CHECK CONSTRAINT [Campaign_AdManager_id_FK_AdManager_id_PK]



