CREATE TABLE [dbo].[PublisherNotes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_name] [varchar](max) NOT NULL,
	[note] [varchar](max) NOT NULL,
	[added_by_system_user] [varchar](255) NULL,
	[created] [datetime] NOT NULL DEFAULT getdate(),
 CONSTRAINT [PK_PublisherNotes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

