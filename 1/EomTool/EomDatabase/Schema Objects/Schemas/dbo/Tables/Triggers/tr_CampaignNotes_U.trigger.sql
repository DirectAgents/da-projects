CREATE TRIGGER tr_CampaignNotes_U on CampaignNotes FOR UPDATE AS
    BEGIN
        RAISERROR ('cannot update note', 16, 1)
        ROLLBACK TRANSACTION
        RETURN
    END
