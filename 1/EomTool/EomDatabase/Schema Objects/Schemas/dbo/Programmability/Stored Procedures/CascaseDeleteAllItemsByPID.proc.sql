-- =============================================
-- Author:		Aaron Anodide
-- Create date: 1/9/2012
-- Description:	Cascase delete all items with a PID
-- =============================================
CREATE PROCEDURE [dbo].[CascaseDeleteAllItemsByPID] 
	@pid int
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION

		DELETE FROM Item
		WHERE pid=@pid
		
		IF @@ERROR <> 0
			BEGIN
				ROLLBACK
				RAISERROR ('Error in deleting Items in CascaseDeleteAllItemsWithPID.', 16, 1)
				RETURN
			END
			
		DELETE FROM Stat
		WHERE pid=@pid
		
		IF @@ERROR <> 0
			BEGIN
				ROLLBACK
				RAISERROR ('Error in deleting Stats in CascaseDeleteAllItemsWithPID.', 16, 1)
				RETURN
			END

		DELETE FROM Payout
		WHERE pid=@pid
		
		IF @@ERROR <> 0
			BEGIN
				ROLLBACK
				RAISERROR ('Error in deleting Payouts in CascaseDeleteAllItemsWithPID.', 16, 1)
				RETURN
			END

		COMMIT

	END
