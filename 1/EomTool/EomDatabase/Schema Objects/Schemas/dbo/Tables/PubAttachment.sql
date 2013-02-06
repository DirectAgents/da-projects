CREATE TABLE [dbo].[PubAttachment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_name] [varchar](max) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[description] [nvarchar](255) NULL,
	[binary_content] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_PubAttachment] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
