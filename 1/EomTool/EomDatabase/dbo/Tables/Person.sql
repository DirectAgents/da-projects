CREATE TABLE [dbo].[Person] (
    [id]         INT           NOT NULL,
    [first_name] VARCHAR (100) NOT NULL,
    [last_name]  VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([id] ASC)
);

