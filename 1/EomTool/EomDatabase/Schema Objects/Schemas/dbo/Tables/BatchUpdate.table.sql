CREATE TABLE [dbo].[BatchUpdate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[note] [varchar](max) NOT NULL,
	[author] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[media_buyer_approval_status_id] [int] NULL,
	[extra] [varchar](255) NULL,
	[from_media_buyer_approval_status_id] [int] NULL,
 CONSTRAINT [PK_BatchUpdate] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


