CREATE TABLE [dbo].[IncomeType] (
    [id]      INT           NOT NULL,
    [name]    VARCHAR (100) NOT NULL,
    [qb_code] VARCHAR (100) NULL,
    CONSTRAINT [PK_IncomeType] PRIMARY KEY CLUSTERED ([id] ASC)
);

