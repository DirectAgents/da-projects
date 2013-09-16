CREATE TABLE [dbo].[EmailedReport] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [EmailTo]            NVARCHAR (MAX) NOT NULL,
    [EmailStatus]        INT            NOT NULL,
    [EmailStatusUpdated] DATETIME       NULL,
    [GeneratedReportId]  INT            NOT NULL,
    CONSTRAINT [PK_EmailedReports] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GeneratedReportEmailedReport] FOREIGN KEY ([GeneratedReportId]) REFERENCES [dbo].[GeneratedReport] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_GeneratedReportEmailedReport]
    ON [dbo].[EmailedReport]([GeneratedReportId] ASC);

