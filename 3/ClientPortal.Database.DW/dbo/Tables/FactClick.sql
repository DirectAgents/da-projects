CREATE TABLE [dbo].[FactClick] (
    [ClickKey]      INT      NOT NULL,
    [RegionKey]     INT      NOT NULL,
    [CountryKey]    INT      NOT NULL,
    [DateKey]       DATETIME NOT NULL,
    [AdvertiserKey] INT      NOT NULL,
    [OfferKey]      INT      NOT NULL,
    [AffiliateKey]  INT      NOT NULL,
    [BrowserKey]    INT      NOT NULL,
    [DeviceKey]     INT      NOT NULL,
    CONSTRAINT [PK_FactClick] PRIMARY KEY CLUSTERED ([ClickKey] ASC),
    CONSTRAINT [FK_FactClick_DimAdvertiser] FOREIGN KEY ([AdvertiserKey]) REFERENCES [dbo].[DimAdvertiser] ([AdvertiserKey]),
    CONSTRAINT [FK_FactClick_DimAffiliate] FOREIGN KEY ([AffiliateKey]) REFERENCES [dbo].[DimAffiliate] ([AffiliateKey]),
    CONSTRAINT [FK_FactClick_DimBrowser] FOREIGN KEY ([BrowserKey]) REFERENCES [dbo].[DimBrowser] ([BrowserKey]),
    CONSTRAINT [FK_FactClick_DimCountry] FOREIGN KEY ([CountryKey]) REFERENCES [dbo].[DimCountry] ([CountryKey]),
    CONSTRAINT [FK_FactClick_DimDate] FOREIGN KEY ([DateKey]) REFERENCES [dbo].[DimDate] ([PK_Date]),
    CONSTRAINT [FK_FactClick_DimDevice] FOREIGN KEY ([DeviceKey]) REFERENCES [dbo].[DimDevice] ([DeviceKey]),
    CONSTRAINT [FK_FactClick_DimOffer] FOREIGN KEY ([OfferKey]) REFERENCES [dbo].[DimOffer] ([OfferKey]),
    CONSTRAINT [FK_FactClick_DimRegion] FOREIGN KEY ([RegionKey]) REFERENCES [dbo].[DimRegion] ([RegionKey])
);






GO
CREATE NONCLUSTERED INDEX [IX_NC_ON_FactClick_CountryKey_AdvertiserKey_INC_ClickKey_RegionKey]
    ON [dbo].[FactClick]([CountryKey] ASC, [AdvertiserKey] ASC)
    INCLUDE([ClickKey], [RegionKey]);

