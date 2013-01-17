CREATE VIEW CampaignPublisherPaymentBatchesSummary AS
SELECT
		COALESCE(Publisher.name, Affiliate.name) AS PublisherName
		,NetTermType.name AS NetTermTypeName
		,AffiliatePaymentMethod.name AS PaymentMethodName
		,AffiliateCurrency.name AS AffiliateCurrencyName
		,SUM(Item.total_cost * ItemCurrency.to_usd_multiplier / AffiliateCurrency.to_usd_multiplier) AS SumItemTotalCost
		,ItemAccountingStatus.name AS ItemAccountingStatusName
		,PaymentBatch.id AS PaymentBatchId
		,dbo.SumKeys(Item.id) AS ItemIds
FROM
	Item
	INNER JOIN ItemAccountingStatus ON Item.item_accounting_status_id = ItemAccountingStatus.id
	INNER JOIN Currency ItemCurrency ON Item.cost_currency_id = ItemCurrency.id
	INNER JOIN Affiliate ON Item.affid = Affiliate.affid
	INNER JOIN NetTermType ON Affiliate.net_term_type_id = NetTermType.id
	INNER JOIN AffiliatePaymentMethod ON Affiliate.payment_method_id = AffiliatePaymentMethod.id
	INNER JOIN Currency AffiliateCurrency ON Affiliate.currency_id = AffiliateCurrency.id
	LEFT OUTER JOIN Publisher ON Affiliate.affid = Publisher.affid
	LEFT OUTER JOIN PaymentBatch ON Item.payment_batch_id = PaymentBatch.id
WHERE
	ItemAccountingStatus.name IN ('Approved', 'Check Signed and Paid', 'Hold')
GROUP BY
		COALESCE(Publisher.name, Affiliate.name)
	,NetTermType.name
	,AffiliatePaymentMethod.name
	,AffiliateCurrency.name
	,ItemAccountingStatus.name
	,PaymentBatch.id
