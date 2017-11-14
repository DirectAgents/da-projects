CREATE TABLE [dbo].[Analyst] (
    [id]         INT             IDENTITY (1, 1) NOT NULL,
    [manager_id] INT             NULL,
    [name]       VARCHAR (100)   NOT NULL,
    [comm_rate]  DECIMAL (10, 5) NOT NULL,
    CONSTRAINT [PK_Analyst] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Analyst_AnalystManager] FOREIGN KEY ([manager_id]) REFERENCES [dbo].[AnalystManager] ([id]),
    CONSTRAINT [UK_Analyst_name] UNIQUE NONCLUSTERED ([name] ASC)
);

