-- =============================================
-- Author:		Aaron Anodide
-- Create date: 4/10/12
-- Description:	Set created_date after an Affiliate is inserted
--              and update modified date after Affiliate is updated.
--              Additionally ensures that date_created never changes.
-- =============================================
CREATE TRIGGER [dbo].[tr_Affiliate_IU] 
   ON  [dbo].[Affiliate] 
   AFTER INSERT, UPDATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Get the current date.
	DECLARE @getDate DATETIME = GETDATE()

    -- Set the initial values of date_created and date_modified.
    UPDATE
		dbo.Affiliate
    SET 
		 date_created = @getDate
    FROM
		dbo.Affiliate A 
		INNER JOIN INSERTED I ON A.id = I.id
		LEFT OUTER JOIN DELETED D ON I.id = D.id
	WHERE
		D.id IS NULL
		
	-- Ensure the value of date_created does never changes.
    -- Update the value of date_modified to the current date.
    UPDATE
		dbo.Affiliate
    SET
		 date_created = D.date_created
		,date_modified = @getDate
    FROM 
		dbo.Affiliate A 
		INNER JOIN INSERTED I ON A.id = I.id
		INNER JOIN DELETED D ON I.id = D.id 
END
-- ===========================
-- TODO: improve this trigger using the following re-write from http://stackoverflow.com/questions/10097685/are-these-joins-inside-an-after-insert-update-trigger-equiv-to-using-if-update
-- ===========================
--CREATE TRIGGER [dbo].[tr_Affiliate_IU]  
--   ON  [dbo].[Affiliate]  
--   AFTER INSERT, UPDATE 
--AS 
--BEGIN 
--    -- SET NOCOUNT ON added to prevent extra result sets from 
--    -- interfering with SELECT statements. 
--    SET NOCOUNT ON; 
 
--    -- Get the current date. 
--    DECLARE @getDate DATETIME = GETDATE() 
 
--    -- Set the initial values of date_created and date_modified. 
--    UPDATE 
--        dbo.Affiliate 
--    SET  
--      -- If there is a record for this ID in Deleted 
--         date_created = case when D.id is not null  
--                          -- Take date of creation from Deleted 
--                             then D.date_created  
--                             else @getDate  
--                             end 
--        ,date_modified = @getDate 
--    FROM 
--        dbo.Affiliate A  
--        INNER JOIN INSERTED I ON A.id = I.id 
--        LEFT OUTER JOIN DELETED D ON I.id = D.id 
--END 