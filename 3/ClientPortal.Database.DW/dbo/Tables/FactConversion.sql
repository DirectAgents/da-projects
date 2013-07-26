CREATE TABLE [dbo].[FactConversion] (
    [ConversionKey] INT      NOT NULL,
    [ClickKey]      INT      NULL,
    [DateKey]       DATETIME NOT NULL,
    [ClickDateKey]  DATETIME NULL,
    CONSTRAINT [PK_FactConversion_1] PRIMARY KEY CLUSTERED ([ConversionKey] ASC),
    CONSTRAINT [FK_FactConversion_DimDate] FOREIGN KEY ([DateKey]) REFERENCES [dbo].[DimDate] ([PK_Date]),
    CONSTRAINT [FK_FactConversion_DimDateClick] FOREIGN KEY ([ClickDateKey]) REFERENCES [dbo].[DimDate] ([PK_Date]),
    CONSTRAINT [FK_FactConversion_FactClick] FOREIGN KEY ([ClickKey]) REFERENCES [dbo].[FactClick] ([ClickKey])
);



