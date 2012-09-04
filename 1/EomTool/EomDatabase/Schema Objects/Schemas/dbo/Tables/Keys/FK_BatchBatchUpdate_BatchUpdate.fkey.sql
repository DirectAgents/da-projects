ALTER TABLE [dbo].[BatchBatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_BatchBatchUpdate_BatchUpdate] FOREIGN KEY([batch_update_id])
REFERENCES [dbo].[BatchUpdate] ([id])


GO
ALTER TABLE [dbo].[BatchBatchUpdate] CHECK CONSTRAINT [FK_BatchBatchUpdate_BatchUpdate]

