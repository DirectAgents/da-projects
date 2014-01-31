CREATE TABLE [dbo].[CPMReport] (
    [CPMReportId] INT            IDENTITY (1, 1) NOT NULL,
    [OfferId]     INT            NOT NULL,
    [Name]        NVARCHAR (255) NULL,
    [DateSent]    DATETIME       NULL,
    [Recipient]   NVARCHAR (255) NULL,
    [RecipientCC] NVARCHAR (255) NULL,
    [Summary]     NVARCHAR (MAX) NULL,
    [Conclusion]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_CPMReport] PRIMARY KEY CLUSTERED ([CPMReportId] ASC),
    CONSTRAINT [FK_CPMReport_Offer] FOREIGN KEY ([OfferId]) REFERENCES [dbo].[Offer] ([OfferId])
);

