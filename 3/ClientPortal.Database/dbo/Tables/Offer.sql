CREATE TABLE [dbo].[Offer] (
    [OfferId]            INT             NOT NULL,
    [OfferName]          NVARCHAR (255)  NULL,
    [AdvertiserId]       INT             NULL,
    [DefaultPriceFormat] NVARCHAR (255)  NULL,
    [Currency]           NCHAR (3)       NULL,
    [Logo]               VARBINARY (MAX) NULL,
    CONSTRAINT [PK_Offer] PRIMARY KEY CLUSTERED ([OfferId] ASC)
);





