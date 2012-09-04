ALTER TABLE [dbo].[BatchBatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_BatchBatchUpdate_Batch] FOREIGN KEY([batch_id])
REFERENCES [dbo].[Batch] ([id])


GO
ALTER TABLE [dbo].[BatchBatchUpdate] CHECK CONSTRAINT [FK_BatchBatchUpdate_Batch]

