ALTER TABLE [dbo].[Campaign]  WITH CHECK ADD  CONSTRAINT [Campaign_AccountManager_id_FK_AccountManager_id_PK] FOREIGN KEY([account_manager_id])
REFERENCES [dbo].[AccountManager] ([id])


GO
ALTER TABLE [dbo].[Campaign] CHECK CONSTRAINT [Campaign_AccountManager_id_FK_AccountManager_id_PK]



