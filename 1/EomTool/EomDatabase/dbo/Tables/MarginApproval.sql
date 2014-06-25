CREATE TABLE [dbo].[MarginApproval] (
    [id]                  INT            IDENTITY (1, 1) NOT NULL,
    [pid]                 INT            NULL,
    [affid]               INT            NULL,
    [revenue_currency_id] INT            NULL,
    [total_revenue]       MONEY          NULL,
    [cost_currency_id]    INT            NULL,
    [total_cost]          MONEY          NULL,
    [margin_pct]          DECIMAL (8, 4) NULL,
    [comment]             VARCHAR (MAX)  NULL,
    [added_by]            VARCHAR (255)  NULL,
    [created]             DATETIME       NOT NULL,
    [used]                DATETIME       NULL,
    CONSTRAINT [PK_MarginApproval] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_MarginApproval_Affiliate] FOREIGN KEY ([affid]) REFERENCES [dbo].[Affiliate] ([affid]),
    CONSTRAINT [FK_MarginApproval_Campaign] FOREIGN KEY ([pid]) REFERENCES [dbo].[Campaign] ([pid]),
    CONSTRAINT [FK_MarginApproval_Currency] FOREIGN KEY ([revenue_currency_id]) REFERENCES [dbo].[Currency] ([id]),
    CONSTRAINT [FK_MarginApproval_Currency1] FOREIGN KEY ([cost_currency_id]) REFERENCES [dbo].[Currency] ([id])
);

