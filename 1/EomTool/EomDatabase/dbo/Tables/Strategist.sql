CREATE TABLE [dbo].[Strategist] (
    [id]        INT             IDENTITY (1, 1) NOT NULL,
    [name]      VARCHAR (100)   NOT NULL,
    [comm_rate] DECIMAL (10, 5) NOT NULL,
    CONSTRAINT [PK_Strategist] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UK_Strategist_name] UNIQUE NONCLUSTERED ([name] ASC)
);

