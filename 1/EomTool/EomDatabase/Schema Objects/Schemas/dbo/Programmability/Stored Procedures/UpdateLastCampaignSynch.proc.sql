CREATE PROCEDURE [dbo].[UpdateLastCampaignSynch]
	@CampaignNumber int
AS
BEGIN
	SET NOCOUNT ON;
	--UPDATE CampaignSynch
	--SET LastSynched=GETDATE()
	--WHERE CampaignId=(
	--	SELECT id
	--	FROM Campaign
	--	WHERE pid=@CampaignNumber
	--)
END
