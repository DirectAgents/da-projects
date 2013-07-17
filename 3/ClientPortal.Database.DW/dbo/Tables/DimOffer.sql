CREATE TABLE [dbo].[DimOffer] (
    [OfferKey]  INT            NOT NULL,
    [OfferName] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_DimOffer] PRIMARY KEY CLUSTERED ([OfferKey] ASC)
);

