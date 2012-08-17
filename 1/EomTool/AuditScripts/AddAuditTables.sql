use July2012Test2
go

DECLARE @RC int
DECLARE @SchemaName sysname
DECLARE @TableName sysname
DECLARE @StrictUserContext bit
DECLARE @LogSQL bit
DECLARE @BaseTableDDL bit
DECLARE @LogInsert tinyint

set @SchemaName = 'dbo'
set @TableName = 'Item'
set @StrictUserContext = 1
set @LogSQL = 0
set @BaseTableDDL = 0
set @LogInsert = 1

EXECUTE @RC = July2012Test2.[dbo].[pAutoAudit] 
   @SchemaName
  ,@TableName
  ,@StrictUserContext
  ,@LogSQL
  ,@BaseTableDDL
  ,@LogInsert
GO
