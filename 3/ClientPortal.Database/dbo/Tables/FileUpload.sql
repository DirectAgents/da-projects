CREATE TABLE [dbo].[FileUpload] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [UploadDate]   DATETIME       NOT NULL,
    [Filename]     NVARCHAR (MAX) NULL,
    [Text]         NVARCHAR (MAX) NULL,
    [AdvertiserId] INT            NULL,
    CONSTRAINT [PK_dbo.FileUpload] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FileUpload_Advertiser] FOREIGN KEY ([AdvertiserId]) REFERENCES [dbo].[Advertiser] ([AdvertiserId])
);

