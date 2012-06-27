CREATE TABLE [dbo].[Item] (
    [id]                        INT             IDENTITY (1, 1) NOT NULL,
    [name]                      VARCHAR (300)   NULL,
    [pid]                       INT             NOT NULL,
    [affid]                     INT             NOT NULL,
    [source_id]                 INT             NOT NULL,
    [unit_type_id]              INT             NOT NULL,
    [stat_id_n]                 INT             NULL,
    [revenue_currency_id]       INT             NOT NULL,
    [cost_currency_id]          INT             NOT NULL,
    [revenue_per_unit]          MONEY           NOT NULL,
    [cost_per_unit]             MONEY           NOT NULL,
    [num_units]                 DECIMAL (16, 6) NOT NULL,
    [notes]                     NVARCHAR (MAX)  NOT NULL,
    [accounting_notes]          NVARCHAR (255)  NOT NULL,
    [item_accounting_status_id] INT             NOT NULL,
    [item_reporting_status_id]  INT             NOT NULL,
    [total_revenue]             AS              ([num_units]*[revenue_per_unit]),
    [total_cost]                AS              ([num_units]*[cost_per_unit]),
    [margin]                    AS              ([dbo].[tousd3]([revenue_currency_id],[num_units]*[revenue_per_unit])-[dbo].[tousd3]([cost_currency_id],[num_units]*[cost_per_unit])),
	[modified]					AS				(GETDATE())
);

