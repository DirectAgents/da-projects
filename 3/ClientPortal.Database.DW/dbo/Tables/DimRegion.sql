CREATE TABLE [dbo].[DimRegion] (
    [RegionKey]  INT           IDENTITY (1, 1) NOT NULL,
    [RegionCode] NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_DimRegion] PRIMARY KEY CLUSTERED ([RegionKey] ASC)
);

