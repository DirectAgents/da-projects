/****** Object:  Table [dbo].[Employee]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[Account]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[Account](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlatformId] [int] NOT NULL,
	[ExternalId] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[CampaignId] [int] NULL,
	[CreativeURLFormat] [nvarchar](max) NULL,
	[NetworkId] [int] NULL,
	[Disabled] [bit] NOT NULL DEFAULT ((0)),
	[Filter] [nvarchar](max) NULL,
 CONSTRAINT [PK_td.Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[ActionType]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[ActionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NULL,
	[DisplayName] [nvarchar](max) NULL,
 CONSTRAINT [PK_td.ActionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[Ad]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[Ad](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[ExternalId] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[Headline] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[DestinationUrl] [nvarchar](max) NULL,
 CONSTRAINT [PK_td.Ad] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[AdSet]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[AdSet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[StrategyId] [int] NULL,
	[ExternalId] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_td.AdSet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[AdSetAction]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[AdSetAction](
	[Date] [datetime] NOT NULL,
	[AdSetId] [int] NOT NULL,
	[ActionTypeId] [int] NOT NULL,
	[PostClick] [int] NOT NULL,
	[PostView] [int] NOT NULL,
	[PostClickVal] [decimal](18, 4) NOT NULL DEFAULT ((0)),
	[PostViewVal] [decimal](18, 4) NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_td.AdSetAction] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[AdSetId] ASC,
	[ActionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [td].[AdSetSummary]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[AdSetSummary](
	[Date] [datetime] NOT NULL,
	[AdSetId] [int] NOT NULL,
	[Impressions] [int] NOT NULL,
	[Clicks] [int] NOT NULL,
	[PostClickConv] [int] NOT NULL,
	[PostViewConv] [int] NOT NULL,
	[Cost] [decimal](18, 6) NOT NULL,
	[PostClickRev] [decimal](18, 4) NOT NULL,
	[PostViewRev] [decimal](18, 4) NOT NULL,
	[AllClicks] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_td.AdSetSummary] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[AdSetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [td].[AdSummary]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[AdSummary](
	[Date] [datetime] NOT NULL,
	[TDadId] [int] NOT NULL,
	[Impressions] [int] NOT NULL,
	[Clicks] [int] NOT NULL,
	[PostClickConv] [int] NOT NULL,
	[PostViewConv] [int] NOT NULL,
	[Cost] [decimal](18, 6) NOT NULL,
	[AllClicks] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_td.AdSummary] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[TDadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [td].[Advertiser]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [td].[Advertiser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[SalesRepId] [int] NULL,
	[AMId] [int] NULL,
	[Logo] [varbinary](max) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_td.Advertiser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [td].[BudgetInfo]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[BudgetInfo](
	[CampaignId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[MediaSpend] [decimal](14, 2) NOT NULL,
	[MgmtFeePct] [decimal](10, 5) NOT NULL,
	[MarginPct] [decimal](10, 5) NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_td.BudgetInfo] PRIMARY KEY CLUSTERED 
(
	[CampaignId] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [td].[Campaign]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[Campaign](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AdvertiserId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[MediaSpend] [decimal](14, 2) NOT NULL,
	[MgmtFeePct] [decimal](10, 5) NOT NULL,
	[MarginPct] [decimal](10, 5) NOT NULL,
	[BaseFee] [decimal](14, 2) NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_td.Campaign] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[DailySummary]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[DailySummary](
	[Date] [datetime] NOT NULL,
	[AccountId] [int] NOT NULL,
	[Impressions] [int] NOT NULL,
	[Clicks] [int] NOT NULL,
	[Cost] [decimal](18, 6) NOT NULL,
	[PostClickConv] [int] NOT NULL DEFAULT ((0)),
	[PostViewConv] [int] NOT NULL DEFAULT ((0)),
	[PostClickRev] [decimal](18, 4) NOT NULL DEFAULT ((0)),
	[PostViewRev] [decimal](18, 4) NOT NULL DEFAULT ((0)),
	[AllClicks] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_td.DailySummary] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [td].[ExtraItem]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[ExtraItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[CampaignId] [int] NOT NULL,
	[PlatformId] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Cost] [decimal](14, 2) NOT NULL,
	[Revenue] [decimal](14, 2) NOT NULL,
 CONSTRAINT [PK_td.ExtraItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[Network]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[Network](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_td.Network] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[PlatColMapping]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[PlatColMapping](
	[Id] [int] NOT NULL,
	[Date] [nvarchar](max) NOT NULL,
	[Cost] [nvarchar](max) NULL,
	[Impressions] [nvarchar](max) NULL,
	[Clicks] [nvarchar](max) NULL,
	[PostClickConv] [nvarchar](max) NULL,
	[PostViewConv] [nvarchar](max) NULL,
	[StrategyName] [nvarchar](max) NULL,
	[StrategyEid] [nvarchar](max) NULL,
	[TDadName] [nvarchar](max) NULL,
	[TDadEid] [nvarchar](max) NULL,
	[SiteName] [nvarchar](max) NULL,
	[Month] [nvarchar](max) NULL,
	[PostClickRev] [nvarchar](max) NULL,
	[PostViewRev] [nvarchar](max) NULL,
 CONSTRAINT [PK_td.PlatColMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[Platform]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[Platform](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](max) NULL,
	[Tokens] [nvarchar](max) NULL,
 CONSTRAINT [PK_td.Platform] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[PlatformBudgetInfo]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[PlatformBudgetInfo](
	[CampaignId] [int] NOT NULL,
	[PlatformId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[MediaSpend] [decimal](14, 2) NOT NULL,
	[MgmtFeePct] [decimal](10, 5) NOT NULL,
	[MarginPct] [decimal](10, 5) NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_td.PlatformBudgetInfo] PRIMARY KEY CLUSTERED 
(
	[CampaignId] ASC,
	[PlatformId] ASC,
	[Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [td].[Strategy]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[Strategy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[ExternalId] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_td.Strategy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [td].[StrategyAction]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[StrategyAction](
	[Date] [datetime] NOT NULL,
	[StrategyId] [int] NOT NULL,
	[ActionTypeId] [int] NOT NULL,
	[PostClick] [int] NOT NULL,
	[PostView] [int] NOT NULL,
 CONSTRAINT [PK_td.StrategyAction] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[StrategyId] ASC,
	[ActionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [td].[StrategySummary]    Script Date: 11/16/2017 11:19:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [td].[StrategySummary](
	[Date] [datetime] NOT NULL,
	[StrategyId] [int] NOT NULL,
	[Impressions] [int] NOT NULL,
	[Clicks] [int] NOT NULL,
	[PostClickConv] [int] NOT NULL,
	[PostViewConv] [int] NOT NULL,
	[Cost] [decimal](18, 6) NOT NULL,
	[PostClickRev] [decimal](18, 4) NOT NULL DEFAULT ((0)),
	[PostViewRev] [decimal](18, 4) NOT NULL DEFAULT ((0)),
	[AllClicks] [int] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_td.StrategySummary] PRIMARY KEY CLUSTERED 
(
	[Date] ASC,
	[StrategyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [td].[Account]  WITH CHECK ADD  CONSTRAINT [FK_td.Account_td.Campaign_CampaignId] FOREIGN KEY([CampaignId])
REFERENCES [td].[Campaign] ([Id])
GO
ALTER TABLE [td].[Account] CHECK CONSTRAINT [FK_td.Account_td.Campaign_CampaignId]
GO
ALTER TABLE [td].[Account]  WITH CHECK ADD  CONSTRAINT [FK_td.Account_td.Network_NetworkId] FOREIGN KEY([NetworkId])
REFERENCES [td].[Network] ([Id])
GO
ALTER TABLE [td].[Account] CHECK CONSTRAINT [FK_td.Account_td.Network_NetworkId]
GO
ALTER TABLE [td].[Account]  WITH CHECK ADD  CONSTRAINT [FK_td.Account_td.Platform_PlatformId] FOREIGN KEY([PlatformId])
REFERENCES [td].[Platform] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[Account] CHECK CONSTRAINT [FK_td.Account_td.Platform_PlatformId]
GO
ALTER TABLE [td].[Ad]  WITH CHECK ADD  CONSTRAINT [FK_td.Ad_td.Account_AccountId] FOREIGN KEY([AccountId])
REFERENCES [td].[Account] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[Ad] CHECK CONSTRAINT [FK_td.Ad_td.Account_AccountId]
GO
ALTER TABLE [td].[AdSet]  WITH CHECK ADD  CONSTRAINT [FK_td.AdSet_td.Account_AccountId] FOREIGN KEY([AccountId])
REFERENCES [td].[Account] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[AdSet] CHECK CONSTRAINT [FK_td.AdSet_td.Account_AccountId]
GO
ALTER TABLE [td].[AdSet]  WITH CHECK ADD  CONSTRAINT [FK_td.AdSet_td.Strategy_StrategyId] FOREIGN KEY([StrategyId])
REFERENCES [td].[Strategy] ([Id])
GO
ALTER TABLE [td].[AdSet] CHECK CONSTRAINT [FK_td.AdSet_td.Strategy_StrategyId]
GO
ALTER TABLE [td].[AdSetAction]  WITH CHECK ADD  CONSTRAINT [FK_td.AdSetAction_td.ActionType_ActionTypeId] FOREIGN KEY([ActionTypeId])
REFERENCES [td].[ActionType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[AdSetAction] CHECK CONSTRAINT [FK_td.AdSetAction_td.ActionType_ActionTypeId]
GO
ALTER TABLE [td].[AdSetAction]  WITH CHECK ADD  CONSTRAINT [FK_td.AdSetAction_td.AdSet_AdSetId] FOREIGN KEY([AdSetId])
REFERENCES [td].[AdSet] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[AdSetAction] CHECK CONSTRAINT [FK_td.AdSetAction_td.AdSet_AdSetId]
GO
ALTER TABLE [td].[AdSetSummary]  WITH CHECK ADD  CONSTRAINT [FK_td.AdSetSummary_td.AdSet_AdSetId] FOREIGN KEY([AdSetId])
REFERENCES [td].[AdSet] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[AdSetSummary] CHECK CONSTRAINT [FK_td.AdSetSummary_td.AdSet_AdSetId]
GO
ALTER TABLE [td].[AdSummary]  WITH CHECK ADD  CONSTRAINT [FK_td.AdSummary_td.Ad_TDadId] FOREIGN KEY([TDadId])
REFERENCES [td].[Ad] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[AdSummary] CHECK CONSTRAINT [FK_td.AdSummary_td.Ad_TDadId]
GO
ALTER TABLE [td].[Advertiser]  WITH CHECK ADD  CONSTRAINT [FK_td.Advertiser_dbo.Employee_AMId] FOREIGN KEY([AMId])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [td].[Advertiser] CHECK CONSTRAINT [FK_td.Advertiser_dbo.Employee_AMId]
GO
ALTER TABLE [td].[Advertiser]  WITH CHECK ADD  CONSTRAINT [FK_td.Advertiser_dbo.Employee_SalesRepId] FOREIGN KEY([SalesRepId])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [td].[Advertiser] CHECK CONSTRAINT [FK_td.Advertiser_dbo.Employee_SalesRepId]
GO
ALTER TABLE [td].[BudgetInfo]  WITH CHECK ADD  CONSTRAINT [FK_td.BudgetInfo_td.Campaign_CampaignId] FOREIGN KEY([CampaignId])
REFERENCES [td].[Campaign] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[BudgetInfo] CHECK CONSTRAINT [FK_td.BudgetInfo_td.Campaign_CampaignId]
GO
ALTER TABLE [td].[Campaign]  WITH CHECK ADD  CONSTRAINT [FK_td.Campaign_td.Advertiser_AdvertiserId] FOREIGN KEY([AdvertiserId])
REFERENCES [td].[Advertiser] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[Campaign] CHECK CONSTRAINT [FK_td.Campaign_td.Advertiser_AdvertiserId]
GO
ALTER TABLE [td].[DailySummary]  WITH CHECK ADD  CONSTRAINT [FK_td.DailySummary_td.Account_AccountId] FOREIGN KEY([AccountId])
REFERENCES [td].[Account] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[DailySummary] CHECK CONSTRAINT [FK_td.DailySummary_td.Account_AccountId]
GO
ALTER TABLE [td].[ExtraItem]  WITH CHECK ADD  CONSTRAINT [FK_td.ExtraItem_td.Campaign_CampaignId] FOREIGN KEY([CampaignId])
REFERENCES [td].[Campaign] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[ExtraItem] CHECK CONSTRAINT [FK_td.ExtraItem_td.Campaign_CampaignId]
GO
ALTER TABLE [td].[ExtraItem]  WITH CHECK ADD  CONSTRAINT [FK_td.ExtraItem_td.Platform_PlatformId] FOREIGN KEY([PlatformId])
REFERENCES [td].[Platform] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[ExtraItem] CHECK CONSTRAINT [FK_td.ExtraItem_td.Platform_PlatformId]
GO
ALTER TABLE [td].[PlatColMapping]  WITH CHECK ADD  CONSTRAINT [FK_td.PlatColMapping_td.Platform_Id] FOREIGN KEY([Id])
REFERENCES [td].[Platform] ([Id])
GO
ALTER TABLE [td].[PlatColMapping] CHECK CONSTRAINT [FK_td.PlatColMapping_td.Platform_Id]
GO
ALTER TABLE [td].[PlatformBudgetInfo]  WITH CHECK ADD  CONSTRAINT [FK_td.PlatformBudgetInfo_td.Campaign_CampaignId] FOREIGN KEY([CampaignId])
REFERENCES [td].[Campaign] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[PlatformBudgetInfo] CHECK CONSTRAINT [FK_td.PlatformBudgetInfo_td.Campaign_CampaignId]
GO
ALTER TABLE [td].[PlatformBudgetInfo]  WITH CHECK ADD  CONSTRAINT [FK_td.PlatformBudgetInfo_td.Platform_PlatformId] FOREIGN KEY([PlatformId])
REFERENCES [td].[Platform] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[PlatformBudgetInfo] CHECK CONSTRAINT [FK_td.PlatformBudgetInfo_td.Platform_PlatformId]
GO
ALTER TABLE [td].[Strategy]  WITH CHECK ADD  CONSTRAINT [FK_td.Strategy_td.Account_AccountId] FOREIGN KEY([AccountId])
REFERENCES [td].[Account] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[Strategy] CHECK CONSTRAINT [FK_td.Strategy_td.Account_AccountId]
GO
ALTER TABLE [td].[StrategyAction]  WITH CHECK ADD  CONSTRAINT [FK_td.StrategyAction_td.ActionType_ActionTypeId] FOREIGN KEY([ActionTypeId])
REFERENCES [td].[ActionType] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[StrategyAction] CHECK CONSTRAINT [FK_td.StrategyAction_td.ActionType_ActionTypeId]
GO
ALTER TABLE [td].[StrategyAction]  WITH CHECK ADD  CONSTRAINT [FK_td.StrategyAction_td.Strategy_StrategyId] FOREIGN KEY([StrategyId])
REFERENCES [td].[Strategy] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[StrategyAction] CHECK CONSTRAINT [FK_td.StrategyAction_td.Strategy_StrategyId]
GO
ALTER TABLE [td].[StrategySummary]  WITH CHECK ADD  CONSTRAINT [FK_td.StrategySummary_td.Strategy_StrategyId] FOREIGN KEY([StrategyId])
REFERENCES [td].[Strategy] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [td].[StrategySummary] CHECK CONSTRAINT [FK_td.StrategySummary_td.Strategy_StrategyId]
GO


INSERT [td].[Platform] ([Code], [Name], [Tokens]) VALUES (N'adf', N'Adform', NULL)
INSERT [td].[Platform] ([Code], [Name], [Tokens]) VALUES (N'adr', N'AdRoll', NULL)
INSERT [td].[Platform] ([Code], [Name], [Tokens]) VALUES (N'dbm', N'DBM', NULL)
INSERT [td].[Platform] ([Code], [Name], [Tokens]) VALUES (N'fb', N'Facebook', NULL)
INSERT [td].[Platform] ([Code], [Name], [Tokens]) VALUES (N'yam', N'YAM', NULL)
INSERT [td].[Platform] ([Code], [Name], [Tokens]) VALUES (N'amzn', N'Amazon', NULL)
INSERT [td].[Platform] ([Code], [Name], [Tokens]) VALUES (N'datd', N'DA Trading Desk', NULL)
INSERT [td].[Platform] ([Code], [Name], [Tokens]) VALUES (N'mf', N'Management Fee', NULL)
