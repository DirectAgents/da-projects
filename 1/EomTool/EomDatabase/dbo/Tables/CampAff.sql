CREATE TABLE [dbo].[CampAff] (
    [pid]           INT NOT NULL,
    [affid]         INT NOT NULL,
    [analyst_id]    INT NULL,
    [strategist_id] INT NULL,
    CONSTRAINT [PK_CampAff] PRIMARY KEY CLUSTERED ([pid] ASC, [affid] ASC),
    CONSTRAINT [FK_CampAff_Affiliate] FOREIGN KEY ([affid]) REFERENCES [dbo].[Affiliate] ([affid]),
    CONSTRAINT [FK_CampAff_Analyst] FOREIGN KEY ([analyst_id]) REFERENCES [dbo].[Analyst] ([id]),
    CONSTRAINT [FK_CampAff_Campaign] FOREIGN KEY ([pid]) REFERENCES [dbo].[Campaign] ([pid]),
    CONSTRAINT [FK_CampAff_Strategist] FOREIGN KEY ([strategist_id]) REFERENCES [dbo].[Strategist] ([id])
);

