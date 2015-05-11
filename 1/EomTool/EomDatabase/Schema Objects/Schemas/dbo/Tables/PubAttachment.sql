CREATE TABLE [dbo].[PubAttachment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[publisher_name] [varchar](max) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[description] [nvarchar](255) NULL,
	[binary_content] [varbinary](max) NOT NULL, 
    CONSTRAINT [PK_PubAttachment] PRIMARY KEY ([id])
) ON [PRIMARY]
