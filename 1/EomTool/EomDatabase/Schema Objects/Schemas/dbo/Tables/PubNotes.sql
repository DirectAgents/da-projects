CREATE TABLE [dbo].[PubNotes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_name] [varchar](max) NOT NULL,
	[note] [varchar](max) NOT NULL,
	[added_by_system_user] [varchar](255) NULL,
	[created] [datetime] NOT NULL DEFAULT getdate(), 
    CONSTRAINT [PK_PubNotes] PRIMARY KEY ([id])
) ON [PRIMARY]