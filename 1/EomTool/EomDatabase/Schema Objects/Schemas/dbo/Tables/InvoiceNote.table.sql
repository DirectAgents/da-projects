CREATE TABLE [dbo].[InvoiceNote] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [invoice_id] INT           NOT NULL,
    [note]       VARCHAR (MAX) NOT NULL,
    [added_by]   VARCHAR (255) NULL,
    [created]    DATETIME      NOT NULL,
    CONSTRAINT [PK_InvoiceNote] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_InvoiceNote_Invoice] FOREIGN KEY ([invoice_id]) REFERENCES [dbo].[Invoice] ([id])
);

