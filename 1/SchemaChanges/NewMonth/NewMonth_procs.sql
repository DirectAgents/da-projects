USE [DAMain1]
GO

/****** Object:  StoredProcedure [dbo].[ColumnList]    Script Date: 03/11/2014 12:33:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kevin Slesinsky
-- Create date: 3/11/14
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[ColumnList] 
	@db_name sysname, 
	@table_name sysname,
	@columns nvarchar(max) output
AS
BEGIN
SET NOCOUNT ON;

declare @sql nvarchar(max)

SET @sql = 'SELECT @colList = coalesce(@colList+ '','', '''')+ B.NAME'
+ ' FROM ' +@db_name+ '.SYS.OBJECTS A JOIN ' +@db_name+ '.SYS.COLUMNS B ON A.object_id = B.object_id'
+ ' WHERE A.object_id = OBJECT_ID(''' + @db_name + '.dbo.' + @table_name + ''')'
+ ' AND B.is_computed=0'
+ ' ORDER BY B.column_id'
EXEC sp_executesql @sql, N'@colList varchar(max) output', @colList = @columns output

END

GO


-- =============================================
-- Author:		Kevin Slesinsky
-- Create date: 3/10/14
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[EOMcopyTable] 
	@dbFrom SYSNAME, 
	@dbTo SYSNAME,
	@tableName SYSNAME,
	@identityInsert BIT
AS
BEGIN
SET NOCOUNT ON;

DECLARE @sql NVARCHAR(max), @toTablePath NVARCHAR(max), @fromTablePath NVARCHAR(max), @colList NVARCHAR(max)

SET @toTablePath = '[' + @dbTo + '].[dbo].[' + @tableName + ']'
SET @fromTablePath = '[' + @dbFrom + '].[dbo].[' + @tableName + ']'
EXEC ColumnList @dbFrom, @tableName, @colList output

SET @sql = 'INSERT INTO ' + @toTablePath + ' (' + @colList + ')'
		+ ' SELECT ' + @colList + ' FROM ' + @fromTablePath
IF @identityInsert=1
	SET @sql = 'SET IDENTITY_INSERT ' + @toTablePath + ' ON '
		+ @sql
		+ ' SET IDENTITY_INSERT ' + @toTablePath + ' OFF'

EXEC (@sql)

END

GO


-- =============================================
-- Author:		Kevin Slesinsky
-- Create date: 3/10/14
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[EOMcopy]
	@dbFrom SYSNAME,
	@dbTo SYSNAME
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;

DECLARE @sql NVARCHAR(max)

SET @sql = 'USE ' + @dbTo + '; DISABLE TRIGGER tr_Affiliate_IU ON Affiliate'
EXEC (@sql)

EXEC EOMcopyTable @dbFrom,@dbTo,'AccountManager',1
EXEC EOMcopyTable @dbFrom,@dbTo,'AdManager',1
EXEC EOMcopyTable @dbFrom,@dbTo,'Advertiser',1
EXEC EOMcopyTable @dbFrom,@dbTo,'AffiliatePaymentMethod',1
EXEC EOMcopyTable @dbFrom,@dbTo,'Currency',1
EXEC EOMcopyTable @dbFrom,@dbTo,'CampaignStatus',1
EXEC EOMcopyTable @dbFrom,@dbTo,'ItemAccountingStatus',1
EXEC EOMcopyTable @dbFrom,@dbTo,'ItemReportingStatus',1
EXEC EOMcopyTable @dbFrom,@dbTo,'DTCampaignStatus',1
EXEC EOMcopyTable @dbFrom,@dbTo,'MediaBuyer',1
EXEC EOMcopyTable @dbFrom,@dbTo,'NetTermType',1
EXEC EOMcopyTable @dbFrom,@dbTo,'Source',1
EXEC EOMcopyTable @dbFrom,@dbTo,'IncomeType',0
EXEC EOMcopyTable @dbFrom,@dbTo,'UnitType',1
EXEC EOMcopyTable @dbFrom,@dbTo,'Vendor',1

EXEC EOMcopyTable @dbFrom,@dbTo,'Campaign',1
EXEC EOMcopyTable @dbFrom,@dbTo,'Affiliate',1

EXEC EOMcopyTable @dbFrom,@dbTo,'Person',0
EXEC EOMcopyTable @dbFrom,@dbTo,'AnalystRole',0

EXEC EOMcopyTable @dbFrom,@dbTo,'AnalystManager',1
EXEC EOMcopyTable @dbFrom,@dbTo,'Analyst',1
EXEC EOMcopyTable @dbFrom,@dbTo,'Strategist',1

SET @sql = 'USE ' + @dbTo + ';'
+ ' UPDATE Affiliate SET date_created=NULL, date_modified=NULL;'
+ ' ENABLE TRIGGER tr_Affiliate_IU ON Affiliate;'
+ ' UPDATE Campaign SET campaign_status_id=3 WHERE (campaign_status_id=2) OR (campaign_status_id=4)'
EXEC (@sql)

END

GO
