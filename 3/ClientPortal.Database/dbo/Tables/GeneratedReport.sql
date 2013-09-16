CREATE TABLE [dbo].[GeneratedReport] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [ReportType]          INT            NOT NULL,
    [StartDate]           DATETIME       NOT NULL,
    [EndDate]             DATETIME       NOT NULL,
    [ReportStatus]        INT            NOT NULL,
    [ReportStatusUpdated] DATETIME       NULL,
    [Content]             NVARCHAR (MAX) NOT NULL,
    [ScheduledReportId]   INT            NOT NULL,
    CONSTRAINT [PK_GeneratedReports] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScheduledReportGeneratedReport] FOREIGN KEY ([ScheduledReportId]) REFERENCES [dbo].[ScheduledReport] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ScheduledReportGeneratedReport]
    ON [dbo].[GeneratedReport]([ScheduledReportId] ASC);

