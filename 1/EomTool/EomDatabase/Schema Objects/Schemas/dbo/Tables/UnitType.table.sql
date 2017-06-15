CREATE TABLE [dbo].[UnitType] (
    [id]             INT          IDENTITY (1, 1) NOT NULL,
    [name]           VARCHAR (50) NOT NULL,
    [income_type_id] INT          NULL,
    CONSTRAINT [UnitType_id_PK] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_UnitType_IncomeType] FOREIGN KEY ([income_type_id]) REFERENCES [dbo].[IncomeType] ([id]),
    CONSTRAINT [UnitType_name_UK] UNIQUE NONCLUSTERED ([name] ASC)
);



