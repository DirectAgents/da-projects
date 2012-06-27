CREATE FUNCTION [dbo].[GetCampaignsPublisherReportSummaryByNetTerms](@p_net_terms varchar(50))
RETURNS TABLE AS
RETURN (
	SELECT 'Total' [Status], * 
	FROM(
		SELECT Total,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(Total) FOR PayCurrency in (USD,EUR,GBP,AUD))P
	UNION
	SELECT 'ToBePaid' [Status], * 
	FROM(
		SELECT ToBePaid,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(ToBePaid) FOR PayCurrency in (USD,EUR,GBP,AUD))P
	UNION
	SELECT 'Approved' [Status], * 
	FROM(
		SELECT Approved,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(Approved) FOR PayCurrency in (USD,EUR,GBP,AUD))P
	UNION
	SELECT 'Verified' [Status], * 
	FROM(
		SELECT Verified,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(Verified) FOR PayCurrency in (USD,EUR,GBP,AUD))P
	UNION
	SELECT 'Unverified' [Status], * 
	FROM(
		SELECT Unverified,PayCurrency 
		FROM CampaignsPublisherReportSummary 
		WHERE NetTerms LIKE @p_net_terms)A
	PIVOT(SUM(Unverified) FOR PayCurrency in (USD,EUR,GBP,AUD))P
)
