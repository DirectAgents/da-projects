CREATE TABLE [dbo].[Creative] (
    [CreativeId]        INT             NOT NULL,
    [OfferId]           INT             NOT NULL,
    [CreativeTypeId]    INT             NOT NULL,
    [CreativeName]      NVARCHAR (255)  NULL,
    [DateCreated]       DATETIME        NOT NULL,
    [CreativeStatusId]  INT             CONSTRAINT [DF_Creative_CreativeStatusId] DEFAULT ((1)) NOT NULL,
    [OfferLinkOverride] NVARCHAR (255)  NULL,
    [Width]             INT             NULL,
    [Height]            INT             NULL,
    [Thumbnail]         VARBINARY (MAX) NULL,
    CONSTRAINT [PK_Creative] PRIMARY KEY CLUSTERED ([CreativeId] ASC),
    CONSTRAINT [FK_Creative_CreativeType] FOREIGN KEY ([CreativeTypeId]) REFERENCES [dbo].[CreativeType] ([CreativeTypeId]),
    CONSTRAINT [FK_Creative_Offer] FOREIGN KEY ([OfferId]) REFERENCES [dbo].[Offer] ([OfferId])
);

