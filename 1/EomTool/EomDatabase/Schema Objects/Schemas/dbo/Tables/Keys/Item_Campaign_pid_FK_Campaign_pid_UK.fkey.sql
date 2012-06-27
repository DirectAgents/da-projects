ALTER TABLE [dbo].[Item]
    ADD CONSTRAINT [Item_Campaign_pid_FK_Campaign_pid_UK] FOREIGN KEY ([pid]) REFERENCES [dbo].[Campaign] ([pid]) ON DELETE NO ACTION ON UPDATE NO ACTION;

