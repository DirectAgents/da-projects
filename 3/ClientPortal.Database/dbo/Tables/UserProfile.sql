CREATE TABLE [dbo].[UserProfile] (
    [UserId]                 INT            IDENTITY (1, 1) NOT NULL,
    [UserName]               NVARCHAR (MAX) NULL,
    [CakeAdvertiserId]       INT            NULL,
    [QuickBooksCompanyId]    INT            NULL,
    [QuickBooksAdvertiserId] INT            NULL,
    CONSTRAINT [PK_dbo.UserProfile] PRIMARY KEY CLUSTERED ([UserId] ASC)
);

