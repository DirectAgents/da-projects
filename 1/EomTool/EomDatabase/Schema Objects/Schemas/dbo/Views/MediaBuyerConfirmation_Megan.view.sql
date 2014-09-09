CREATE VIEW [dbo].[MediaBuyerConfirmation_Megan] AS
SELECT
REPLACE(DB_NAME(),'DADatabase','') as Period,
a.name2 as Publisher,
m.name as [Media Buyer],
'' as [Correct Media Buyer]
FROM affiliate a JOIN mediabuyer m on m.id=a.media_buyer_id
WHERE m.id in (9,33)
and a.affid in (select distinct affid from item)
