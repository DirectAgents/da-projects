ALTER TABLE [dbo].[CampaignNotes]
    ADD CONSTRAINT [CampaignNotes_Campaign_pid_FK_Campaign_pid_UK] FOREIGN KEY ([campaign_id]) REFERENCES [dbo].[Campaign] ([pid]) ON DELETE NO ACTION ON UPDATE NO ACTION;

