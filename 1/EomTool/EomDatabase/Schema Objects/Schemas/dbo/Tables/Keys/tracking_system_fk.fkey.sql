ALTER TABLE [dbo].[Campaign]  WITH CHECK ADD  CONSTRAINT [tracking_system_fk] FOREIGN KEY([tracking_system_id])
REFERENCES [dbo].[TrackingSystem] ([id])





GO
ALTER TABLE [dbo].[Campaign] CHECK CONSTRAINT [tracking_system_fk]

