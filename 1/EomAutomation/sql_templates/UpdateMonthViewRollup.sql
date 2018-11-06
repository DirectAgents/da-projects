USE [DAMain1]
GO

/****** Object:  View [dbo].[CommissionViewRollupJan18ToDec18]    Script Date: 10/19/2018 17:14:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[CommissionViewRollupJan18ToDec18] AS
--SELECT * FROM (select '2018-12' AS Period, DADatabaseDec2018.dbo.CommissionView.* from DADatabaseDec2018.dbo.CommissionView) DADatabaseDec2018 UNION ALL
--SELECT * FROM (select '2018-11' AS Period, DADatabaseNov2018.dbo.CommissionView.* from DADatabaseNov2018.dbo.CommissionView) DADatabaseNov2018 UNION ALL
SELECT * FROM (select '2018-10' AS Period, DADatabaseOct2018.dbo.CommissionView.* from DADatabaseOct2018.dbo.CommissionView) DADatabaseOct2018 --UNION ALL
--SELECT * FROM (select '2018-09' AS Period, DADatabaseSep2018.dbo.CommissionView.* from DADatabaseSep2018.dbo.CommissionView) DADatabaseSep2018 UNION ALL
--SELECT * FROM (select '2018-08' AS Period, DADatabaseAug2018.dbo.CommissionView.* from DADatabaseAug2018.dbo.CommissionView) DADatabaseAug2018 UNION ALL
--SELECT * FROM (select '2018-07' AS Period, DADatabaseJuly2018.dbo.CommissionView.* from DADatabaseJuly2018.dbo.CommissionView) DADatabaseJuly2018 UNION ALL
--SELECT * FROM (select '2018-06' AS Period, DADatabaseJune2018.dbo.CommissionView.* from DADatabaseJune2018.dbo.CommissionView) DADatabaseJune2018 UNION ALL
--SELECT * FROM (select '2018-05' AS Period, DADatabaseMay2018.dbo.CommissionView.* from DADatabaseMay2018.dbo.CommissionView) DADatabaseMay2018 UNION ALL
--SELECT * FROM (select '2018-04' AS Period, DADatabaseApril2018.dbo.CommissionView.* from DADatabaseApril2018.dbo.CommissionView) DADatabaseApril2018 UNION ALL
--SELECT * FROM (select '2018-03' AS Period, DADatabaseMarch2018.dbo.CommissionView.* from DADatabaseMarch2018.dbo.CommissionView) DADatabaseMarch2018 UNION ALL
--SELECT * FROM (select '2018-02' AS Period, DADatabaseFeb2018.dbo.CommissionView.* from DADatabaseFeb2018.dbo.CommissionView) DADatabaseFeb2018 UNION ALL
--SELECT * FROM (select '2018-01' AS Period, DADatabaseJan2018.dbo.CommissionView.* from DADatabaseJan2018.dbo.CommissionView) DADatabaseJan2018 

GO