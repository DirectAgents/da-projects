
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/22/2013 18:19:01
-- Generated from EDMX file: C:\GitHub\da-projects\1\AccountingBackupWeb.Models\AccountingBackup\AccountingBackupData.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ABWebDatabase1];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ARAccountReceivedPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReceivedPayments] DROP CONSTRAINT [FK_ARAccountReceivedPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_CurrencyARAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountsReceivable] DROP CONSTRAINT [FK_CurrencyARAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_QuickBooksCompanyCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_QuickBooksCompanyCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoices] DROP CONSTRAINT [FK_CustomerInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_TermsInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoices] DROP CONSTRAINT [FK_TermsInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertiserCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_AdvertiserCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerReceivedPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReceivedPayments] DROP CONSTRAINT [FK_CustomerReceivedPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountReceivableInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoices] DROP CONSTRAINT [FK_AccountReceivableInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_CurrencyCampaignSpend]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CampaignSpends] DROP CONSTRAINT [FK_CurrencyCampaignSpend];
GO
IF OBJECT_ID(N'[dbo].[FK_PeriodCampaignSpend]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CampaignSpends] DROP CONSTRAINT [FK_PeriodCampaignSpend];
GO
IF OBJECT_ID(N'[dbo].[FK_UnitTypeCampaignSpend]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CampaignSpends] DROP CONSTRAINT [FK_UnitTypeCampaignSpend];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertiserCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_AdvertiserCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_CampaignCampaignSpend]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CampaignSpends] DROP CONSTRAINT [FK_CampaignCampaignSpend];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeCampaign]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_EmployeeCampaign];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeCampaign1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Campaigns] DROP CONSTRAINT [FK_EmployeeCampaign1];
GO
IF OBJECT_ID(N'[dbo].[FK_CurrencyRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rates] DROP CONSTRAINT [FK_CurrencyRate];
GO
IF OBJECT_ID(N'[dbo].[FK_PeriodRate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Rates] DROP CONSTRAINT [FK_PeriodRate];
GO
IF OBJECT_ID(N'[dbo].[FK_TermsCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_TermsCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertiserTerms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisers] DROP CONSTRAINT [FK_AdvertiserTerms];
GO
IF OBJECT_ID(N'[dbo].[FK_AdvertiserCurrency]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Advertisers] DROP CONSTRAINT [FK_AdvertiserCurrency];
GO
IF OBJECT_ID(N'[dbo].[FK_LogEntryType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Logs] DROP CONSTRAINT [FK_LogEntryType];
GO
IF OBJECT_ID(N'[dbo].[FK_QuickBooksTableQuickBooksColumn]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuickBooksColumns] DROP CONSTRAINT [FK_QuickBooksTableQuickBooksColumn];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCreditMemo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CreditMemoes] DROP CONSTRAINT [FK_CustomerCreditMemo];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountReceivableCreditMemo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CreditMemoes] DROP CONSTRAINT [FK_AccountReceivableCreditMemo];
GO
IF OBJECT_ID(N'[dbo].[FK_TabTabGroup_Tab]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TabTabGroup] DROP CONSTRAINT [FK_TabTabGroup_Tab];
GO
IF OBJECT_ID(N'[dbo].[FK_TabTabGroup_TabGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TabTabGroup] DROP CONSTRAINT [FK_TabTabGroup_TabGroup];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Advertisers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Advertisers];
GO
IF OBJECT_ID(N'[dbo].[Currencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Currencies];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Periods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Periods];
GO
IF OBJECT_ID(N'[dbo].[SpendItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpendItems];
GO
IF OBJECT_ID(N'[dbo].[UnitTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UnitTypes];
GO
IF OBJECT_ID(N'[dbo].[Invoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoices];
GO
IF OBJECT_ID(N'[dbo].[Terms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Terms];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[ReceivedPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceivedPayments];
GO
IF OBJECT_ID(N'[dbo].[AccountsReceivable]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountsReceivable];
GO
IF OBJECT_ID(N'[dbo].[SynchTimes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SynchTimes];
GO
IF OBJECT_ID(N'[dbo].[Notes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Notes];
GO
IF OBJECT_ID(N'[dbo].[CampaignSpends]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CampaignSpends];
GO
IF OBJECT_ID(N'[dbo].[Campaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Campaigns];
GO
IF OBJECT_ID(N'[dbo].[Rates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rates];
GO
IF OBJECT_ID(N'[dbo].[vAdvertiserEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[vAdvertiserEmployees];
GO
IF OBJECT_ID(N'[dbo].[EntryTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EntryTypes];
GO
IF OBJECT_ID(N'[dbo].[Logs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Logs];
GO
IF OBJECT_ID(N'[dbo].[CreditMemoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditMemoes];
GO
IF OBJECT_ID(N'[dbo].[QuickBooksTables]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuickBooksTables];
GO
IF OBJECT_ID(N'[dbo].[QuickBooksColumns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuickBooksColumns];
GO
IF OBJECT_ID(N'[dbo].[BudgetReportRows]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BudgetReportRows];
GO
IF OBJECT_ID(N'[dbo].[Tabs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tabs];
GO
IF OBJECT_ID(N'[dbo].[TabGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TabGroups];
GO
IF OBJECT_ID(N'[dbo].[TabTabGroup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TabTabGroup];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Advertisers'
CREATE TABLE [dbo].[Advertisers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CreditCheck] nvarchar(max)  NOT NULL,
    [CreditLimit] decimal(18,0)  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [EOM] nvarchar(max)  NULL,
    [Suppression] nvarchar(max)  NULL,
    [StartingBalance] decimal(14,4)  NULL,
    [ContactName1] nvarchar(max)  NULL,
    [ContactRole1] nvarchar(max)  NULL,
    [ContactEmail1] nvarchar(max)  NULL,
    [ContactPhone1] nvarchar(max)  NULL,
    [ContactName2] nvarchar(max)  NULL,
    [ContactRole2] nvarchar(max)  NULL,
    [ContactEmail2] nvarchar(max)  NULL,
    [ContactPhone2] nvarchar(max)  NULL,
    [ContactName3] nvarchar(max)  NULL,
    [ContactRole3] nvarchar(max)  NULL,
    [ContactEmail3] nvarchar(max)  NULL,
    [ContactPhone3] nvarchar(max)  NULL,
    [LoginName1] nvarchar(max)  NULL,
    [URL1] nvarchar(max)  NULL,
    [UserName1] nvarchar(max)  NULL,
    [Password1] nvarchar(max)  NULL,
    [LoginName2] nvarchar(max)  NULL,
    [URL2] nvarchar(max)  NULL,
    [UserName2] nvarchar(max)  NULL,
    [Password2] nvarchar(max)  NULL,
    [LoginName3] nvarchar(max)  NULL,
    [URL3] nvarchar(max)  NULL,
    [UserName3] nvarchar(max)  NULL,
    [Password3] nvarchar(max)  NULL,
    [Term_Id] int  NOT NULL,
    [Currency_Id] int  NOT NULL
);
GO

-- Creating table 'Currencies'
CREATE TABLE [dbo].[Currencies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Culture] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Identity] nvarchar(max)  NULL
);
GO

-- Creating table 'Periods'
CREATE TABLE [dbo].[Periods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [BeginDate] datetime  NOT NULL
);
GO

-- Creating table 'SpendItems'
CREATE TABLE [dbo].[SpendItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Period] datetime  NOT NULL,
    [Advertiser] nvarchar(max)  NOT NULL,
    [PID] int  NOT NULL,
    [Currency] nvarchar(max)  NOT NULL,
    [Rate] decimal(14,4)  NOT NULL,
    [Volume] decimal(18,0)  NOT NULL,
    [UnitType] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UnitTypes'
CREATE TABLE [dbo].[UnitTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Invoices'
CREATE TABLE [dbo].[Invoices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TxnID] nvarchar(max)  NOT NULL,
    [TxnDate] datetime  NOT NULL,
    [RefNumber] nvarchar(max)  NOT NULL,
    [AppliedAmount] decimal(14,4)  NOT NULL,
    [BalanceRemaining] decimal(14,4)  NOT NULL,
    [AccountReceivableId] int  NOT NULL,
    [IsPending] bit  NOT NULL,
    [DueDate] datetime  NOT NULL,
    [Memo] nvarchar(max)  NOT NULL,
    [IsPaid] bit  NOT NULL,
    [LineType] nvarchar(50)  NOT NULL,
    [LineSeq] int  NOT NULL,
    [LineDesc] nvarchar(max)  NOT NULL,
    [LineClass] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [TxnID2] nvarchar(max)  NOT NULL,
    [Customer_Id] int  NOT NULL,
    [Term_Id] int  NOT NULL
);
GO

-- Creating table 'Terms'
CREATE TABLE [dbo].[Terms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Days] int  NOT NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [ListID] nvarchar(max)  NOT NULL,
    [QuickBooksCompany_Id] int  NOT NULL,
    [Advertiser_Id] int  NOT NULL,
    [Term_Id] int  NOT NULL
);
GO

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ReceivedPayments'
CREATE TABLE [dbo].[ReceivedPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TxnID] nvarchar(max)  NOT NULL,
    [TxnDate] datetime  NOT NULL,
    [TotalAmount] decimal(14,4)  NOT NULL,
    [Memo] nvarchar(max)  NULL,
    [AppliedToTxnTxnID] nvarchar(255)  NOT NULL,
    [AppliedToTxnTxnType] nvarchar(255)  NOT NULL,
    [AppliedToTxnAmount] decimal(14,4)  NOT NULL,
    [FQPrimaryKey] nvarchar(255)  NOT NULL,
    [RefNumber] nvarchar(255)  NULL,
    [ARAccount_Id] int  NOT NULL,
    [Customer_Id] int  NOT NULL
);
GO

-- Creating table 'AccountsReceivable'
CREATE TABLE [dbo].[AccountsReceivable] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Currency_Id] int  NOT NULL
);
GO

-- Creating table 'SynchTimes'
CREATE TABLE [dbo].[SynchTimes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [TimeStamp] datetime  NULL
);
GO

-- Creating table 'Notes'
CREATE TABLE [dbo].[Notes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [Data] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CampaignSpends'
CREATE TABLE [dbo].[CampaignSpends] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Rate] decimal(14,4)  NOT NULL,
    [Volume] decimal(18,0)  NOT NULL,
    [Currency_Id] int  NOT NULL,
    [Period_Id] int  NOT NULL,
    [UnitType_Id] int  NOT NULL,
    [Campaign_Id] int  NOT NULL
);
GO

-- Creating table 'Campaigns'
CREATE TABLE [dbo].[Campaigns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Pid] int  NOT NULL,
    [Advertiser_Id] int  NOT NULL,
    [Employee_Id] int  NOT NULL,
    [Employee1_Id] int  NOT NULL
);
GO

-- Creating table 'Rates'
CREATE TABLE [dbo].[Rates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ToUSD] decimal(14,4)  NOT NULL,
    [Currency_Id] int  NOT NULL,
    [Period_Id] int  NOT NULL
);
GO

-- Creating table 'vAdvertiserEmployees'
CREATE TABLE [dbo].[vAdvertiserEmployees] (
    [AdvertiserName] nvarchar(255)  NOT NULL,
    [AdvertiserId] int  NOT NULL,
    [AM] nvarchar(50)  NOT NULL,
    [AD] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'EntryTypes'
CREATE TABLE [dbo].[EntryTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Logs'
CREATE TABLE [dbo].[Logs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Entry] nvarchar(max)  NOT NULL,
    [Timestamp] nvarchar(max)  NULL,
    [EntryType_Id] int  NOT NULL
);
GO

-- Creating table 'CreditMemoes'
CREATE TABLE [dbo].[CreditMemoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TxnID] nvarchar(max)  NOT NULL,
    [TxnDate] datetime  NOT NULL,
    [TxnType] nvarchar(max)  NOT NULL,
    [Amount] decimal(14,4)  NOT NULL,
    [Customer_Id] int  NOT NULL,
    [AccountReceivable_Id] int  NOT NULL
);
GO

-- Creating table 'QuickBooksTables'
CREATE TABLE [dbo].[QuickBooksTables] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QUALIFIERNAME] nvarchar(max)  NOT NULL,
    [TABLENAME] nvarchar(max)  NOT NULL,
    [TYPENAME] nvarchar(max)  NOT NULL,
    [REMARKS] nvarchar(max)  NOT NULL,
    [DELETEABLE] int  NOT NULL,
    [VOIDABLE] int  NOT NULL,
    [INSERT_ONLY] int  NOT NULL,
    [BestRowID] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'QuickBooksColumns'
CREATE TABLE [dbo].[QuickBooksColumns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QUALIFIERNAME] nvarchar(max)  NOT NULL,
    [TABLENAME] nvarchar(max)  NOT NULL,
    [COLUMNNAME] nvarchar(max)  NOT NULL,
    [TYPE] nvarchar(max)  NOT NULL,
    [TYPENAME] nvarchar(max)  NOT NULL,
    [PRECISION] nvarchar(max)  NOT NULL,
    [LENGTH] nvarchar(max)  NOT NULL,
    [SCALE] nvarchar(max)  NOT NULL,
    [NULLABLE] nvarchar(max)  NOT NULL,
    [DEFAULT] nvarchar(max)  NOT NULL,
    [DATATYPE] nvarchar(max)  NOT NULL,
    [OCTET_LENGTH] nvarchar(max)  NULL,
    [ORDINAL_POSITION] nvarchar(max)  NOT NULL,
    [IS_NULLABLE] nvarchar(max)  NULL,
    [QUERYABLE] nvarchar(max)  NOT NULL,
    [UPDATEABLE] nvarchar(max)  NOT NULL,
    [INSERTABLE] nvarchar(max)  NOT NULL,
    [REQUIRED_ON_INSERT] nvarchar(max)  NOT NULL,
    [FORMAT] nvarchar(max)  NOT NULL,
    [RELATES_TO] nvarchar(max)  NULL,
    [JUMPIN_TYPE] nvarchar(max)  NULL,
    [FORCE_UNOPTIMIZED] nvarchar(max)  NOT NULL,
    [ADVANCED] nvarchar(max)  NOT NULL,
    [SDK_QB_NAME] nvarchar(max)  NULL,
    [COLUMNFULLNAME] nvarchar(max)  NOT NULL,
    [COLUMNSHORTNAME] nvarchar(max)  NOT NULL,
    [QuickBooksTable_Id] int  NOT NULL
);
GO

-- Creating table 'BudgetReportRows'
CREATE TABLE [dbo].[BudgetReportRows] (
    [AdvertiserId] int  NOT NULL,
    [Total] decimal(14,4)  NOT NULL,
    [CurrencyName] nvarchar(3)  NOT NULL,
    [StartingBalance] decimal(14,4)  NOT NULL,
    [Payments] decimal(14,4)  NOT NULL,
    [InvoiceCredits] decimal(14,4)  NOT NULL,
    [Spends] decimal(14,4)  NOT NULL,
    [CheckCredits] decimal(14,4)  NOT NULL
);
GO

-- Creating table 'Tabs'
CREATE TABLE [dbo].[Tabs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Location] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TabGroups'
CREATE TABLE [dbo].[TabGroups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TabTabGroup'
CREATE TABLE [dbo].[TabTabGroup] (
    [Tabs_Id] int  NOT NULL,
    [TabGroups_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Advertisers'
ALTER TABLE [dbo].[Advertisers]
ADD CONSTRAINT [PK_Advertisers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Currencies'
ALTER TABLE [dbo].[Currencies]
ADD CONSTRAINT [PK_Currencies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Periods'
ALTER TABLE [dbo].[Periods]
ADD CONSTRAINT [PK_Periods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SpendItems'
ALTER TABLE [dbo].[SpendItems]
ADD CONSTRAINT [PK_SpendItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UnitTypes'
ALTER TABLE [dbo].[UnitTypes]
ADD CONSTRAINT [PK_UnitTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [PK_Invoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Terms'
ALTER TABLE [dbo].[Terms]
ADD CONSTRAINT [PK_Terms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReceivedPayments'
ALTER TABLE [dbo].[ReceivedPayments]
ADD CONSTRAINT [PK_ReceivedPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AccountsReceivable'
ALTER TABLE [dbo].[AccountsReceivable]
ADD CONSTRAINT [PK_AccountsReceivable]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SynchTimes'
ALTER TABLE [dbo].[SynchTimes]
ADD CONSTRAINT [PK_SynchTimes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Notes'
ALTER TABLE [dbo].[Notes]
ADD CONSTRAINT [PK_Notes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CampaignSpends'
ALTER TABLE [dbo].[CampaignSpends]
ADD CONSTRAINT [PK_CampaignSpends]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [PK_Campaigns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Rates'
ALTER TABLE [dbo].[Rates]
ADD CONSTRAINT [PK_Rates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AdvertiserName], [AdvertiserId], [AM], [AD] in table 'vAdvertiserEmployees'
ALTER TABLE [dbo].[vAdvertiserEmployees]
ADD CONSTRAINT [PK_vAdvertiserEmployees]
    PRIMARY KEY CLUSTERED ([AdvertiserName], [AdvertiserId], [AM], [AD] ASC);
GO

-- Creating primary key on [Id] in table 'EntryTypes'
ALTER TABLE [dbo].[EntryTypes]
ADD CONSTRAINT [PK_EntryTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [PK_Logs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CreditMemoes'
ALTER TABLE [dbo].[CreditMemoes]
ADD CONSTRAINT [PK_CreditMemoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuickBooksTables'
ALTER TABLE [dbo].[QuickBooksTables]
ADD CONSTRAINT [PK_QuickBooksTables]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuickBooksColumns'
ALTER TABLE [dbo].[QuickBooksColumns]
ADD CONSTRAINT [PK_QuickBooksColumns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AdvertiserId] in table 'BudgetReportRows'
ALTER TABLE [dbo].[BudgetReportRows]
ADD CONSTRAINT [PK_BudgetReportRows]
    PRIMARY KEY CLUSTERED ([AdvertiserId] ASC);
GO

-- Creating primary key on [Id] in table 'Tabs'
ALTER TABLE [dbo].[Tabs]
ADD CONSTRAINT [PK_Tabs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TabGroups'
ALTER TABLE [dbo].[TabGroups]
ADD CONSTRAINT [PK_TabGroups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Tabs_Id], [TabGroups_Id] in table 'TabTabGroup'
ALTER TABLE [dbo].[TabTabGroup]
ADD CONSTRAINT [PK_TabTabGroup]
    PRIMARY KEY NONCLUSTERED ([Tabs_Id], [TabGroups_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ARAccount_Id] in table 'ReceivedPayments'
ALTER TABLE [dbo].[ReceivedPayments]
ADD CONSTRAINT [FK_ARAccountReceivedPayment]
    FOREIGN KEY ([ARAccount_Id])
    REFERENCES [dbo].[AccountsReceivable]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ARAccountReceivedPayment'
CREATE INDEX [IX_FK_ARAccountReceivedPayment]
ON [dbo].[ReceivedPayments]
    ([ARAccount_Id]);
GO

-- Creating foreign key on [Currency_Id] in table 'AccountsReceivable'
ALTER TABLE [dbo].[AccountsReceivable]
ADD CONSTRAINT [FK_CurrencyARAccount]
    FOREIGN KEY ([Currency_Id])
    REFERENCES [dbo].[Currencies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CurrencyARAccount'
CREATE INDEX [IX_FK_CurrencyARAccount]
ON [dbo].[AccountsReceivable]
    ([Currency_Id]);
GO

-- Creating foreign key on [QuickBooksCompany_Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_QuickBooksCompanyCustomer]
    FOREIGN KEY ([QuickBooksCompany_Id])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuickBooksCompanyCustomer'
CREATE INDEX [IX_FK_QuickBooksCompanyCustomer]
ON [dbo].[Customers]
    ([QuickBooksCompany_Id]);
GO

-- Creating foreign key on [Customer_Id] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [FK_CustomerInvoice]
    FOREIGN KEY ([Customer_Id])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerInvoice'
CREATE INDEX [IX_FK_CustomerInvoice]
ON [dbo].[Invoices]
    ([Customer_Id]);
GO

-- Creating foreign key on [Term_Id] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [FK_TermsInvoice]
    FOREIGN KEY ([Term_Id])
    REFERENCES [dbo].[Terms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TermsInvoice'
CREATE INDEX [IX_FK_TermsInvoice]
ON [dbo].[Invoices]
    ([Term_Id]);
GO

-- Creating foreign key on [Advertiser_Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_AdvertiserCustomer]
    FOREIGN KEY ([Advertiser_Id])
    REFERENCES [dbo].[Advertisers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertiserCustomer'
CREATE INDEX [IX_FK_AdvertiserCustomer]
ON [dbo].[Customers]
    ([Advertiser_Id]);
GO

-- Creating foreign key on [Customer_Id] in table 'ReceivedPayments'
ALTER TABLE [dbo].[ReceivedPayments]
ADD CONSTRAINT [FK_CustomerReceivedPayment]
    FOREIGN KEY ([Customer_Id])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerReceivedPayment'
CREATE INDEX [IX_FK_CustomerReceivedPayment]
ON [dbo].[ReceivedPayments]
    ([Customer_Id]);
GO

-- Creating foreign key on [AccountReceivableId] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [FK_AccountReceivableInvoice]
    FOREIGN KEY ([AccountReceivableId])
    REFERENCES [dbo].[AccountsReceivable]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountReceivableInvoice'
CREATE INDEX [IX_FK_AccountReceivableInvoice]
ON [dbo].[Invoices]
    ([AccountReceivableId]);
GO

-- Creating foreign key on [Currency_Id] in table 'CampaignSpends'
ALTER TABLE [dbo].[CampaignSpends]
ADD CONSTRAINT [FK_CurrencyCampaignSpend]
    FOREIGN KEY ([Currency_Id])
    REFERENCES [dbo].[Currencies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CurrencyCampaignSpend'
CREATE INDEX [IX_FK_CurrencyCampaignSpend]
ON [dbo].[CampaignSpends]
    ([Currency_Id]);
GO

-- Creating foreign key on [Period_Id] in table 'CampaignSpends'
ALTER TABLE [dbo].[CampaignSpends]
ADD CONSTRAINT [FK_PeriodCampaignSpend]
    FOREIGN KEY ([Period_Id])
    REFERENCES [dbo].[Periods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PeriodCampaignSpend'
CREATE INDEX [IX_FK_PeriodCampaignSpend]
ON [dbo].[CampaignSpends]
    ([Period_Id]);
GO

-- Creating foreign key on [UnitType_Id] in table 'CampaignSpends'
ALTER TABLE [dbo].[CampaignSpends]
ADD CONSTRAINT [FK_UnitTypeCampaignSpend]
    FOREIGN KEY ([UnitType_Id])
    REFERENCES [dbo].[UnitTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitTypeCampaignSpend'
CREATE INDEX [IX_FK_UnitTypeCampaignSpend]
ON [dbo].[CampaignSpends]
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

-- Creating foreign key on [Campaign_Id] in table 'CampaignSpends'
ALTER TABLE [dbo].[CampaignSpends]
ADD CONSTRAINT [FK_CampaignCampaignSpend]
    FOREIGN KEY ([Campaign_Id])
    REFERENCES [dbo].[Campaigns]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CampaignCampaignSpend'
CREATE INDEX [IX_FK_CampaignCampaignSpend]
ON [dbo].[CampaignSpends]
    ([Campaign_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [FK_EmployeeCampaign]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeCampaign'
CREATE INDEX [IX_FK_EmployeeCampaign]
ON [dbo].[Campaigns]
    ([Employee_Id]);
GO

-- Creating foreign key on [Employee1_Id] in table 'Campaigns'
ALTER TABLE [dbo].[Campaigns]
ADD CONSTRAINT [FK_EmployeeCampaign1]
    FOREIGN KEY ([Employee1_Id])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeCampaign1'
CREATE INDEX [IX_FK_EmployeeCampaign1]
ON [dbo].[Campaigns]
    ([Employee1_Id]);
GO

-- Creating foreign key on [Currency_Id] in table 'Rates'
ALTER TABLE [dbo].[Rates]
ADD CONSTRAINT [FK_CurrencyRate]
    FOREIGN KEY ([Currency_Id])
    REFERENCES [dbo].[Currencies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CurrencyRate'
CREATE INDEX [IX_FK_CurrencyRate]
ON [dbo].[Rates]
    ([Currency_Id]);
GO

-- Creating foreign key on [Period_Id] in table 'Rates'
ALTER TABLE [dbo].[Rates]
ADD CONSTRAINT [FK_PeriodRate]
    FOREIGN KEY ([Period_Id])
    REFERENCES [dbo].[Periods]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PeriodRate'
CREATE INDEX [IX_FK_PeriodRate]
ON [dbo].[Rates]
    ([Period_Id]);
GO

-- Creating foreign key on [Term_Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_TermsCustomer]
    FOREIGN KEY ([Term_Id])
    REFERENCES [dbo].[Terms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TermsCustomer'
CREATE INDEX [IX_FK_TermsCustomer]
ON [dbo].[Customers]
    ([Term_Id]);
GO

-- Creating foreign key on [Term_Id] in table 'Advertisers'
ALTER TABLE [dbo].[Advertisers]
ADD CONSTRAINT [FK_AdvertiserTerms]
    FOREIGN KEY ([Term_Id])
    REFERENCES [dbo].[Terms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertiserTerms'
CREATE INDEX [IX_FK_AdvertiserTerms]
ON [dbo].[Advertisers]
    ([Term_Id]);
GO

-- Creating foreign key on [Currency_Id] in table 'Advertisers'
ALTER TABLE [dbo].[Advertisers]
ADD CONSTRAINT [FK_AdvertiserCurrency]
    FOREIGN KEY ([Currency_Id])
    REFERENCES [dbo].[Currencies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AdvertiserCurrency'
CREATE INDEX [IX_FK_AdvertiserCurrency]
ON [dbo].[Advertisers]
    ([Currency_Id]);
GO

-- Creating foreign key on [EntryType_Id] in table 'Logs'
ALTER TABLE [dbo].[Logs]
ADD CONSTRAINT [FK_LogEntryType]
    FOREIGN KEY ([EntryType_Id])
    REFERENCES [dbo].[EntryTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LogEntryType'
CREATE INDEX [IX_FK_LogEntryType]
ON [dbo].[Logs]
    ([EntryType_Id]);
GO

-- Creating foreign key on [QuickBooksTable_Id] in table 'QuickBooksColumns'
ALTER TABLE [dbo].[QuickBooksColumns]
ADD CONSTRAINT [FK_QuickBooksTableQuickBooksColumn]
    FOREIGN KEY ([QuickBooksTable_Id])
    REFERENCES [dbo].[QuickBooksTables]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuickBooksTableQuickBooksColumn'
CREATE INDEX [IX_FK_QuickBooksTableQuickBooksColumn]
ON [dbo].[QuickBooksColumns]
    ([QuickBooksTable_Id]);
GO

-- Creating foreign key on [Customer_Id] in table 'CreditMemoes'
ALTER TABLE [dbo].[CreditMemoes]
ADD CONSTRAINT [FK_CustomerCreditMemo]
    FOREIGN KEY ([Customer_Id])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCreditMemo'
CREATE INDEX [IX_FK_CustomerCreditMemo]
ON [dbo].[CreditMemoes]
    ([Customer_Id]);
GO

-- Creating foreign key on [AccountReceivable_Id] in table 'CreditMemoes'
ALTER TABLE [dbo].[CreditMemoes]
ADD CONSTRAINT [FK_AccountReceivableCreditMemo]
    FOREIGN KEY ([AccountReceivable_Id])
    REFERENCES [dbo].[AccountsReceivable]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountReceivableCreditMemo'
CREATE INDEX [IX_FK_AccountReceivableCreditMemo]
ON [dbo].[CreditMemoes]
    ([AccountReceivable_Id]);
GO

-- Creating foreign key on [Tabs_Id] in table 'TabTabGroup'
ALTER TABLE [dbo].[TabTabGroup]
ADD CONSTRAINT [FK_TabTabGroup_Tab]
    FOREIGN KEY ([Tabs_Id])
    REFERENCES [dbo].[Tabs]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [TabGroups_Id] in table 'TabTabGroup'
ALTER TABLE [dbo].[TabTabGroup]
ADD CONSTRAINT [FK_TabTabGroup_TabGroup]
    FOREIGN KEY ([TabGroups_Id])
    REFERENCES [dbo].[TabGroups]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TabTabGroup_TabGroup'
CREATE INDEX [IX_FK_TabTabGroup_TabGroup]
ON [dbo].[TabTabGroup]
    ([TabGroups_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------