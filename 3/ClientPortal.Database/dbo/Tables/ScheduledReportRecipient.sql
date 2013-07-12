CREATE TABLE [dbo].[ScheduledReportRecipient] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [EmailAddress]      NVARCHAR (MAX) NOT NULL,
    [ScheduledReportId] INT            NOT NULL,
    CONSTRAINT [PK_ScheduledReportRecipients] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScheduledReportScheduledReportRecipient] FOREIGN KEY ([ScheduledReportId]) REFERENCES [dbo].[ScheduledReport] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ScheduledReportScheduledReportRecipient]
    ON [dbo].[ScheduledReportRecipient]([ScheduledReportId] ASC);

