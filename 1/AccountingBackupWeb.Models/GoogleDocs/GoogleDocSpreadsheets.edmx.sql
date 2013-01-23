
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/27/2012 18:17:38
-- Generated from EDMX file: C:\Code2011\da2\AccountingBackupWeb.Models\GoogleDocs\GoogleDocSpreadsheets.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [GoogleDocsSpreadsheets];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_WorksheetCell]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cells] DROP CONSTRAINT [FK_WorksheetCell];
GO
IF OBJECT_ID(N'[dbo].[FK_SpreadsheetWorksheet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Worksheets] DROP CONSTRAINT [FK_SpreadsheetWorksheet];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Spreadsheets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Spreadsheets];
GO
IF OBJECT_ID(N'[dbo].[Worksheets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Worksheets];
GO
IF OBJECT_ID(N'[dbo].[Cells]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cells];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Spreadsheets'
CREATE TABLE [dbo].[Spreadsheets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Worksheets'
CREATE TABLE [dbo].[Worksheets] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(200)  NOT NULL,
    [Spreadsheet_Id] int  NOT NULL
);
GO

-- Creating table 'Cells'
CREATE TABLE [dbo].[Cells] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Row] int  NOT NULL,
    [Column] int  NOT NULL,
    [Value] nvarchar(500)  NOT NULL,
    [Worksheet_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Spreadsheets'
ALTER TABLE [dbo].[Spreadsheets]
ADD CONSTRAINT [PK_Spreadsheets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Worksheets'
ALTER TABLE [dbo].[Worksheets]
ADD CONSTRAINT [PK_Worksheets]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cells'
ALTER TABLE [dbo].[Cells]
ADD CONSTRAINT [PK_Cells]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Worksheet_Id] in table 'Cells'
ALTER TABLE [dbo].[Cells]
ADD CONSTRAINT [FK_WorksheetCell]
    FOREIGN KEY ([Worksheet_Id])
    REFERENCES [dbo].[Worksheets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_WorksheetCell'
CREATE INDEX [IX_FK_WorksheetCell]
ON [dbo].[Cells]
    ([Worksheet_Id]);
GO

-- Creating foreign key on [Spreadsheet_Id] in table 'Worksheets'
ALTER TABLE [dbo].[Worksheets]
ADD CONSTRAINT [FK_SpreadsheetWorksheet]
    FOREIGN KEY ([Spreadsheet_Id])
    REFERENCES [dbo].[Spreadsheets]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SpreadsheetWorksheet'
CREATE INDEX [IX_FK_SpreadsheetWorksheet]
ON [dbo].[Worksheets]
    ([Spreadsheet_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------