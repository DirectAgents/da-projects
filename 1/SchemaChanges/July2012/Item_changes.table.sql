USE [DADatabaseJune2012]
GO

/****** Object:  Table [dbo].[Item_changes]    Script Date: 07/17/2012 14:25:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Item_changes](
	[id] [int] NOT NULL,
	[timestamp]  AS (getdate()),
	[name] [varchar](300) NULL,
	[d_name] [varchar](300) NULL,
	[pid] [int] NOT NULL,
	[d_pid] [int] NOT NULL,
	[affid] [int] NOT NULL,
	[d_affid] [int] NOT NULL,
	[source_id] [int] NOT NULL,
	[d_source_id] [int] NOT NULL,
	[unit_type_id] [int] NOT NULL,
	[d_unit_type_id] [int] NOT NULL,
	[stat_id_n] [int] NULL,
	[d_stat_id_n] [int] NULL,
	[revenue_currency_id] [int] NOT NULL,
	[d_revenue_currency_id] [int] NOT NULL,
	[cost_currency_id] [int] NOT NULL,
	[d_cost_currency_id] [int] NOT NULL,
	[revenue_per_unit] [money] NOT NULL,
	[d_revenue_per_unit] [money] NOT NULL,
	[cost_per_unit] [money] NOT NULL,
	[d_cost_per_unit] [money] NOT NULL,
	[num_units] [decimal](16, 6) NOT NULL,
	[d_num_units] [decimal](16, 6) NOT NULL,
	[notes] [nvarchar](max) NOT NULL,
	[d_notes] [nvarchar](max) NOT NULL,
	[accounting_notes] [nvarchar](255) NOT NULL,
	[d_accounting_notes] [nvarchar](255) NOT NULL,
	[item_accounting_status_id] [int] NOT NULL,
	[d_item_accounting_status_id] [int] NOT NULL,
	[item_reporting_status_id] [int] NOT NULL,
	[d_item_reporting_status_id] [int] NOT NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

