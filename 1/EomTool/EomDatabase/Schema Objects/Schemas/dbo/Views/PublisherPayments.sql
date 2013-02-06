CREATE VIEW [dbo].[PublisherPayments]
AS
SELECT     dbo.Affiliate.net_term_type_id AS NetTermTypeId, dbo.NetTermType.name AS NetTermType, dbo.SumKeys(dbo.Item.affid) AS AffIds, dbo.Affiliate.name AS Publisher, 
                      AffCurrency.name AS PubPayCurr, SUM(dbo.tousd3(dbo.Item.cost_currency_id, dbo.Item.cost_per_unit) * dbo.Item.num_units / AffCurrency.to_usd_multiplier) 
                      AS PubPayout, dbo.Item.item_accounting_status_id AS AccountingStatusId, dbo.ItemAccountingStatus.name AS AccountingStatus, 
                      dbo.Affiliate.payment_method_id AS PaymentMethodId, dbo.AffiliatePaymentMethod.name AS PaymentMethod, dbo.Item.payment_batch_id AS PaymentBatchId, 
                      dbo.PaymentBatch.payment_batch_state_id AS PaymentBatchStateId, dbo.PaymentBatchState.name AS PaymentBatchState, 
                      dbo.PaymentBatch.approver_identity AS ApproverIdentity, dbo.SumKeys(dbo.Item.id) AS ItemIds
FROM         dbo.Item INNER JOIN
                      dbo.Affiliate ON dbo.Item.affid = dbo.Affiliate.affid INNER JOIN
                      dbo.Currency AS AffCurrency ON dbo.Affiliate.currency_id = AffCurrency.id INNER JOIN
                      dbo.Currency AS CostCurrency ON dbo.Item.cost_currency_id = CostCurrency.id INNER JOIN
                      dbo.ItemAccountingStatus ON dbo.Item.item_accounting_status_id = dbo.ItemAccountingStatus.id INNER JOIN
                      dbo.PaymentBatch ON dbo.Item.payment_batch_id = dbo.PaymentBatch.id INNER JOIN
                      dbo.PaymentBatchState ON dbo.PaymentBatch.payment_batch_state_id = dbo.PaymentBatchState.id INNER JOIN
                      dbo.AffiliatePaymentMethod ON dbo.Affiliate.payment_method_id = dbo.AffiliatePaymentMethod.id INNER JOIN
                      dbo.NetTermType ON dbo.Affiliate.net_term_type_id = dbo.NetTermType.id
GROUP BY dbo.Affiliate.name, dbo.Item.item_accounting_status_id, dbo.ItemAccountingStatus.name, dbo.Affiliate.payment_method_id, dbo.AffiliatePaymentMethod.name, 
                      dbo.Item.payment_batch_id, dbo.PaymentBatch.payment_batch_state_id, dbo.PaymentBatchState.name, dbo.PaymentBatch.approver_identity, AffCurrency.name, 
                      dbo.Affiliate.net_term_type_id, dbo.NetTermType.name