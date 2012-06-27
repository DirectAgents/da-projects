CREATE VIEW [dbo].[Campaign_Default_Payout_View_1]
AS
SELECT     TOP (100) PERCENT D.pid, P.lead, P.currency_id
FROM         dbo.Payout AS P INNER JOIN
                          (SELECT     pid, MAX(effective_date) AS maxd, payout_type, affid
                            FROM          dbo.Payout
                            WHERE      (payout_type = 'campaign') AND (affid = 0)
                            GROUP BY pid, payout_type, affid) AS D ON P.pid = D.pid AND P.effective_date = D.maxd AND P.payout_type = D.payout_type AND P.affid = D.affid
ORDER BY D.pid
