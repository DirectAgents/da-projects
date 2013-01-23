
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/19/2012 13:23:26
-- Generated from EDMX file: C:\Code2011\da2\AccountingBackupWeb.Models\Staging\Staging.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ABWeb_staging];
GO


-- Creating table 'EomAccountManagerViewRows'
CREATE TABLE [dbo].[EomAccountManagerViewRows] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Period] nvarchar(max)  NOT NULL,
    [Publisher] nvarchar(max)  NOT NULL,
    [Advertiser] nvarchar(max)  NOT NULL,
    [CampaignNumber] nvarchar(max)  NOT NULL,
    [CampaignName] nvarchar(max)  NOT NULL,
    [RevCurrency] nvarchar(max)  NOT NULL,
    [CostCurrency] nvarchar(max)  NOT NULL,
    [CostPerUnit] nvarchar(max)  NOT NULL,
    [RevPerUnit] nvarchar(max)  NOT NULL,
    [Units] nvarchar(max)  NOT NULL,
    [Revenue] nvarchar(max)  NOT NULL,
    [Cost] nvarchar(max)  NOT NULL,
    [AccountManager] nvarchar(max)  NOT NULL,
    [AdManager] nvarchar(max)  NOT NULL
);
GO

-- Creating primary key on [Id] in table 'EomAccountManagerViewRows'
ALTER TABLE [dbo].[EomAccountManagerViewRows]
ADD CONSTRAINT [PK_EomAccountManagerViewRows]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------