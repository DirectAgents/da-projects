CREATE TABLE [dbo].[Invoice] (
    [id]                INT IDENTITY (1, 1) NOT NULL,
    [invoice_status_id] INT NOT NULL,
    CONSTRAINT [PK_Invoice_1] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Invoice_InvoiceStatus] FOREIGN KEY ([invoice_status_id]) REFERENCES [dbo].[InvoiceStatus] ([id])
);

