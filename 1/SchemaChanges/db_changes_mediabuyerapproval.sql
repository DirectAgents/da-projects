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
