CREATE VIEW [dbo].[MediaBuyerConfirmation_MarkG] AS
SELECT
REPLACE(DB_NAME(),'DADatabase','') as Period,
a.name2 as Publisher,
m.name as [Media Buyer],
'' as [Correct Media Buyer]
FROM affiliate a JOIN mediabuyer m on m.id=a.media_buyer_id
WHERE m.id not in (9,33,50,22,27,10,1,15,44,11,45,3,38,52,41,55)
