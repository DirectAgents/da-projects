CREATE TABLE [dbo].[UserProfile] (
    [UserId]                 INT            IDENTITY (1, 1) NOT NULL,
    [UserName]               NVARCHAR (MAX) NULL,
    [CakeAdvertiserId]       INT            NULL,
    [QuickBooksCompanyId]    INT            NULL,
    [QuickBooksAdvertiserId] INT            NULL,
    [SearchWeekStartDay]     INT            CONSTRAINT [DF_UserProfile_SearchWeekStartDay] DEFAULT ((1)) NOT NULL,
    [SearchWeekEndDay]       INT            CONSTRAINT [DF_UserProfile_SearchWeekEndDay] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.UserProfile] PRIMARY KEY CLUSTERED ([UserId] ASC)
);



