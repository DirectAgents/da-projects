CREATE VIEW [dbo].[PublisherRelatedItemCounts] AS
SELECT
       Affiliate.id Id
       ,Affiliate.name Publisher
       ,COUNT(distinct PubNotes.id) NumNotes
       ,COUNT(distinct PubAttachment.id) NumAttachments
FROM
       Item
       INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
       INNER JOIN Affiliate ON Item.affid = Affiliate.affid
       LEFT OUTER JOIN PubNotes on Affiliate.name = PubNotes.publisher_name
       LEFT OUTER JOIN PubAttachment on Affiliate.name = PubAttachment.publisher_name
GROUP BY
       Affiliate.id
       ,Affiliate.name
HAVING COUNT(distinct PubNotes.id) != 0 OR COUNT(distinct PubAttachment.id) != 0