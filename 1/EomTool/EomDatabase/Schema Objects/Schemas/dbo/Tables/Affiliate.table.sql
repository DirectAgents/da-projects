CREATE TABLE [dbo].[Affiliate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[media_buyer_id] [int] NOT NULL,
	[affid] [int] NOT NULL,
	[currency_id] [int] NOT NULL,
	[email] [varchar](100) NOT NULL,
	[add_code] [varchar](100) NOT NULL,
	[net_term_type_id] [int] NULL,
	[payment_method_id] [int] NOT NULL,
	[name2]  AS ((([name]+' (')+[add_code])+')'),
	[date_created] [datetime] NULL,
	[date_modified] [datetime] NULL,
	[tracking_system_id] [int] NULL,
	[external_id] [int] NULL
);