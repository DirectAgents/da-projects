CREATE TABLE [dbo].[DimAdvertiser] (
    [AdvertiserKey]  INT            NOT NULL,
    [AdvertiserName] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_DimAdvertiser] PRIMARY KEY CLUSTERED ([AdvertiserKey] ASC)
);

