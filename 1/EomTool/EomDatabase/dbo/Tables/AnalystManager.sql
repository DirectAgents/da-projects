CREATE TABLE [dbo].[AnalystManager] (
    [id]        INT             IDENTITY (1, 1) NOT NULL,
    [name]      VARCHAR (100)   NOT NULL,
    [comm_rate] DECIMAL (10, 5) NOT NULL,
    CONSTRAINT [PK_AnalystManager] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UK_AnalystManager_name] UNIQUE NONCLUSTERED ([name] ASC)
);

