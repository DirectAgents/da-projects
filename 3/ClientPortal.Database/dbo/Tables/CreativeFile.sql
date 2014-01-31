CREATE TABLE [dbo].[CreativeFile] (
    [CreativeFileId]   INT            NOT NULL,
    [CreativeId]       INT            NOT NULL,
    [CreativeFileName] NVARCHAR (255) NULL,
    [CreativeFileLink] NVARCHAR (255) NULL,
    [Preview]          BIT            CONSTRAINT [DF_CreativeFile_Preview] DEFAULT ((0)) NOT NULL,
    [DateCreated]      DATETIME       CONSTRAINT [DF_CreativeFile_DateCreated] DEFAULT ('1/1/00') NOT NULL,
    CONSTRAINT [PK_CreativeFile] PRIMARY KEY CLUSTERED ([CreativeFileId] ASC),
    CONSTRAINT [FK_CreativeFile_Creative] FOREIGN KEY ([CreativeId]) REFERENCES [dbo].[Creative] ([CreativeId])
);

