
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/28/2013 00:52:38
-- Generated from EDMX file: C:\GitHub\da-projects\3\QuickBooks.WebConnector\LogModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [QuickBooksWebConnector];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Entries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Entries];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Entries'
CREATE TABLE [dbo].[Entries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Uid] nvarchar(max)  NOT NULL,
    [Message] nvarchar(max)  NULL,
    [Timestamp] datetime  NOT NULL,
    [Parameters] nvarchar(max)  NULL,
    [Document] nvarchar(max)  NULL,
    [Ticket] nvarchar(max)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Entries'
ALTER TABLE [dbo].[Entries]
ADD CONSTRAINT [PK_Entries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------