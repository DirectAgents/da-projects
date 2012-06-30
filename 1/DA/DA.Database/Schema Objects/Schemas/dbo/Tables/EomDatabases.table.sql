CREATE TABLE [dbo].[EomDatabases](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ConnectionString] [nvarchar](500) NOT NULL,
	[FriendlyName] [nvarchar](100) NOT NULL,
	[StartDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


