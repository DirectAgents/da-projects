CREATE VIEW [dbo].[Item_To_USD_View_1]
AS
SELECT     dbo.Item.id, dbo.Item.total_revenue * Currency_1.to_usd_multiplier AS total_revenue_usd, 
                      dbo.Item.total_cost * dbo.Currency.to_usd_multiplier AS total_cost_usd
FROM         dbo.Item INNER JOIN
                      dbo.Currency ON dbo.Item.cost_currency_id = dbo.Currency.id INNER JOIN
                      dbo.Currency AS Currency_1 ON dbo.Item.revenue_currency_id = Currency_1.id
