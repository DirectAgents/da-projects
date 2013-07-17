CREATE TABLE [dbo].[DimCountry] (
    [CountryKey]  INT           IDENTITY (1, 1) NOT NULL,
    [CountryCode] NVARCHAR (10) NOT NULL,
    CONSTRAINT [PK_DimCountry] PRIMARY KEY CLUSTERED ([CountryKey] ASC)
);

