CREATE TABLE [dbo].[Contact] (
    [ContactId] INT            IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (MAX) NULL,
    [LastName]  NVARCHAR (MAX) NULL,
    [Title]     NVARCHAR (MAX) NULL,
    [Email]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Contact] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);

