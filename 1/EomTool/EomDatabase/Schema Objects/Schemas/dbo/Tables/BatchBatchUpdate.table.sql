CREATE TABLE [dbo].[BatchBatchUpdate](
	[batch_id] [int] NOT NULL,
	[batch_update_id] [int] NOT NULL,
 CONSTRAINT [PK_BatchBatchUpdate] PRIMARY KEY CLUSTERED 
(
	[batch_id] ASC,
	[batch_update_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


