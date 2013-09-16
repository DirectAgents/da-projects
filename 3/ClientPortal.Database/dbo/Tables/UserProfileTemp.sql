CREATE TABLE [dbo].[UserProfileTemp] (
    [UserId]                  INT            IDENTITY (1, 1) NOT NULL,
    [UserName]                NVARCHAR (MAX) NULL,
    [CakeAdvertiserId]        INT            NULL,
    [QuickBooksCompanyId]     INT            NULL,
    [QuickBooksAdvertiserId]  INT            NULL,
    [Culture]                 NVARCHAR (MAX) NULL,
    [ShowCPMRep]              BIT            CONSTRAINT [DF_UserProfileTemp_ShowCPMRep] DEFAULT ((0)) NOT NULL,
    [ShowConversionData]      BIT            CONSTRAINT [DF_UserProfileTemp_ShowConversionData] DEFAULT ((0)) NOT NULL,
    [ConversionValueName]     NVARCHAR (MAX) NULL,
    [ConversionValueIsNumber] BIT            CONSTRAINT [DF_UserProfileTemp_ConversionValueIsNumber] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.UserProfileTemp] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

