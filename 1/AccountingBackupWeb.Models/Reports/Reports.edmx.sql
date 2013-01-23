
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/10/2012 11:19:11
-- Generated from EDMX file: C:\Code2011\da2\AccountingBackupWeb.Models\Reports\Reports.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Reports];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CampaignItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_CampaignItem];
GO
IF OBJECT_ID(N'[dbo].[FK_PublisherItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_PublisherItem];
GO
IF OBJECT_ID(N'[dbo].[FK_SourceItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_SourceItem];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitTypeItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_UnitTypeItem];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertiserCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_AdvertiserCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_MediaBuyerPublisher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Publishers] DROP CONSTRAINT [FK_MediaBuyerPublisher];
GO
IF OBJECT_ID(N'[dbo].[FK_AdManagerCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_AdManagerCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountManagerCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_AccountManagerCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_PeriodItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_PeriodItem];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Databases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Databases];
GO
IF OBJECT_ID(N'[dbo].[Periods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Periods];
GO
IF OBJECT_ID(N'[dbo].[Publishers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Publishers];
GO
IF OBJECT_ID(N'[dbo].[Advertisers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advertisers];
GO
IF OBJECT_ID(N'[dbo].[Campaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Campaigns];
GO
IF OBJECT_ID(N'[dbo].[AdManagers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdManagers];
GO
IF OBJECT_ID(N'[dbo].[AccountManagers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountManagers];
GO
IF OBJECT_ID(N'[dbo].[MediaBuyers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MediaBuyers];
GO
IF OBJECT_ID(N'[dbo].[Items]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items];
GO
IF OBJECT_ID(N'[dbo].[Sources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sources];
GO
IF OBJECT_ID(N'[dbo].[UnitTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Databases'
CREATE TABLE [dbo].[Databases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ConnectionString] nvarchar(max)  NOT NULL,
    [FriendlyName] nvarchar(max)  NOT NULL,
    [StartDate] datetime  NOT NULL
);
GO

-- Creating table 'Periods'
CREATE TABLE [dbo].[Periods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Publishers'
CREATE TABLE [dbo].[Publishers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [MediaBuyer_Id] int  NOT NULL
);
GO

-- Creating table 'Advertisers'
CREATE TABLE [dbo].[Advertisers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Campaigns'
CREATE TABLE [dbo].[Campaigns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ProgramId] nvarchar(max)  NOT NULL,
    [Advertiser_Id] int  NOT NULL,
    [AdManager_Id] int  NOT NULL,
    [AccountManager_Id] int  NOT NULL
);
GO

-- Creating table 'AdManagers'
CREATE TABLE [dbo].[AdManagers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AccountManagers'
CREATE TABLE [dbo].[AccountManagers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MediaBuyers'
CREATE TABLE [dbo].[MediaBuyers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(300)  NOT NULL,
    [RevenueCurrency] nchar(3)  NOT NULL,
    [CostCurrency] nchar(3)  NOT NULL,
    [RevenuePerUnit] decimal(14,4)  NOT NULL,
    [CostPerUnit] decimal(14,4)  NOT NULL,
    [NumUnits] decimal(14,4)  NOT NULL,
    [TotalRevenue] decimal(14,4)  NOT NULL,
    [TotalCost] nvarchar(max)  NOT NULL,
    [Margin] nvarchar(max)  NOT NULL,
    [Campaign_Id] int  NOT NULL,
    [Publisher_Id] int  NOT NULL,
    [Source_Id] int  NOT NULL,
    [UnitType_Id] int  NOT NULL,
    [Period_Id] int  NOT NULL
);
GO

-- Creating table 'Sources'
CREATE TABLE [dbo].[Sources] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UnitTypes'
CREATE TABLE [dbo].[UnitTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Databases'
ALTER TABLE [dbo].[Databases]
ADD CONSTRAINT [PK_Databases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Periods'
ALTER TABLE [dbo].[Periods]
ADD CONSTRAINT [PK_Periods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Publishers'
ALTER TABLE [dbo].[Publishers]
ADD CONSTRAINT [PK_Publishers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Advertisers'
ALTER TABLE [dbo].[Advertisers]
ADD CONSTRAINT [PK_Advertisers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [PK_Campaigns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AdManagers'
ALTER TABLE [dbo].[AdManagers]
ADD CONSTRAINT [PK_AdManagers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountManagers'
ALTER TABLE [dbo].[AccountManagers]
ADD CONSTRAINT [PK_AccountManagers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MediaBuyers'
ALTER TABLE [dbo].[MediaBuyers]
ADD CONSTRAINT [PK_MediaBuyers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sources'
ALTER TABLE [dbo].[Sources]
ADD CONSTRAINT [PK_Sources]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UnitTypes'
ALTER TABLE [dbo].[UnitTypes]
ADD CONSTRAINT [PK_UnitTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Campaign_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_CampaignItem]
    FOREIGN KEY ([Campaign_Id])
    REFERENCES [dbo].[Campaigns]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CampaignItem'
CREATE INDEX [IX_FK_CampaignItem]
ON [dbo].[Items]
    ([Campaign_Id]);
GO

-- Creating foreign key on [Publisher_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_PublisherItem]
    FOREIGN KEY ([Publisher_Id])
    REFERENCES [dbo].[Publishers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PublisherItem'
CREATE INDEX [IX_FK_PublisherItem]
ON [dbo].[Items]
    ([Publisher_Id]);
GO

-- Creating foreign key on [Source_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_SourceItem]
    FOREIGN KEY ([Source_Id])
    REFERENCES [dbo].[Sources]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SourceItem'
CREATE INDEX [IX_FK_SourceItem]
ON [dbo].[Items]
    ([Source_Id]);
GO

-- Creating foreign key on [UnitType_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_UnitTypeItem]
    FOREIGN KEY ([UnitType_Id])
    REFERENCES [dbo].[UnitTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitTypeItem'
CREATE INDEX [IX_FK_UnitTypeItem]
ON [dbo].[Items]
    ([UnitType_Id]);
GO

-- Creating foreign key on [Advertiser_Id] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [FK_AdvertiserCampaign]
    FOREIGN KEY ([Advertiser_Id])
    REFERENCES [dbo].[Advertisers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertiserCampaign'
CREATE INDEX [IX_FK_AdvertiserCampaign]
ON [dbo].[Campaigns]
    ([Advertiser_Id]);
GO

-- Creating foreign key on [MediaBuyer_Id] in table 'Publishers'
ALTER TABLE [dbo].[Publishers]
ADD CONSTRAINT [FK_MediaBuyerPublisher]
    FOREIGN KEY ([MediaBuyer_Id])
    REFERENCES [dbo].[MediaBuyers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MediaBuyerPublisher'
CREATE INDEX [IX_FK_MediaBuyerPublisher]
ON [dbo].[Publishers]
    ([MediaBuyer_Id]);
GO

-- Creating foreign key on [AdManager_Id] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [FK_AdManagerCampaign]
    FOREIGN KEY ([AdManager_Id])
    REFERENCES [dbo].[AdManagers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdManagerCampaign'
CREATE INDEX [IX_FK_AdManagerCampaign]
ON [dbo].[Campaigns]
    ([AdManager_Id]);
GO

-- Creating foreign key on [AccountManager_Id] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [FK_AccountManagerCampaign]
    FOREIGN KEY ([AccountManager_Id])
    REFERENCES [dbo].[AccountManagers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountManagerCampaign'
CREATE INDEX [IX_FK_AccountManagerCampaign]
ON [dbo].[Campaigns]
    ([AccountManager_Id]);
GO

-- Creating foreign key on [Period_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_PeriodItem]
    FOREIGN KEY ([Period_Id])
    REFERENCES [dbo].[Periods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PeriodItem'
CREATE INDEX [IX_FK_PeriodItem]
ON [dbo].[Items]
    ([Period_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------