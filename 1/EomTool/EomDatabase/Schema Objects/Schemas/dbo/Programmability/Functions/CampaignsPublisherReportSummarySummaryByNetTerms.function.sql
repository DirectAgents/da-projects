--SELECT Status, USD, EUR, GBP, AUD FROM dbo.CampaignsPublisherReportSummarySummaryByNetTerms('Net 7')
CREATE FUNCTION [dbo].[CampaignsPublisherReportSummarySummaryByNetTerms]
(	
	@p_net_terms varchar(50)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT [Status], SUM(USD) USD, SUM(EUR) EUR, SUM(GBP) GBP, SUM(AUD) AUD FROM (
		SELECT NetTerms, PayCurrency, [Status], Amount, CampaignStatus
		FROM CampaignsPublisherReportSummary
		UNPIVOT (Amount FOR [Status] IN (Total, ToBePaid, Paid, Approved, Verified, Unverified)) U
		WHERE NetTerms LIKE @p_net_terms
		--WHERE NetTerms LIKE 'Net 7'
		AND CampaignStatus='Verified'
	) A
	PIVOT (SUM(Amount) FOR PayCurrency IN (USD, EUR, GBP, AUD)) P
	GROUP BY [Status]
)
