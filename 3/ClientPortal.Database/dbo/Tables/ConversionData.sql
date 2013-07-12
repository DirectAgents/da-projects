CREATE TABLE [dbo].[ConversionData] (
    [conversion_id] NVARCHAR (128)  NOT NULL,
    [value0]        DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_dbo.ConversionRevenue] PRIMARY KEY CLUSTERED ([conversion_id] ASC)
);

