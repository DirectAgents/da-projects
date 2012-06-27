
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/02/2011 20:04:21
-- Generated from EDMX file: C:\zzzDA\vs\sln\DA\DirectTrack.Rest\Models\DirectTrack.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DirectTrack2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ResourceTypeResource]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Resources] DROP CONSTRAINT [FK_ResourceTypeResource];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Resources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Resources];
GO
IF OBJECT_ID(N'[dbo].[ResourceTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ResourceTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Resources'
CREATE TABLE [dbo].[Resources] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Location] nvarchar(500)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [Got] datetime  NULL,
    [Posted] datetime  NULL,
    [ResourceTypeId] int  NOT NULL
);
GO

-- Creating table 'ResourceTypes'
CREATE TABLE [dbo].[ResourceTypes] (
    [Id] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [PK_Resources]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ResourceTypes'
ALTER TABLE [dbo].[ResourceTypes]
ADD CONSTRAINT [PK_ResourceTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ResourceTypeId] in table 'Resources'
ALTER TABLE [dbo].[Resources]
ADD CONSTRAINT [FK_ResourceTypeResource]
    FOREIGN KEY ([ResourceTypeId])
    REFERENCES [dbo].[ResourceTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ResourceTypeResource'
CREATE INDEX [IX_FK_ResourceTypeResource]
ON [dbo].[Resources]
    ([ResourceTypeId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------