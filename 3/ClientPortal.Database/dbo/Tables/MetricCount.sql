CREATE TABLE [dbo].[MetricCount] (
    [offer_id]         INT      NOT NULL,
    [date]             DATETIME NOT NULL,
    [conversions_only] BIT      NOT NULL,
    [metricvalue_id]   INT      NOT NULL,
    [count]            INT      CONSTRAINT [DF_MetricCount_count] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MetricCount] PRIMARY KEY CLUSTERED ([offer_id] ASC, [date] ASC, [conversions_only] ASC, [metricvalue_id] ASC),
    CONSTRAINT [FK_MetricCount_MetricValue] FOREIGN KEY ([metricvalue_id]) REFERENCES [dbo].[MetricValue] ([id])
);

