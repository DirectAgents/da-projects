
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/17/2011 12:02:09
-- Generated from EDMX file: C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\AB2\Model\ABModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DirectAgents7];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Amount_Unit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Amounts] DROP CONSTRAINT [FK_Amount_Unit];
GO
IF OBJECT_ID(N'[dbo].[FK_Item_Amount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_Item_Amount];
GO
IF OBJECT_ID(N'[dbo].[FK_Item_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_Item_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitConversion_Unit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitConversions] DROP CONSTRAINT [FK_UnitConversion_Unit];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitConversion_Unit1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitConversions] DROP CONSTRAINT [FK_UnitConversion_Unit1];
GO
IF OBJECT_ID(N'[dbo].[FK_Advertiser_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisers] DROP CONSTRAINT [FK_Advertiser_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_Customer_Client]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_Customer_Client];
GO
IF OBJECT_ID(N'[dbo].[FK_Credit_Item]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Credits] DROP CONSTRAINT [FK_Credit_Item];
GO
IF OBJECT_ID(N'[dbo].[FK_Charge_Debit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Charges] DROP CONSTRAINT [FK_Charge_Debit];
GO
IF OBJECT_ID(N'[dbo].[FK_Debit_Item]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Debits] DROP CONSTRAINT [FK_Debit_Item];
GO
IF OBJECT_ID(N'[dbo].[FK_Item_DateSpan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Items] DROP CONSTRAINT [FK_Item_DateSpan];
GO
IF OBJECT_ID(N'[dbo].[FK_Period_DateSpan]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Periods] DROP CONSTRAINT [FK_Period_DateSpan];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertisingRevenue_Charge]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdvertisingRevenues] DROP CONSTRAINT [FK_AdvertisingRevenue_Charge];
GO
IF OBJECT_ID(N'[dbo].[FK_DateSpanUnitConversion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UnitConversions] DROP CONSTRAINT [FK_DateSpanUnitConversion];
GO
IF OBJECT_ID(N'[dbo].[FK_SqlServerDatabasePeriod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SqlServerDatabases] DROP CONSTRAINT [FK_SqlServerDatabasePeriod];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientPayTerm]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Clients] DROP CONSTRAINT [FK_ClientPayTerm];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertisingRevenueCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdvertisingRevenues] DROP CONSTRAINT [FK_AdvertisingRevenueCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentCredit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_PaymentCredit];
GO
IF OBJECT_ID(N'[dbo].[FK_QuickBooksReceivedPaymentQuickBooksCompanyFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuickBooksReceivedPayments] DROP CONSTRAINT [FK_QuickBooksReceivedPaymentQuickBooksCompanyFile];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentQuickBooksReceivedPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_PaymentQuickBooksReceivedPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerQuickBooksCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuickBooksCustomers] DROP CONSTRAINT [FK_CustomerQuickBooksCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectTrackAdvertiserAdvertiser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DirectTrackAdvertisers] DROP CONSTRAINT [FK_DirectTrackAdvertiserAdvertiser];
GO
IF OBJECT_ID(N'[dbo].[FK_QuickBooksCompanyFileQuickBooksCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuickBooksCustomers] DROP CONSTRAINT [FK_QuickBooksCompanyFileQuickBooksCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_QuickBooksCustomerQuickBooksReceivedPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuickBooksReceivedPayments] DROP CONSTRAINT [FK_QuickBooksCustomerQuickBooksReceivedPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_LogEntry_Log]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LogEntries] DROP CONSTRAINT [FK_LogEntry_Log];
GO
IF OBJECT_ID(N'[dbo].[FK_LogEntryTypeLogEntry]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LogEntries] DROP CONSTRAINT [FK_LogEntryTypeLogEntry];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertiserCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_AdvertiserCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_SqlServerDatabaseExternalItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExternalItems] DROP CONSTRAINT [FK_SqlServerDatabaseExternalItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ExternalItemAdvertisingRevenue]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExternalItems] DROP CONSTRAINT [FK_ExternalItemAdvertisingRevenue];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientCreditLimit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Clients] DROP CONSTRAINT [FK_ClientCreditLimit];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientStartingBalance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Credits_StartingBalance] DROP CONSTRAINT [FK_ClientStartingBalance];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectTrackCampaignGroupDirectTrackCampaign_DirectTrackCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DirectTrackCampaignGroupDirectTrackCampaign] DROP CONSTRAINT [FK_DirectTrackCampaignGroupDirectTrackCampaign_DirectTrackCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectTrackCampaignGroupDirectTrackCampaign_DirectTrackCampaignGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DirectTrackCampaignGroupDirectTrackCampaign] DROP CONSTRAINT [FK_DirectTrackCampaignGroupDirectTrackCampaign_DirectTrackCampaignGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectTrackResourceDirectTrackCampaignGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DirectTrackCampaignGroups] DROP CONSTRAINT [FK_DirectTrackResourceDirectTrackCampaignGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_DirectTrackResourceDirectTrackCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DirectTrackCampaigns] DROP CONSTRAINT [FK_DirectTrackResourceDirectTrackCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_CampaignDirectTrackCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DirectTrackCampaigns] DROP CONSTRAINT [FK_CampaignDirectTrackCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_CreditLimit_inherits_Amount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Amounts_CreditLimit] DROP CONSTRAINT [FK_CreditLimit_inherits_Amount];
GO
IF OBJECT_ID(N'[dbo].[FK_StartingBalance_inherits_Credit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Credits_StartingBalance] DROP CONSTRAINT [FK_StartingBalance_inherits_Credit];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Amounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Amounts];
GO
IF OBJECT_ID(N'[dbo].[Items]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items];
GO
IF OBJECT_ID(N'[dbo].[Periods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Periods];
GO
IF OBJECT_ID(N'[dbo].[Units]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Units];
GO
IF OBJECT_ID(N'[dbo].[UnitConversions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitConversions];
GO
IF OBJECT_ID(N'[dbo].[Clients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Clients];
GO
IF OBJECT_ID(N'[dbo].[SqlServerDatabases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SqlServerDatabases];
GO
IF OBJECT_ID(N'[dbo].[Advertisers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advertisers];
GO
IF OBJECT_ID(N'[dbo].[QuickBooksCompanyFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuickBooksCompanyFiles];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Credits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Credits];
GO
IF OBJECT_ID(N'[dbo].[Debits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Debits];
GO
IF OBJECT_ID(N'[dbo].[Charges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Charges];
GO
IF OBJECT_ID(N'[dbo].[DateSpans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DateSpans];
GO
IF OBJECT_ID(N'[dbo].[AdvertisingRevenues]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdvertisingRevenues];
GO
IF OBJECT_ID(N'[dbo].[PayTerms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PayTerms];
GO
IF OBJECT_ID(N'[dbo].[Campaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Campaigns];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackCampaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackCampaigns];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO
IF OBJECT_ID(N'[dbo].[QuickBooksReceivedPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuickBooksReceivedPayments];
GO
IF OBJECT_ID(N'[dbo].[QuickBooksCustomers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuickBooksCustomers];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackAdvertisers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackAdvertisers];
GO
IF OBJECT_ID(N'[dbo].[Logs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Logs];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackResources]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackResources];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackInstances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackInstances];
GO
IF OBJECT_ID(N'[dbo].[Permissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Permissions];
GO
IF OBJECT_ID(N'[dbo].[PermissionGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PermissionGroups];
GO
IF OBJECT_ID(N'[dbo].[Tasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tasks];
GO
IF OBJECT_ID(N'[dbo].[TaskGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskGroups];
GO
IF OBJECT_ID(N'[dbo].[Schedules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Schedules];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackPayouts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackPayouts];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackAdvertiserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackAdvertiserGroups];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackAffiliates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackAffiliates];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackAffiliateGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackAffiliateGroups];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackStats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackStats];
GO
IF OBJECT_ID(N'[dbo].[QuickBooksInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuickBooksInvoices];
GO
IF OBJECT_ID(N'[dbo].[Notifications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Notifications];
GO
IF OBJECT_ID(N'[dbo].[NotificationGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NotificationGroups];
GO
IF OBJECT_ID(N'[dbo].[EmailServices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmailServices];
GO
IF OBJECT_ID(N'[dbo].[Messages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Messages];
GO
IF OBJECT_ID(N'[dbo].[Menus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menus];
GO
IF OBJECT_ID(N'[dbo].[MenuItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MenuItems];
GO
IF OBJECT_ID(N'[dbo].[Apps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Apps];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
GO
IF OBJECT_ID(N'[dbo].[Screens]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Screens];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackCreatives]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackCreatives];
GO
IF OBJECT_ID(N'[dbo].[LogEntries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogEntries];
GO
IF OBJECT_ID(N'[dbo].[LogEntryTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogEntryTypes];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackAllowedCountries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackAllowedCountries];
GO
IF OBJECT_ID(N'[dbo].[ExternalItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExternalItems];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackCampaignGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackCampaignGroups];
GO
IF OBJECT_ID(N'[dbo].[Amounts_CreditLimit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Amounts_CreditLimit];
GO
IF OBJECT_ID(N'[dbo].[Credits_StartingBalance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Credits_StartingBalance];
GO
IF OBJECT_ID(N'[dbo].[DirectTrackCampaignGroupDirectTrackCampaign]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DirectTrackCampaignGroupDirectTrackCampaign];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Amounts'
CREATE TABLE [dbo].[Amounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] decimal(18,0)  NOT NULL,
    [Unit_Id] int  NOT NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount_Id] int  NOT NULL,
    [Client_Id] int  NOT NULL,
    [DateSpan_Id] int  NOT NULL
);
GO

-- Creating table 'Periods'
CREATE TABLE [dbo].[Periods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [DateSpan_Id] int  NOT NULL
);
GO

-- Creating table 'Units'
CREATE TABLE [dbo].[Units] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'UnitConversions'
CREATE TABLE [dbo].[UnitConversions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Multiplier] decimal(14,4)  NOT NULL,
    [From_Id] int  NOT NULL,
    [To_Id] int  NOT NULL,
    [DateSpan_Id] int  NOT NULL
);
GO

-- Creating table 'Clients'
CREATE TABLE [dbo].[Clients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [PayTerm_Id] int  NOT NULL,
    [CreditLimit_Id] int  NOT NULL
);
GO

-- Creating table 'SqlServerDatabases'
CREATE TABLE [dbo].[SqlServerDatabases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [ConnectionString] nvarchar(500)  NOT NULL,
    [Period_Id] int  NOT NULL
);
GO

-- Creating table 'Advertisers'
CREATE TABLE [dbo].[Advertisers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Client_Id] int  NOT NULL
);
GO

-- Creating table 'QuickBooksCompanyFiles'
CREATE TABLE [dbo].[QuickBooksCompanyFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(255)  NOT NULL,
    [Client_Id] int  NOT NULL
);
GO

-- Creating table 'Credits'
CREATE TABLE [dbo].[Credits] (
    [CreditId] int IDENTITY(1,1) NOT NULL,
    [Item_Id] int  NOT NULL
);
GO

-- Creating table 'Debits'
CREATE TABLE [dbo].[Debits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Item_Id] int  NOT NULL
);
GO

-- Creating table 'Charges'
CREATE TABLE [dbo].[Charges] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Debit_Id] int  NOT NULL
);
GO

-- Creating table 'DateSpans'
CREATE TABLE [dbo].[DateSpans] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [From] datetime  NOT NULL,
    [To] datetime  NULL
);
GO

-- Creating table 'AdvertisingRevenues'
CREATE TABLE [dbo].[AdvertisingRevenues] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Charge_Id] int  NULL,
    [Campaign_Id] int  NOT NULL
);
GO

-- Creating table 'PayTerms'
CREATE TABLE [dbo].[PayTerms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Campaigns'
CREATE TABLE [dbo].[Campaigns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Advertiser_Id] int  NULL
);
GO

-- Creating table 'DirectTrackCampaigns'
CREATE TABLE [dbo].[DirectTrackCampaigns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CampaignNumber] int  NOT NULL,
    [CampaignName] nvarchar(100)  NOT NULL,
    [DirectTrackResource_Id] int  NOT NULL,
    [Campaign_Id] int  NOT NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Credit_CreditId] int  NOT NULL,
    [QuickBooksReceivedPayment_Id] int  NOT NULL
);
GO

-- Creating table 'QuickBooksReceivedPayments'
CREATE TABLE [dbo].[QuickBooksReceivedPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TxnNumber] nvarchar(max)  NOT NULL,
    [Memo] nvarchar(max)  NULL,
    [TxnId] nvarchar(max)  NOT NULL,
    [ARAccountRefFullName] nvarchar(max)  NOT NULL,
    [TxnDate] datetime  NOT NULL,
    [TotalAmount] decimal(18,0)  NOT NULL,
    [PaymentMethodRefFullName] nvarchar(max)  NOT NULL,
    [QuickBooksCompanyFile_Id] int  NOT NULL,
    [QuickBooksCustomer_Id] int  NOT NULL
);
GO

-- Creating table 'QuickBooksCustomers'
CREATE TABLE [dbo].[QuickBooksCustomers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ListId] nvarchar(max)  NOT NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [CompanyName] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NOT NULL,
    [TermsRefFullName] nvarchar(max)  NOT NULL,
    [Customer_Id] int  NULL,
    [QuickBooksCompanyFile_Id] int  NOT NULL
);
GO

-- Creating table 'DirectTrackAdvertisers'
CREATE TABLE [dbo].[DirectTrackAdvertisers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdvertiserId] int  NOT NULL,
    [Advertiser_Id] int  NULL
);
GO

-- Creating table 'Logs'
CREATE TABLE [dbo].[Logs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DirectTrackResources'
CREATE TABLE [dbo].[DirectTrackResources] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Location] nvarchar(500)  NOT NULL,
    [XmlDoc] nvarchar(max)  NOT NULL,
    [Updated] datetime  NOT NULL,
    [ResourceName] nvarchar(50)  NOT NULL,
    [Pushed] datetime  NULL,
    [Posted] datetime  NULL
);
GO

-- Creating table 'DirectTrackInstances'
CREATE TABLE [dbo].[DirectTrackInstances] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Permissions'
CREATE TABLE [dbo].[Permissions] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'PermissionGroups'
CREATE TABLE [dbo].[PermissionGroups] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'TaskGroups'
CREATE TABLE [dbo].[TaskGroups] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Schedules'
CREATE TABLE [dbo].[Schedules] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'DirectTrackPayouts'
CREATE TABLE [dbo].[DirectTrackPayouts] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'DirectTrackAdvertiserGroups'
CREATE TABLE [dbo].[DirectTrackAdvertiserGroups] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'DirectTrackAffiliates'
CREATE TABLE [dbo].[DirectTrackAffiliates] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'DirectTrackAffiliateGroups'
CREATE TABLE [dbo].[DirectTrackAffiliateGroups] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'DirectTrackStats'
CREATE TABLE [dbo].[DirectTrackStats] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'QuickBooksInvoices'
CREATE TABLE [dbo].[QuickBooksInvoices] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Notifications'
CREATE TABLE [dbo].[Notifications] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'NotificationGroups'
CREATE TABLE [dbo].[NotificationGroups] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'EmailServices'
CREATE TABLE [dbo].[EmailServices] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Menus'
CREATE TABLE [dbo].[Menus] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'MenuItems'
CREATE TABLE [dbo].[MenuItems] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Apps'
CREATE TABLE [dbo].[Apps] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(100)  NOT NULL
);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Screens'
CREATE TABLE [dbo].[Screens] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'DirectTrackCreatives'
CREATE TABLE [dbo].[DirectTrackCreatives] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'LogEntries'
CREATE TABLE [dbo].[LogEntries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StringData] nvarchar(255)  NULL,
    [Created] datetime  NULL,
    [Log_Id] int  NOT NULL,
    [LogEntryType_Id] int  NOT NULL
);
GO

-- Creating table 'LogEntryTypes'
CREATE TABLE [dbo].[LogEntryTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DirectTrackAllowedCountries'
CREATE TABLE [dbo].[DirectTrackAllowedCountries] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'ExternalItems'
CREATE TABLE [dbo].[ExternalItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ExternalId] int  NOT NULL,
    [SqlServerDatabase_Id] int  NOT NULL,
    [AdvertisingRevenue_Id] int  NULL
);
GO

-- Creating table 'DirectTrackCampaignGroups'
CREATE TABLE [dbo].[DirectTrackCampaignGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [DirectTrackResource_Id] int  NOT NULL
);
GO

-- Creating table 'Amounts_CreditLimit'
CREATE TABLE [dbo].[Amounts_CreditLimit] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Credits_StartingBalance'
CREATE TABLE [dbo].[Credits_StartingBalance] (
    [CreditId] int  NOT NULL,
    [Client_Id] int  NOT NULL
);
GO

-- Creating table 'DirectTrackCampaignGroupDirectTrackCampaign'
CREATE TABLE [dbo].[DirectTrackCampaignGroupDirectTrackCampaign] (
    [DirectTrackCampaigns_Id] int  NOT NULL,
    [DirectTrackCampaignGroups_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Amounts'
ALTER TABLE [dbo].[Amounts]
ADD CONSTRAINT [PK_Amounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Periods'
ALTER TABLE [dbo].[Periods]
ADD CONSTRAINT [PK_Periods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Units'
ALTER TABLE [dbo].[Units]
ADD CONSTRAINT [PK_Units]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UnitConversions'
ALTER TABLE [dbo].[UnitConversions]
ADD CONSTRAINT [PK_UnitConversions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Clients'
ALTER TABLE [dbo].[Clients]
ADD CONSTRAINT [PK_Clients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SqlServerDatabases'
ALTER TABLE [dbo].[SqlServerDatabases]
ADD CONSTRAINT [PK_SqlServerDatabases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Advertisers'
ALTER TABLE [dbo].[Advertisers]
ADD CONSTRAINT [PK_Advertisers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuickBooksCompanyFiles'
ALTER TABLE [dbo].[QuickBooksCompanyFiles]
ADD CONSTRAINT [PK_QuickBooksCompanyFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [CreditId] in table 'Credits'
ALTER TABLE [dbo].[Credits]
ADD CONSTRAINT [PK_Credits]
    PRIMARY KEY CLUSTERED ([CreditId] ASC);
GO

-- Creating primary key on [Id] in table 'Debits'
ALTER TABLE [dbo].[Debits]
ADD CONSTRAINT [PK_Debits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Charges'
ALTER TABLE [dbo].[Charges]
ADD CONSTRAINT [PK_Charges]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DateSpans'
ALTER TABLE [dbo].[DateSpans]
ADD CONSTRAINT [PK_DateSpans]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AdvertisingRevenues'
ALTER TABLE [dbo].[AdvertisingRevenues]
ADD CONSTRAINT [PK_AdvertisingRevenues]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PayTerms'
ALTER TABLE [dbo].[PayTerms]
ADD CONSTRAINT [PK_PayTerms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [PK_Campaigns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackCampaigns'
ALTER TABLE [dbo].[DirectTrackCampaigns]
ADD CONSTRAINT [PK_DirectTrackCampaigns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuickBooksReceivedPayments'
ALTER TABLE [dbo].[QuickBooksReceivedPayments]
ADD CONSTRAINT [PK_QuickBooksReceivedPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuickBooksCustomers'
ALTER TABLE [dbo].[QuickBooksCustomers]
ADD CONSTRAINT [PK_QuickBooksCustomers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [AdvertiserId] in table 'DirectTrackAdvertisers'
ALTER TABLE [dbo].[DirectTrackAdvertisers]
ADD CONSTRAINT [PK_DirectTrackAdvertisers]
    PRIMARY KEY CLUSTERED ([Id], [AdvertiserId] ASC);
GO

-- Creating primary key on [Id] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [PK_Logs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackResources'
ALTER TABLE [dbo].[DirectTrackResources]
ADD CONSTRAINT [PK_DirectTrackResources]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackInstances'
ALTER TABLE [dbo].[DirectTrackInstances]
ADD CONSTRAINT [PK_DirectTrackInstances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [PK_Permissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PermissionGroups'
ALTER TABLE [dbo].[PermissionGroups]
ADD CONSTRAINT [PK_PermissionGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaskGroups'
ALTER TABLE [dbo].[TaskGroups]
ADD CONSTRAINT [PK_TaskGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Schedules'
ALTER TABLE [dbo].[Schedules]
ADD CONSTRAINT [PK_Schedules]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackPayouts'
ALTER TABLE [dbo].[DirectTrackPayouts]
ADD CONSTRAINT [PK_DirectTrackPayouts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackAdvertiserGroups'
ALTER TABLE [dbo].[DirectTrackAdvertiserGroups]
ADD CONSTRAINT [PK_DirectTrackAdvertiserGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackAffiliates'
ALTER TABLE [dbo].[DirectTrackAffiliates]
ADD CONSTRAINT [PK_DirectTrackAffiliates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackAffiliateGroups'
ALTER TABLE [dbo].[DirectTrackAffiliateGroups]
ADD CONSTRAINT [PK_DirectTrackAffiliateGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackStats'
ALTER TABLE [dbo].[DirectTrackStats]
ADD CONSTRAINT [PK_DirectTrackStats]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuickBooksInvoices'
ALTER TABLE [dbo].[QuickBooksInvoices]
ADD CONSTRAINT [PK_QuickBooksInvoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notifications'
ALTER TABLE [dbo].[Notifications]
ADD CONSTRAINT [PK_Notifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NotificationGroups'
ALTER TABLE [dbo].[NotificationGroups]
ADD CONSTRAINT [PK_NotificationGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmailServices'
ALTER TABLE [dbo].[EmailServices]
ADD CONSTRAINT [PK_EmailServices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Menus'
ALTER TABLE [dbo].[Menus]
ADD CONSTRAINT [PK_Menus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MenuItems'
ALTER TABLE [dbo].[MenuItems]
ADD CONSTRAINT [PK_MenuItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Apps'
ALTER TABLE [dbo].[Apps]
ADD CONSTRAINT [PK_Apps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [PK_UserGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Screens'
ALTER TABLE [dbo].[Screens]
ADD CONSTRAINT [PK_Screens]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackCreatives'
ALTER TABLE [dbo].[DirectTrackCreatives]
ADD CONSTRAINT [PK_DirectTrackCreatives]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LogEntries'
ALTER TABLE [dbo].[LogEntries]
ADD CONSTRAINT [PK_LogEntries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LogEntryTypes'
ALTER TABLE [dbo].[LogEntryTypes]
ADD CONSTRAINT [PK_LogEntryTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackAllowedCountries'
ALTER TABLE [dbo].[DirectTrackAllowedCountries]
ADD CONSTRAINT [PK_DirectTrackAllowedCountries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExternalItems'
ALTER TABLE [dbo].[ExternalItems]
ADD CONSTRAINT [PK_ExternalItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DirectTrackCampaignGroups'
ALTER TABLE [dbo].[DirectTrackCampaignGroups]
ADD CONSTRAINT [PK_DirectTrackCampaignGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Amounts_CreditLimit'
ALTER TABLE [dbo].[Amounts_CreditLimit]
ADD CONSTRAINT [PK_Amounts_CreditLimit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [CreditId] in table 'Credits_StartingBalance'
ALTER TABLE [dbo].[Credits_StartingBalance]
ADD CONSTRAINT [PK_Credits_StartingBalance]
    PRIMARY KEY CLUSTERED ([CreditId] ASC);
GO

-- Creating primary key on [DirectTrackCampaigns_Id], [DirectTrackCampaignGroups_Id] in table 'DirectTrackCampaignGroupDirectTrackCampaign'
ALTER TABLE [dbo].[DirectTrackCampaignGroupDirectTrackCampaign]
ADD CONSTRAINT [PK_DirectTrackCampaignGroupDirectTrackCampaign]
    PRIMARY KEY NONCLUSTERED ([DirectTrackCampaigns_Id], [DirectTrackCampaignGroups_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Unit_Id] in table 'Amounts'
ALTER TABLE [dbo].[Amounts]
ADD CONSTRAINT [FK_Amount_Unit]
    FOREIGN KEY ([Unit_Id])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Amount_Unit'
CREATE INDEX [IX_FK_Amount_Unit]
ON [dbo].[Amounts]
    ([Unit_Id]);
GO

-- Creating foreign key on [Amount_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_Item_Amount]
    FOREIGN KEY ([Amount_Id])
    REFERENCES [dbo].[Amounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_Amount'
CREATE INDEX [IX_FK_Item_Amount]
ON [dbo].[Items]
    ([Amount_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_Item_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Clients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_Client'
CREATE INDEX [IX_FK_Item_Client]
ON [dbo].[Items]
    ([Client_Id]);
GO

-- Creating foreign key on [From_Id] in table 'UnitConversions'
ALTER TABLE [dbo].[UnitConversions]
ADD CONSTRAINT [FK_UnitConversion_Unit]
    FOREIGN KEY ([From_Id])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitConversion_Unit'
CREATE INDEX [IX_FK_UnitConversion_Unit]
ON [dbo].[UnitConversions]
    ([From_Id]);
GO

-- Creating foreign key on [To_Id] in table 'UnitConversions'
ALTER TABLE [dbo].[UnitConversions]
ADD CONSTRAINT [FK_UnitConversion_Unit1]
    FOREIGN KEY ([To_Id])
    REFERENCES [dbo].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitConversion_Unit1'
CREATE INDEX [IX_FK_UnitConversion_Unit1]
ON [dbo].[UnitConversions]
    ([To_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'Advertisers'
ALTER TABLE [dbo].[Advertisers]
ADD CONSTRAINT [FK_Advertiser_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Clients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Advertiser_Client'
CREATE INDEX [IX_FK_Advertiser_Client]
ON [dbo].[Advertisers]
    ([Client_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_Customer_Client]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Clients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Customer_Client'
CREATE INDEX [IX_FK_Customer_Client]
ON [dbo].[Customers]
    ([Client_Id]);
GO

-- Creating foreign key on [Item_Id] in table 'Credits'
ALTER TABLE [dbo].[Credits]
ADD CONSTRAINT [FK_Credit_Item]
    FOREIGN KEY ([Item_Id])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Credit_Item'
CREATE INDEX [IX_FK_Credit_Item]
ON [dbo].[Credits]
    ([Item_Id]);
GO

-- Creating foreign key on [Debit_Id] in table 'Charges'
ALTER TABLE [dbo].[Charges]
ADD CONSTRAINT [FK_Charge_Debit]
    FOREIGN KEY ([Debit_Id])
    REFERENCES [dbo].[Debits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Charge_Debit'
CREATE INDEX [IX_FK_Charge_Debit]
ON [dbo].[Charges]
    ([Debit_Id]);
GO

-- Creating foreign key on [Item_Id] in table 'Debits'
ALTER TABLE [dbo].[Debits]
ADD CONSTRAINT [FK_Debit_Item]
    FOREIGN KEY ([Item_Id])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Debit_Item'
CREATE INDEX [IX_FK_Debit_Item]
ON [dbo].[Debits]
    ([Item_Id]);
GO

-- Creating foreign key on [DateSpan_Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [FK_Item_DateSpan]
    FOREIGN KEY ([DateSpan_Id])
    REFERENCES [dbo].[DateSpans]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_DateSpan'
CREATE INDEX [IX_FK_Item_DateSpan]
ON [dbo].[Items]
    ([DateSpan_Id]);
GO

-- Creating foreign key on [DateSpan_Id] in table 'Periods'
ALTER TABLE [dbo].[Periods]
ADD CONSTRAINT [FK_Period_DateSpan]
    FOREIGN KEY ([DateSpan_Id])
    REFERENCES [dbo].[DateSpans]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Period_DateSpan'
CREATE INDEX [IX_FK_Period_DateSpan]
ON [dbo].[Periods]
    ([DateSpan_Id]);
GO

-- Creating foreign key on [Charge_Id] in table 'AdvertisingRevenues'
ALTER TABLE [dbo].[AdvertisingRevenues]
ADD CONSTRAINT [FK_AdvertisingRevenue_Charge]
    FOREIGN KEY ([Charge_Id])
    REFERENCES [dbo].[Charges]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertisingRevenue_Charge'
CREATE INDEX [IX_FK_AdvertisingRevenue_Charge]
ON [dbo].[AdvertisingRevenues]
    ([Charge_Id]);
GO

-- Creating foreign key on [DateSpan_Id] in table 'UnitConversions'
ALTER TABLE [dbo].[UnitConversions]
ADD CONSTRAINT [FK_DateSpanUnitConversion]
    FOREIGN KEY ([DateSpan_Id])
    REFERENCES [dbo].[DateSpans]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DateSpanUnitConversion'
CREATE INDEX [IX_FK_DateSpanUnitConversion]
ON [dbo].[UnitConversions]
    ([DateSpan_Id]);
GO

-- Creating foreign key on [Period_Id] in table 'SqlServerDatabases'
ALTER TABLE [dbo].[SqlServerDatabases]
ADD CONSTRAINT [FK_SqlServerDatabasePeriod]
    FOREIGN KEY ([Period_Id])
    REFERENCES [dbo].[Periods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SqlServerDatabasePeriod'
CREATE INDEX [IX_FK_SqlServerDatabasePeriod]
ON [dbo].[SqlServerDatabases]
    ([Period_Id]);
GO

-- Creating foreign key on [PayTerm_Id] in table 'Clients'
ALTER TABLE [dbo].[Clients]
ADD CONSTRAINT [FK_ClientPayTerm]
    FOREIGN KEY ([PayTerm_Id])
    REFERENCES [dbo].[PayTerms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientPayTerm'
CREATE INDEX [IX_FK_ClientPayTerm]
ON [dbo].[Clients]
    ([PayTerm_Id]);
GO

-- Creating foreign key on [Campaign_Id] in table 'AdvertisingRevenues'
ALTER TABLE [dbo].[AdvertisingRevenues]
ADD CONSTRAINT [FK_AdvertisingRevenueCampaign]
    FOREIGN KEY ([Campaign_Id])
    REFERENCES [dbo].[Campaigns]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertisingRevenueCampaign'
CREATE INDEX [IX_FK_AdvertisingRevenueCampaign]
ON [dbo].[AdvertisingRevenues]
    ([Campaign_Id]);
GO

-- Creating foreign key on [Credit_CreditId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_PaymentCredit]
    FOREIGN KEY ([Credit_CreditId])
    REFERENCES [dbo].[Credits]
        ([CreditId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentCredit'
CREATE INDEX [IX_FK_PaymentCredit]
ON [dbo].[Payments]
    ([Credit_CreditId]);
GO

-- Creating foreign key on [QuickBooksCompanyFile_Id] in table 'QuickBooksReceivedPayments'
ALTER TABLE [dbo].[QuickBooksReceivedPayments]
ADD CONSTRAINT [FK_QuickBooksReceivedPaymentQuickBooksCompanyFile]
    FOREIGN KEY ([QuickBooksCompanyFile_Id])
    REFERENCES [dbo].[QuickBooksCompanyFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuickBooksReceivedPaymentQuickBooksCompanyFile'
CREATE INDEX [IX_FK_QuickBooksReceivedPaymentQuickBooksCompanyFile]
ON [dbo].[QuickBooksReceivedPayments]
    ([QuickBooksCompanyFile_Id]);
GO

-- Creating foreign key on [QuickBooksReceivedPayment_Id] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_PaymentQuickBooksReceivedPayment]
    FOREIGN KEY ([QuickBooksReceivedPayment_Id])
    REFERENCES [dbo].[QuickBooksReceivedPayments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentQuickBooksReceivedPayment'
CREATE INDEX [IX_FK_PaymentQuickBooksReceivedPayment]
ON [dbo].[Payments]
    ([QuickBooksReceivedPayment_Id]);
GO

-- Creating foreign key on [Customer_Id] in table 'QuickBooksCustomers'
ALTER TABLE [dbo].[QuickBooksCustomers]
ADD CONSTRAINT [FK_CustomerQuickBooksCustomer]
    FOREIGN KEY ([Customer_Id])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerQuickBooksCustomer'
CREATE INDEX [IX_FK_CustomerQuickBooksCustomer]
ON [dbo].[QuickBooksCustomers]
    ([Customer_Id]);
GO

-- Creating foreign key on [Advertiser_Id] in table 'DirectTrackAdvertisers'
ALTER TABLE [dbo].[DirectTrackAdvertisers]
ADD CONSTRAINT [FK_DirectTrackAdvertiserAdvertiser]
    FOREIGN KEY ([Advertiser_Id])
    REFERENCES [dbo].[Advertisers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectTrackAdvertiserAdvertiser'
CREATE INDEX [IX_FK_DirectTrackAdvertiserAdvertiser]
ON [dbo].[DirectTrackAdvertisers]
    ([Advertiser_Id]);
GO

-- Creating foreign key on [QuickBooksCompanyFile_Id] in table 'QuickBooksCustomers'
ALTER TABLE [dbo].[QuickBooksCustomers]
ADD CONSTRAINT [FK_QuickBooksCompanyFileQuickBooksCustomer]
    FOREIGN KEY ([QuickBooksCompanyFile_Id])
    REFERENCES [dbo].[QuickBooksCompanyFiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuickBooksCompanyFileQuickBooksCustomer'
CREATE INDEX [IX_FK_QuickBooksCompanyFileQuickBooksCustomer]
ON [dbo].[QuickBooksCustomers]
    ([QuickBooksCompanyFile_Id]);
GO

-- Creating foreign key on [QuickBooksCustomer_Id] in table 'QuickBooksReceivedPayments'
ALTER TABLE [dbo].[QuickBooksReceivedPayments]
ADD CONSTRAINT [FK_QuickBooksCustomerQuickBooksReceivedPayment]
    FOREIGN KEY ([QuickBooksCustomer_Id])
    REFERENCES [dbo].[QuickBooksCustomers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuickBooksCustomerQuickBooksReceivedPayment'
CREATE INDEX [IX_FK_QuickBooksCustomerQuickBooksReceivedPayment]
ON [dbo].[QuickBooksReceivedPayments]
    ([QuickBooksCustomer_Id]);
GO

-- Creating foreign key on [Log_Id] in table 'LogEntries'
ALTER TABLE [dbo].[LogEntries]
ADD CONSTRAINT [FK_LogEntry_Log]
    FOREIGN KEY ([Log_Id])
    REFERENCES [dbo].[Logs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LogEntry_Log'
CREATE INDEX [IX_FK_LogEntry_Log]
ON [dbo].[LogEntries]
    ([Log_Id]);
GO

-- Creating foreign key on [LogEntryType_Id] in table 'LogEntries'
ALTER TABLE [dbo].[LogEntries]
ADD CONSTRAINT [FK_LogEntryTypeLogEntry]
    FOREIGN KEY ([LogEntryType_Id])
    REFERENCES [dbo].[LogEntryTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LogEntryTypeLogEntry'
CREATE INDEX [IX_FK_LogEntryTypeLogEntry]
ON [dbo].[LogEntries]
    ([LogEntryType_Id]);
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

-- Creating foreign key on [SqlServerDatabase_Id] in table 'ExternalItems'
ALTER TABLE [dbo].[ExternalItems]
ADD CONSTRAINT [FK_SqlServerDatabaseExternalItem]
    FOREIGN KEY ([SqlServerDatabase_Id])
    REFERENCES [dbo].[SqlServerDatabases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SqlServerDatabaseExternalItem'
CREATE INDEX [IX_FK_SqlServerDatabaseExternalItem]
ON [dbo].[ExternalItems]
    ([SqlServerDatabase_Id]);
GO

-- Creating foreign key on [AdvertisingRevenue_Id] in table 'ExternalItems'
ALTER TABLE [dbo].[ExternalItems]
ADD CONSTRAINT [FK_ExternalItemAdvertisingRevenue]
    FOREIGN KEY ([AdvertisingRevenue_Id])
    REFERENCES [dbo].[AdvertisingRevenues]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ExternalItemAdvertisingRevenue'
CREATE INDEX [IX_FK_ExternalItemAdvertisingRevenue]
ON [dbo].[ExternalItems]
    ([AdvertisingRevenue_Id]);
GO

-- Creating foreign key on [CreditLimit_Id] in table 'Clients'
ALTER TABLE [dbo].[Clients]
ADD CONSTRAINT [FK_ClientCreditLimit]
    FOREIGN KEY ([CreditLimit_Id])
    REFERENCES [dbo].[Amounts_CreditLimit]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientCreditLimit'
CREATE INDEX [IX_FK_ClientCreditLimit]
ON [dbo].[Clients]
    ([CreditLimit_Id]);
GO

-- Creating foreign key on [Client_Id] in table 'Credits_StartingBalance'
ALTER TABLE [dbo].[Credits_StartingBalance]
ADD CONSTRAINT [FK_ClientStartingBalance]
    FOREIGN KEY ([Client_Id])
    REFERENCES [dbo].[Clients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientStartingBalance'
CREATE INDEX [IX_FK_ClientStartingBalance]
ON [dbo].[Credits_StartingBalance]
    ([Client_Id]);
GO

-- Creating foreign key on [DirectTrackCampaigns_Id] in table 'DirectTrackCampaignGroupDirectTrackCampaign'
ALTER TABLE [dbo].[DirectTrackCampaignGroupDirectTrackCampaign]
ADD CONSTRAINT [FK_DirectTrackCampaignGroupDirectTrackCampaign_DirectTrackCampaign]
    FOREIGN KEY ([DirectTrackCampaigns_Id])
    REFERENCES [dbo].[DirectTrackCampaigns]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [DirectTrackCampaignGroups_Id] in table 'DirectTrackCampaignGroupDirectTrackCampaign'
ALTER TABLE [dbo].[DirectTrackCampaignGroupDirectTrackCampaign]
ADD CONSTRAINT [FK_DirectTrackCampaignGroupDirectTrackCampaign_DirectTrackCampaignGroup]
    FOREIGN KEY ([DirectTrackCampaignGroups_Id])
    REFERENCES [dbo].[DirectTrackCampaignGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectTrackCampaignGroupDirectTrackCampaign_DirectTrackCampaignGroup'
CREATE INDEX [IX_FK_DirectTrackCampaignGroupDirectTrackCampaign_DirectTrackCampaignGroup]
ON [dbo].[DirectTrackCampaignGroupDirectTrackCampaign]
    ([DirectTrackCampaignGroups_Id]);
GO

-- Creating foreign key on [DirectTrackResource_Id] in table 'DirectTrackCampaignGroups'
ALTER TABLE [dbo].[DirectTrackCampaignGroups]
ADD CONSTRAINT [FK_DirectTrackResourceDirectTrackCampaignGroup]
    FOREIGN KEY ([DirectTrackResource_Id])
    REFERENCES [dbo].[DirectTrackResources]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectTrackResourceDirectTrackCampaignGroup'
CREATE INDEX [IX_FK_DirectTrackResourceDirectTrackCampaignGroup]
ON [dbo].[DirectTrackCampaignGroups]
    ([DirectTrackResource_Id]);
GO

-- Creating foreign key on [DirectTrackResource_Id] in table 'DirectTrackCampaigns'
ALTER TABLE [dbo].[DirectTrackCampaigns]
ADD CONSTRAINT [FK_DirectTrackResourceDirectTrackCampaign]
    FOREIGN KEY ([DirectTrackResource_Id])
    REFERENCES [dbo].[DirectTrackResources]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DirectTrackResourceDirectTrackCampaign'
CREATE INDEX [IX_FK_DirectTrackResourceDirectTrackCampaign]
ON [dbo].[DirectTrackCampaigns]
    ([DirectTrackResource_Id]);
GO

-- Creating foreign key on [Campaign_Id] in table 'DirectTrackCampaigns'
ALTER TABLE [dbo].[DirectTrackCampaigns]
ADD CONSTRAINT [FK_CampaignDirectTrackCampaign]
    FOREIGN KEY ([Campaign_Id])
    REFERENCES [dbo].[Campaigns]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CampaignDirectTrackCampaign'
CREATE INDEX [IX_FK_CampaignDirectTrackCampaign]
ON [dbo].[DirectTrackCampaigns]
    ([Campaign_Id]);
GO

-- Creating foreign key on [Id] in table 'Amounts_CreditLimit'
ALTER TABLE [dbo].[Amounts_CreditLimit]
ADD CONSTRAINT [FK_CreditLimit_inherits_Amount]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Amounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CreditId] in table 'Credits_StartingBalance'
ALTER TABLE [dbo].[Credits_StartingBalance]
ADD CONSTRAINT [FK_StartingBalance_inherits_Credit]
    FOREIGN KEY ([CreditId])
    REFERENCES [dbo].[Credits]
        ([CreditId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------