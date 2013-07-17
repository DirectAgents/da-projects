CREATE TABLE [dbo].[DimAffiliate] (
    [AffiliateKey]  INT            NOT NULL,
    [AffiliateName] NVARCHAR (250) NULL,
    CONSTRAINT [PK_DimAffiliate] PRIMARY KEY CLUSTERED ([AffiliateKey] ASC)
);

