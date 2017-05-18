CREATE TABLE [dbo].[AnalystRole] (
    [person_id] INT NOT NULL,
    [pid]       INT NOT NULL,
    [affid]     INT NOT NULL,
    CONSTRAINT [PK_AnalystRole] PRIMARY KEY CLUSTERED ([person_id] ASC, [pid] ASC, [affid] ASC),
    CONSTRAINT [FK_AnalystRole_Affiliate] FOREIGN KEY ([affid]) REFERENCES [dbo].[Affiliate] ([affid]),
    CONSTRAINT [FK_AnalystRole_Campaign] FOREIGN KEY ([pid]) REFERENCES [dbo].[Campaign] ([pid]),
    CONSTRAINT [FK_AnalystRole_Person] FOREIGN KEY ([person_id]) REFERENCES [dbo].[Person] ([id])
);

