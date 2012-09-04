--USE zDADatabaseJuly2012Test2
--GO
CREATE TABLE [dbo].[MediaBuyerApprovalStatus](
	[id] [int] NOT NULL,
	[name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MediaBuyerApprovalStatus] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT INTO [dbo].[MediaBuyerApprovalStatus] VALUES 
	 ('1', 'default')
	,('2', 'Queued')
	,('3', 'Sent')
	,('4', 'Approved')
	,('5', 'Hold')
GO
ALTER TABLE Item 
ADD media_buyer_approval_status_id int NOT NULL
REFERENCES MediaBuyerApprovalStatus(id)
DEFAULT 1
GO
CREATE TABLE [dbo].[Batch](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL,
 CONSTRAINT [PK_Batch] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE dbo.Item ADD
	batch_id int NULL
GO
ALTER TABLE dbo.Item ADD CONSTRAINT
	FK_Item_Batch FOREIGN KEY
	(
	batch_id
	) REFERENCES dbo.Batch
	(
	id
	) ON UPDATE  NO ACTION
	 ON DELETE  NO ACTION
GO
CREATE TABLE [dbo].[BatchUpdate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[note] [varchar](max) NOT NULL,
	[author] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[media_buyer_approval_status_id] [int] NULL,
	[extra] [varchar](255) NULL,
 CONSTRAINT [PK_BatchUpdate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_BatchUpdate_MediaBuyerApprovalStatus] FOREIGN KEY([media_buyer_approval_status_id])
REFERENCES [dbo].[MediaBuyerApprovalStatus] ([id])
GO
ALTER TABLE [dbo].[BatchUpdate] CHECK CONSTRAINT [FK_BatchUpdate_MediaBuyerApprovalStatus]
GO
ALTER TABLE [dbo].[BatchUpdate] ADD  CONSTRAINT [DF_BatchUpdate_date_created]  DEFAULT (getdate()) FOR [date_created]
GO
CREATE TABLE [dbo].[BatchBatchUpdate](
	[batch_id] [int] NOT NULL,
	[batch_update_id] [int] NOT NULL,
 CONSTRAINT [PK_BatchBatchUpdate] PRIMARY KEY CLUSTERED 
(
	[batch_id] ASC,
	[batch_update_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BatchBatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_BatchBatchUpdate_Batch] FOREIGN KEY([batch_id])
REFERENCES [dbo].[Batch] ([id])
GO
ALTER TABLE [dbo].[BatchBatchUpdate] CHECK CONSTRAINT [FK_BatchBatchUpdate_Batch]
GO
ALTER TABLE [dbo].[BatchBatchUpdate]  WITH CHECK ADD  CONSTRAINT [FK_BatchBatchUpdate_BatchUpdate] FOREIGN KEY([batch_update_id])
REFERENCES [dbo].[BatchUpdate] ([id])
GO
ALTER TABLE [dbo].[BatchBatchUpdate] CHECK CONSTRAINT [FK_BatchBatchUpdate_BatchUpdate]
GO
