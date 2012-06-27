CREATE VIEW [dbo].[AccountingDetailsWithMargin]
AS
SELECT *, ([Rev USD])-([Cost USD]) [Margin USD]
FROM AccountingDetails
