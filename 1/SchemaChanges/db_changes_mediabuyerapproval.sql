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
	[id] [int] NOT NULL,
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
CREATE TABLE [dbo].[BatchNote](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[batch_id] [int] NOT NULL,
	[note] [varchar](max) NOT NULL,
	[author] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
 CONSTRAINT [PK_BatchNote] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BatchNote]  WITH CHECK ADD  CONSTRAINT [FK_BatchNote_Batch] FOREIGN KEY([batch_id])
REFERENCES [dbo].[Batch] ([id])
GO
ALTER TABLE [dbo].[BatchNote] CHECK CONSTRAINT [FK_BatchNote_Batch]
GO
ALTER TABLE [dbo].[BatchNote] ADD  CONSTRAINT [DF_BatchNote_created]  DEFAULT (getdate()) FOR [date_created]
GO