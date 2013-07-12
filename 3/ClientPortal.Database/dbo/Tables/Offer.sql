CREATE TABLE [dbo].[Offer] (
    [Offer_Id]              INT            NOT NULL,
    [OfferName]             NVARCHAR (255) NULL,
    [Advertiser_Id]         INT            NULL,
    [VerticalName]          NVARCHAR (255) NULL,
    [OfferType]             NVARCHAR (255) NULL,
    [StatusName]            NVARCHAR (255) NULL,
    [DefaultPriceFormat]    NVARCHAR (255) NULL,
    [DefaultPayout]         NVARCHAR (255) NULL,
    [PriceReceived]         NVARCHAR (255) NULL,
    [Secure]                NVARCHAR (255) NULL,
    [OfferLink]             NVARCHAR (255) NULL,
    [ThumbnailImageUrl]     NVARCHAR (255) NULL,
    [ExpirationDate]        NVARCHAR (255) NULL,
    [CookieDays]            NVARCHAR (255) NULL,
    [CookieDaysImpressions] NVARCHAR (255) NULL,
    [DateCreated]           NVARCHAR (255) NULL,
    [Currency]              NCHAR (3)      NULL,
    [AllowedCountries]      NVARCHAR (MAX) NULL,
    [Xml]                   XML            NULL,
    [AllowedMediaTypeNames] NVARCHAR (MAX) CONSTRAINT [DF_Offer_AllowedMediaTypeNames] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Offer] PRIMARY KEY CLUSTERED ([Offer_Id] ASC)
);

