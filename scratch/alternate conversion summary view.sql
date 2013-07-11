USE [Cake]
GO
--CREATE VIEW [staging].[CakeConversionSummary]
--AS
SELECT
	'Cake' + '/'
		+ CAST(YEAR(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, ConversionDate)), 0)) AS char(4)) 
		+ '-' + RIGHT('00' + LTRIM(STR(MONTH(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, ConversionDate)), 0)))), 2) 
		+ '-' + RIGHT('00' + LTRIM(STR(DAY(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, ConversionDate)), 0)))), 2)
		+ '/aff:' + CAST(Affiliate_Id AS varchar(20))
		+ '/offer:' + CAST(Offer_Id AS varchar(20))
		+ '/type:' + ConversionType
		+ '/paycur:' + PricePaidCurrency.Name
		+ '/paid:' + CAST(PricePaid AS varchar(20))
		+ '/recvcur:' + PriceReceivedCurrency.Name 
		+ '/recv:' + CAST(PriceReceived AS varchar(20))
	AS [Name],
	DATEADD(month, DATEDIFF(month, 0, CONVERT(date, ConversionDate)), 0) [ConversionDate],
	Affiliate_Id, 
	Offer_Id, 
	ConversionType,
	COUNT(staging.CakeConversions.Conversion_Id) [Units],
	PricePaid, 
	PriceReceived, 
	PricePaidCurrency.Name [PricePaidCurrency], 
    PriceReceivedCurrency.Name [PriceReceivedCurrency],
    COUNT(staging.CakeConversions.Conversion_Id) * PricePaid [Paid],
    COUNT(staging.CakeConversions.Conversion_Id) * PriceReceived [Received]
FROM
	staging.CakeConversions inner join
	dbo.CakeCurrency PricePaidCurrency on PricePaidCurrencyId = PricePaidCurrency.Id inner join
	dbo.CakeCurrency PriceReceivedCurrency on PriceReceivedCurrencyId = PriceReceivedCurrency.Id
WHERE
	(Deleted = 0) and (Offer_Id=12061)
GROUP BY
	DATEADD(month, DATEDIFF(month, 0, CONVERT(date, ConversionDate)), 0), 
	Affiliate_Id, 
	Offer_Id, 
	ConversionType,
	PricePaidCurrencyId,
	PriceReceivedCurrencyId,
	PricePaidCurrency.Name,
	PriceReceivedCurrency.Name,
	PricePaid,
	PriceReceived,
	'Cake' + '/'
		+ CAST(YEAR(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, ConversionDate)), 0)) AS char(4)) 
		+ '-' + RIGHT('00' + LTRIM(STR(MONTH(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, ConversionDate)), 0)))), 2) 
		+ '-' + RIGHT('00' + LTRIM(STR(DAY(DATEADD(month, DATEDIFF(month, 0, CONVERT(date, ConversionDate)), 0)))), 2)
		+ '/aff:' + CAST(Affiliate_Id AS varchar(20))
		+ '/offer:' + CAST(Offer_Id AS varchar(20))
		+ '/type:' + ConversionType
		+ '/paycur:' + PricePaidCurrency.Name
		+ '/paid:' + CAST(PricePaid AS varchar(20))
		+ '/recvcur:' + PriceReceivedCurrency.Name 
		+ '/recv:' + CAST(PriceReceived AS varchar(20))

GO
