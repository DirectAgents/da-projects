
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/29/2013 21:30:40
-- Generated from EDMX file: C:\GitHub\da-projects\3\ClientPortal.Data\Contexts\ClientPortal.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ClientPortal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_dbo_AdvertiserContact_dbo_Advertiser_AdvertiserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdvertiserContact] DROP CONSTRAINT [FK_dbo_AdvertiserContact_dbo_Advertiser_AdvertiserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AdvertiserContact_dbo_Contact_ContactId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdvertiserContact] DROP CONSTRAINT [FK_dbo_AdvertiserContact_dbo_Contact_ContactId];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Advertiser]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advertiser];
GO
IF OBJECT_ID(N'[dbo].[AdvertiserContact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdvertiserContact];
GO
IF OBJECT_ID(N'[dbo].[Click]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Click];
GO
IF OBJECT_ID(N'[dbo].[Contact]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contact];
GO
IF OBJECT_ID(N'[dbo].[Conversion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Conversion];
GO
IF OBJECT_ID(N'[dbo].[ConversionData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConversionData];
GO
IF OBJECT_ID(N'[dbo].[DailySummary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DailySummary];
GO
IF OBJECT_ID(N'[dbo].[Goal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Goal];
GO
IF OBJECT_ID(N'[dbo].[Offer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Offer];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Clicks'
CREATE TABLE [dbo].[Clicks] (
    [click_id] int  NOT NULL,
    [visitor_id] int  NOT NULL,
    [request_session_id] int  NOT NULL,
    [click_date] datetime  NOT NULL,
    [affiliate_id] int  NOT NULL,
    [affiliate_name] nvarchar(max)  NULL,
    [advertiser_id] int  NOT NULL,
    [advertiser_name] nvarchar(max)  NULL,
    [offer_id] int  NOT NULL,
    [offer_name] nvarchar(max)  NULL,
    [campaign_id] int  NOT NULL,
    [creative_id] int  NOT NULL,
    [creative_name] nvarchar(max)  NULL,
    [sub_id_1] nvarchar(max)  NULL,
    [sub_id_2] nvarchar(max)  NULL,
    [sub_id_3] nvarchar(max)  NULL,
    [sub_id_4] nvarchar(max)  NULL,
    [sub_id_5] nvarchar(max)  NULL,
    [ip_address] nvarchar(max)  NULL,
    [user_agent] nvarchar(max)  NULL,
    [referrer_url] nvarchar(max)  NULL,
    [request_url] nvarchar(max)  NULL,
    [redirect_url] nvarchar(max)  NULL,
    [country_code] nvarchar(max)  NULL,
    [country_name] nvarchar(max)  NULL,
    [region_code] nvarchar(max)  NULL,
    [region_name] nvarchar(max)  NULL,
    [language_id] tinyint  NOT NULL,
    [language_name] nvarchar(max)  NULL,
    [language_abbr] nvarchar(max)  NULL,
    [isp_id] int  NOT NULL,
    [isp_name] nvarchar(max)  NULL,
    [device_id] smallint  NOT NULL,
    [device_name] nvarchar(max)  NULL,
    [operating_system_name] nvarchar(max)  NULL,
    [browser_id] int  NOT NULL,
    [browser_name] nvarchar(max)  NULL,
    [disposition] nvarchar(max)  NULL,
    [paid_action] nvarchar(max)  NULL,
    [total_clicks] int  NOT NULL
);
GO

-- Creating table 'Conversions'
CREATE TABLE [dbo].[Conversions] (
    [conversion_id] nvarchar(128)  NOT NULL,
    [visitor_id] int  NOT NULL,
    [request_session_id] int  NOT NULL,
    [click_id] int  NULL,
    [conversion_date] datetime  NOT NULL,
    [last_updated] datetime  NULL,
    [click_date] datetime  NULL,
    [affiliate_id] int  NOT NULL,
    [affiliate_name] nvarchar(max)  NULL,
    [advertiser_id] int  NOT NULL,
    [advertiser_name] nvarchar(max)  NULL,
    [offer_id] int  NOT NULL,
    [offer_name] nvarchar(max)  NULL,
    [campaign_id] int  NOT NULL,
    [creative_id] int  NOT NULL,
    [creative_name] nvarchar(max)  NULL,
    [sub_id_1] nvarchar(max)  NULL,
    [sub_id_2] nvarchar(max)  NULL,
    [sub_id_3] nvarchar(max)  NULL,
    [sub_id_4] nvarchar(max)  NULL,
    [sub_id_5] nvarchar(max)  NULL,
    [conversion_type] nvarchar(max)  NULL,
    [paid_currency_id] tinyint  NOT NULL,
    [paid_amount] decimal(18,2)  NOT NULL,
    [paid_formatted_amount] nvarchar(max)  NULL,
    [received_currency_id] tinyint  NOT NULL,
    [received_amount] decimal(18,2)  NOT NULL,
    [received_formatted_amount] nvarchar(max)  NULL,
    [step_reached] tinyint  NOT NULL,
    [pixel_dropped] bit  NOT NULL,
    [suppressed] bit  NOT NULL,
    [returned] bit  NOT NULL,
    [test] bit  NOT NULL,
    [transaction_id] nvarchar(max)  NULL,
    [conversion_ip_address] nvarchar(max)  NULL,
    [click_ip_address] nvarchar(max)  NULL,
    [country] nvarchar(max)  NULL,
    [conversion_referrer_url] nvarchar(max)  NULL,
    [click_referrer_url] nvarchar(max)  NULL,
    [conversion_user_agent] nvarchar(max)  NULL,
    [click_user_agent] nvarchar(max)  NULL,
    [disposition_approved] bit  NOT NULL,
    [disposition_name] nvarchar(max)  NULL,
    [disposition_contact] nvarchar(max)  NULL,
    [disposition_date] datetime  NULL,
    [note] nvarchar(max)  NULL
);
GO

-- Creating table 'ConversionDatas'
CREATE TABLE [dbo].[ConversionDatas] (
    [conversion_id] nvarchar(128)  NOT NULL,
    [value0] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'Goals'
CREATE TABLE [dbo].[Goals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdvertiserId] int  NOT NULL,
    [OfferId] int  NULL,
    [Name] nvarchar(max)  NULL,
    [TypeId] int  NOT NULL,
    [MetricId] int  NOT NULL,
    [Target] decimal(18,2)  NOT NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL
);
GO

-- Creating table 'Advertisers'
CREATE TABLE [dbo].[Advertisers] (
    [AdvertiserId] int  NOT NULL,
    [AdvertiserName] nvarchar(max)  NULL,
    [Logo] varbinary(max)  NULL,
    [Culture] nvarchar(max)  NULL,
    [ShowCPMRep] bit  NOT NULL,
    [ShowConversionData] bit  NOT NULL,
    [ConversionValueName] nvarchar(max)  NULL,
    [ConversionValueIsNumber] bit  NOT NULL
);
GO

-- Creating table 'AdvertiserContacts'
CREATE TABLE [dbo].[AdvertiserContacts] (
    [AdvertiserId] int  NOT NULL,
    [ContactId] int  NOT NULL,
    [Order] int  NULL
);
GO

-- Creating table 'Contacts'
CREATE TABLE [dbo].[Contacts] (
    [ContactId] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [Title] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL
);
GO

-- Creating table 'DailySummaries'
CREATE TABLE [dbo].[DailySummaries] (
    [offer_id] int  NOT NULL,
    [date] datetime  NOT NULL,
    [views] int  NOT NULL,
    [clicks] int  NOT NULL,
    [click_thru] decimal(18,2)  NOT NULL,
    [conversions] int  NOT NULL,
    [paid] int  NOT NULL,
    [sellable] int  NOT NULL,
    [conversion_rate] decimal(18,2)  NOT NULL,
    [cpl] decimal(18,2)  NOT NULL,
    [cost] decimal(18,2)  NOT NULL,
    [rpt] decimal(18,2)  NOT NULL,
    [revenue] decimal(18,2)  NOT NULL,
    [margin] decimal(18,2)  NOT NULL,
    [profit] decimal(18,2)  NOT NULL,
    [epc] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'Offers'
CREATE TABLE [dbo].[Offers] (
    [Offer_Id] int  NOT NULL,
    [OfferName] nvarchar(255)  NULL,
    [Advertiser_Id] int  NULL,
    [VerticalName] nvarchar(255)  NULL,
    [OfferType] nvarchar(255)  NULL,
    [StatusName] nvarchar(255)  NULL,
    [DefaultPriceFormat] nvarchar(255)  NULL,
    [DefaultPayout] nvarchar(255)  NULL,
    [PriceReceived] nvarchar(255)  NULL,
    [Secure] nvarchar(255)  NULL,
    [OfferLink] nvarchar(255)  NULL,
    [ThumbnailImageUrl] nvarchar(255)  NULL,
    [ExpirationDate] nvarchar(255)  NULL,
    [CookieDays] nvarchar(255)  NULL,
    [CookieDaysImpressions] nvarchar(255)  NULL,
    [DateCreated] nvarchar(255)  NULL,
    [Currency] nchar(3)  NULL,
    [AllowedCountries] nvarchar(max)  NULL,
    [Xml] nvarchar(max)  NULL,
    [AllowedMediaTypeNames] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ScheduledReports'
CREATE TABLE [dbo].[ScheduledReports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdvertiserId] int  NOT NULL,
    [Months] int  NOT NULL,
    [Days] int  NOT NULL,
    [IsCumulative] bit  NOT NULL,
    [LastSent] datetime  NULL,
    [NextSend] datetime  NULL
);
GO

-- Creating table 'GeneratedReports'
CREATE TABLE [dbo].[GeneratedReports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ReportType] int  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [ReportStatus] int  NOT NULL,
    [ReportStatusUpdated] datetime  NULL,
    [Content] nvarchar(max)  NOT NULL,
    [ScheduledReportId] int  NOT NULL
);
GO

-- Creating table 'ScheduledReportRecipients'
CREATE TABLE [dbo].[ScheduledReportRecipients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmailAddress] nvarchar(max)  NOT NULL,
    [ScheduledReportId] int  NOT NULL
);
GO

-- Creating table 'EmailedReports'
CREATE TABLE [dbo].[EmailedReports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmailTo] nvarchar(max)  NOT NULL,
    [EmailStatus] int  NOT NULL,
    [EmailStatusUpdated] datetime  NULL,
    [GeneratedReportId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [click_id] in table 'Clicks'
ALTER TABLE [dbo].[Clicks]
ADD CONSTRAINT [PK_Clicks]
    PRIMARY KEY CLUSTERED ([click_id] ASC);
GO

-- Creating primary key on [conversion_id] in table 'Conversions'
ALTER TABLE [dbo].[Conversions]
ADD CONSTRAINT [PK_Conversions]
    PRIMARY KEY CLUSTERED ([conversion_id] ASC);
GO

-- Creating primary key on [conversion_id] in table 'ConversionDatas'
ALTER TABLE [dbo].[ConversionDatas]
ADD CONSTRAINT [PK_ConversionDatas]
    PRIMARY KEY CLUSTERED ([conversion_id] ASC);
GO

-- Creating primary key on [Id] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [PK_Goals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AdvertiserId] in table 'Advertisers'
ALTER TABLE [dbo].[Advertisers]
ADD CONSTRAINT [PK_Advertisers]
    PRIMARY KEY CLUSTERED ([AdvertiserId] ASC);
GO

-- Creating primary key on [AdvertiserId], [ContactId] in table 'AdvertiserContacts'
ALTER TABLE [dbo].[AdvertiserContacts]
ADD CONSTRAINT [PK_AdvertiserContacts]
    PRIMARY KEY CLUSTERED ([AdvertiserId], [ContactId] ASC);
GO

-- Creating primary key on [ContactId] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [PK_Contacts]
    PRIMARY KEY CLUSTERED ([ContactId] ASC);
GO

-- Creating primary key on [offer_id], [date] in table 'DailySummaries'
ALTER TABLE [dbo].[DailySummaries]
ADD CONSTRAINT [PK_DailySummaries]
    PRIMARY KEY CLUSTERED ([offer_id], [date] ASC);
GO

-- Creating primary key on [Offer_Id] in table 'Offers'
ALTER TABLE [dbo].[Offers]
ADD CONSTRAINT [PK_Offers]
    PRIMARY KEY CLUSTERED ([Offer_Id] ASC);
GO

-- Creating primary key on [Id] in table 'ScheduledReports'
ALTER TABLE [dbo].[ScheduledReports]
ADD CONSTRAINT [PK_ScheduledReports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GeneratedReports'
ALTER TABLE [dbo].[GeneratedReports]
ADD CONSTRAINT [PK_GeneratedReports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ScheduledReportRecipients'
ALTER TABLE [dbo].[ScheduledReportRecipients]
ADD CONSTRAINT [PK_ScheduledReportRecipients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmailedReports'
ALTER TABLE [dbo].[EmailedReports]
ADD CONSTRAINT [PK_EmailedReports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AdvertiserId] in table 'AdvertiserContacts'
ALTER TABLE [dbo].[AdvertiserContacts]
ADD CONSTRAINT [FK_dbo_AdvertiserContact_dbo_Advertiser_AdvertiserId]
    FOREIGN KEY ([AdvertiserId])
    REFERENCES [dbo].[Advertisers]
        ([AdvertiserId])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ContactId] in table 'AdvertiserContacts'
ALTER TABLE [dbo].[AdvertiserContacts]
ADD CONSTRAINT [FK_dbo_AdvertiserContact_dbo_Contact_ContactId]
    FOREIGN KEY ([ContactId])
    REFERENCES [dbo].[Contacts]
        ([ContactId])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AdvertiserContact_dbo_Contact_ContactId'
CREATE INDEX [IX_FK_dbo_AdvertiserContact_dbo_Contact_ContactId]
ON [dbo].[AdvertiserContacts]
    ([ContactId]);
GO

-- Creating foreign key on [AdvertiserId] in table 'ScheduledReports'
ALTER TABLE [dbo].[ScheduledReports]
ADD CONSTRAINT [FK_AdvertiserScheduledReport]
    FOREIGN KEY ([AdvertiserId])
    REFERENCES [dbo].[Advertisers]
        ([AdvertiserId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertiserScheduledReport'
CREATE INDEX [IX_FK_AdvertiserScheduledReport]
ON [dbo].[ScheduledReports]
    ([AdvertiserId]);
GO

-- Creating foreign key on [ScheduledReportId] in table 'ScheduledReportRecipients'
ALTER TABLE [dbo].[ScheduledReportRecipients]
ADD CONSTRAINT [FK_ScheduledReportScheduledReportRecipient]
    FOREIGN KEY ([ScheduledReportId])
    REFERENCES [dbo].[ScheduledReports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ScheduledReportScheduledReportRecipient'
CREATE INDEX [IX_FK_ScheduledReportScheduledReportRecipient]
ON [dbo].[ScheduledReportRecipients]
    ([ScheduledReportId]);
GO

-- Creating foreign key on [ScheduledReportId] in table 'GeneratedReports'
ALTER TABLE [dbo].[GeneratedReports]
ADD CONSTRAINT [FK_ScheduledReportGeneratedReport]
    FOREIGN KEY ([ScheduledReportId])
    REFERENCES [dbo].[ScheduledReports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ScheduledReportGeneratedReport'
CREATE INDEX [IX_FK_ScheduledReportGeneratedReport]
ON [dbo].[GeneratedReports]
    ([ScheduledReportId]);
GO

-- Creating foreign key on [GeneratedReportId] in table 'EmailedReports'
ALTER TABLE [dbo].[EmailedReports]
ADD CONSTRAINT [FK_GeneratedReportEmailedReport]
    FOREIGN KEY ([GeneratedReportId])
    REFERENCES [dbo].[GeneratedReports]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GeneratedReportEmailedReport'
CREATE INDEX [IX_FK_GeneratedReportEmailedReport]
ON [dbo].[EmailedReports]
    ([GeneratedReportId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------