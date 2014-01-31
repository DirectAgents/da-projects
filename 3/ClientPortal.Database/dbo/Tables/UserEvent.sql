CREATE TABLE [dbo].[UserEvent] (
    [EventId] INT            IDENTITY (1, 1) NOT NULL,
    [Date]    DATETIME       CONSTRAINT [DF_UserEvent_Date] DEFAULT (getdate()) NOT NULL,
    [UserId]  INT            NULL,
    [Event]   NVARCHAR (255) NULL,
    CONSTRAINT [PK_UserEvent] PRIMARY KEY CLUSTERED ([EventId] ASC),
    CONSTRAINT [FK_UserEvent_UserProfile] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserProfile] ([UserId])
);

