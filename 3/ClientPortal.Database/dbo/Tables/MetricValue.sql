CREATE TABLE [dbo].[MetricValue] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [metric_id] INT           NOT NULL,
    [name]      NVARCHAR (50) NOT NULL,
    [code]      NVARCHAR (50) NULL,
    CONSTRAINT [PK_MetricValue] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_MetricValue_Metric] FOREIGN KEY ([metric_id]) REFERENCES [dbo].[Metric] ([id])
);

