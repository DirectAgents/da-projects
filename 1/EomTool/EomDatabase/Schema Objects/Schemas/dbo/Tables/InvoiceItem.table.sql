CREATE TABLE [dbo].[InvoiceItem] (
    [id]              INT   IDENTITY (1, 1) NOT NULL,
    [invoice_id]      INT   NOT NULL,
    [pid]             INT   NULL,
    [affid]           INT   NULL,
    [currency_id]     INT   NOT NULL,
    [amount_per_unit] MONEY NOT NULL,
    [num_units]       INT   NOT NULL,
    [total_amount]    AS    ([amount_per_unit]*[num_units]),
    [unit_type_id]    INT   NULL,
    CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_InvoiceItem_Affiliate] FOREIGN KEY ([affid]) REFERENCES [dbo].[Affiliate] ([affid]),
    CONSTRAINT [FK_InvoiceItem_Campaign] FOREIGN KEY ([pid]) REFERENCES [dbo].[Campaign] ([pid]),
    CONSTRAINT [FK_InvoiceItem_Currency] FOREIGN KEY ([currency_id]) REFERENCES [dbo].[Currency] ([id]),
    CONSTRAINT [FK_InvoiceItem_Invoice] FOREIGN KEY ([invoice_id]) REFERENCES [dbo].[Invoice] ([id]),
    CONSTRAINT [FK_InvoiceItem_UnitType] FOREIGN KEY ([unit_type_id]) REFERENCES [dbo].[UnitType] ([id])
);

