CREATE DEFAULT [dbo].[CampaignNotes_added_by_system_user_DF]
    AS suser_sname();


GO
EXECUTE sp_bindefault @defname = N'[dbo].[CampaignNotes_added_by_system_user_DF]', @objname = N'[dbo].[CampaignNotes].[added_by_system_user]';

