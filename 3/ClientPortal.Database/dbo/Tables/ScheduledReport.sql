CREATE TABLE [dbo].[ScheduledReport] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [AdvertiserId] INT      NOT NULL,
    [ReportType]   INT      NOT NULL,
    [Months]       INT      NOT NULL,
    [Days]         INT      NOT NULL,
    [IsCumulative] BIT      NOT NULL,
    [LastSent]     DATETIME NULL,
    [NextSend]     DATETIME NULL,
    CONSTRAINT [PK_ScheduledReports] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AdvertiserScheduledReport] FOREIGN KEY ([AdvertiserId]) REFERENCES [dbo].[Advertiser] ([AdvertiserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AdvertiserScheduledReport]
    ON [dbo].[ScheduledReport]([AdvertiserId] ASC);

