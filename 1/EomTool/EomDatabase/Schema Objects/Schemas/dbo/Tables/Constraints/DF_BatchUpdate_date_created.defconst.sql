ALTER TABLE [dbo].[BatchUpdate] ADD  CONSTRAINT [DF_BatchUpdate_date_created]  DEFAULT (getdate()) FOR [date_created]


