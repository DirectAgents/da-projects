CREATE TABLE [dbo].[BatchUpdate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[note] [varchar](max) NOT NULL,
	[author] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[media_buyer_approval_status_id] [int] NULL,
	[extra] [varchar](255) NULL,
	[from_media_buyer_approval_status_id] [int] NULL, 
    CONSTRAINT [PK_BatchUpdate] PRIMARY KEY ([id])
) ON [PRIMARY]


