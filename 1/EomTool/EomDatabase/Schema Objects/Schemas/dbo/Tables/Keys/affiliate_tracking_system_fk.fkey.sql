ALTER TABLE [dbo].[Affiliate]  WITH CHECK ADD  CONSTRAINT [affiliate_tracking_system_fk] FOREIGN KEY([tracking_system_id])
REFERENCES [dbo].[TrackingSystem] ([id])





GO
ALTER TABLE [dbo].[Affiliate] CHECK CONSTRAINT [affiliate_tracking_system_fk]

