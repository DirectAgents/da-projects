
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/26/2012 11:32:16
-- Generated from EDMX file: C:\GitHub\da-projects\1\EomTool\EomApp1\Screens\Security\Models\EomToolSecurity.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [EomToolSecurity];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_RolePermission_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePermission] DROP CONSTRAINT [FK_RolePermission_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_RolePermission_Permission]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RolePermission] DROP CONSTRAINT [FK_RolePermission_Permission];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleGroup_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleGroup] DROP CONSTRAINT [FK_RoleGroup_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleGroup_Group]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleGroup] DROP CONSTRAINT [FK_RoleGroup_Group];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Permissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Permissions];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];
GO
IF OBJECT_ID(N'[dbo].[RolePermission]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RolePermission];
GO
IF OBJECT_ID(N'[dbo].[RoleGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleGroup];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Permissions'
CREATE TABLE [dbo].[Permissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Groups'
CREATE TABLE [dbo].[Groups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RolePermission'
CREATE TABLE [dbo].[RolePermission] (
    [Roles_Id] int  NOT NULL,
    [Permissions_Id] int  NOT NULL
);
GO

-- Creating table 'RoleGroup'
CREATE TABLE [dbo].[RoleGroup] (
    [Roles_Id] int  NOT NULL,
    [Groups_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [PK_Permissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Groups'
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [PK_Groups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Roles_Id], [Permissions_Id] in table 'RolePermission'
ALTER TABLE [dbo].[RolePermission]
ADD CONSTRAINT [PK_RolePermission]
    PRIMARY KEY NONCLUSTERED ([Roles_Id], [Permissions_Id] ASC);
GO

-- Creating primary key on [Roles_Id], [Groups_Id] in table 'RoleGroup'
ALTER TABLE [dbo].[RoleGroup]
ADD CONSTRAINT [PK_RoleGroup]
    PRIMARY KEY NONCLUSTERED ([Roles_Id], [Groups_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Roles_Id] in table 'RolePermission'
ALTER TABLE [dbo].[RolePermission]
ADD CONSTRAINT [FK_RolePermission_Role]
    FOREIGN KEY ([Roles_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Permissions_Id] in table 'RolePermission'
ALTER TABLE [dbo].[RolePermission]
ADD CONSTRAINT [FK_RolePermission_Permission]
    FOREIGN KEY ([Permissions_Id])
    REFERENCES [dbo].[Permissions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RolePermission_Permission'
CREATE INDEX [IX_FK_RolePermission_Permission]
ON [dbo].[RolePermission]
    ([Permissions_Id]);
GO

-- Creating foreign key on [Roles_Id] in table 'RoleGroup'
ALTER TABLE [dbo].[RoleGroup]
ADD CONSTRAINT [FK_RoleGroup_Role]
    FOREIGN KEY ([Roles_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Groups_Id] in table 'RoleGroup'
ALTER TABLE [dbo].[RoleGroup]
ADD CONSTRAINT [FK_RoleGroup_Group]
    FOREIGN KEY ([Groups_Id])
    REFERENCES [dbo].[Groups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleGroup_Group'
CREATE INDEX [IX_FK_RoleGroup_Group]
ON [dbo].[RoleGroup]
    ([Groups_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------