CREATE TABLE [dbo].[CakeContact] (
    [CakeContactId] INT            NOT NULL,
    [CakeRoleId]    INT            NOT NULL,
    [FirstName]     NVARCHAR (100) NULL,
    [LastName]      NVARCHAR (100) NULL,
    [EmailAddress]  NVARCHAR (100) NULL,
    [Title]         NVARCHAR (100) NULL,
    [PhoneWork]     NVARCHAR (100) NULL,
    [PhoneCell]     NVARCHAR (100) NULL,
    [PhoneFax]      NVARCHAR (100) NULL,
    CONSTRAINT [PK_CakeContact] PRIMARY KEY CLUSTERED ([CakeContactId] ASC),
    CONSTRAINT [FK_CakeContact_CakeRole] FOREIGN KEY ([CakeRoleId]) REFERENCES [dbo].[CakeRole] ([CakeRoleId])
);

