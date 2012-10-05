ALTER TABLE [dbo].[BatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_BatchUpdate_MediaBuyerApprovalStatusFrom] FOREIGN KEY([from_media_buyer_approval_status_id])
REFERENCES [dbo].[MediaBuyerApprovalStatus] ([id])


GO
ALTER TABLE [dbo].[BatchUpdate] CHECK CONSTRAINT [FK_BatchUpdate_MediaBuyerApprovalStatusFrom]

