USE [Cake]
GO

/****** Object:  Table [dbo].[FileUpload]    Script Date: 03/14/2013 14:22:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FileUpload](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UploadDate] [datetime] NOT NULL,
	[Filename] [nvarchar](100) NULL,
	[Text] [nvarchar](max) NULL,
	[CakeAdvertiserId] [int] NULL,
 CONSTRAINT [PK_FileUpload] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
