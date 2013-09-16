CREATE TABLE [dbo].[Offer] (
    [Offer_Id]           INT            NOT NULL,
    [OfferName]          NVARCHAR (255) NULL,
    [Advertiser_Id]      INT            NULL,
    [DefaultPriceFormat] NVARCHAR (255) NULL,
    [Currency]           NCHAR (3)      NULL,
    CONSTRAINT [PK_Offer] PRIMARY KEY CLUSTERED ([Offer_Id] ASC)
);



