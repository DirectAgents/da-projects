
USE DADatabaseJuly2012  -- edit for your database

/* 
Please read completely and test on your database.

AutoAudit makes several changes to your tables. 

----------------------------------

AutoAudit script
for SQL Server 2005/2008
(c) 2007-2010 Paul Nielsen Consulting, inc.
www.sqlserverbible.com
autoaudit.codeplex.com


August 2010
Version 2.00h 

----------------------------------
executing this script will add the following 
objects to your database:

  - dbo.Audit table
  - dbo.pAutoAudit proc
  - dbo.pAutoAutitDrop proc 
  - dbo.pAutoAuditAll proc
  - dbo.pAutoAuditDroppAll proc 

  - dbo.SchemaAudit Table
  - SchemaAuditDDLTrigger DDL Trigger 
     (on database for DDL_DATABASE_LEVEL_EVENTS)


----------------------------------
Features (default behaviors):
Code-gens triggers to records all inserts, updates, and deletes 
into a common generic audit table. 

on insert: Records insert event in Audit table including
  who made the insert, when, from what application and workstation. 
  The row's Created and CreatedBy columns also
  reflect the user context. 

on update: Records update events in the Audit table including 
  who, when, from where, and the before and after values. 
  The row's Modified and ModifiedBy columns also store the basic 
  audit data. The update also increments the row's RowVersion column 

On delete: all the final values are written to the audit table
  while this permits undeleting rows, it is performance intensive 
  when deleting a large number of rows on a wide table.  

----------------------------------
Limitations:

  Limited to tables with single column primary keys.

  Does not audit changes of columns of these data types:
  text, ntext, image, geography, xml, binary, varbinary, timestamp,
  sql_variant 

  Adding AutoAudit triggers to a table will impact performance, 
  potentially doubling or tripling the normal DML execution times. 
  The width of the table increases the impact of the AutoAudit 
  triggers during updates. 

--------------------------------------------------------------------
--------------------------------------------------------------------
* pAutoAudit Procedure

applies AutoAudit to a single table

parameters: 
  @Schema SYSNAME - the schema of the table (default = 'dbo') 
  @TableName SYSNAME - the name of the table (required)
  
  @StrictUserContext BIT (default = 1)
     (SYSNAME is NVARCHAR(128))
  @LogSQL BIT (Default = 0)
  @BaseTableDDL BIT (Default = 1)
  @LogInsert TINYINT (Default = 1)    

---
pAutoAudit will make the following changes:

  add columns: Created, CreatedBy, Modified, ModifiedBy, 
               and RowVersion
  add triggers: tablename_Audit_Insert, tablename_Audit_Update, 
                tablename_Audit_Delete
  add view: schema.vtablename_Deleted
  add function: schema.tablename_RowHistory

---
Options: 

@StrictUserContext determines how user context columns are set
      (user - CreatedBy and ModifiedBy, audit time - Created 
       and Modified)  
    1 = (default) user context set by server login - suser_sname() 
        and server time (GetDate())
    0 = user context default to server values, but can be determined 
        by DML and are nullable. 
    
    
    When using @StrictUserContext = 0: 
      Insert: an insert DML statement can insert into the created 
              and createdby columns. 
      Update: an update DML statement can freely update the created, 
              createdby, modified, and modifiedby columns.
      Delete: delete DML statements do not include columns, so the 
              when @StrictUserContext is set to 0, the previous modified 
              and modified values are captured into the audit trail table. 
              To record the correct delete user and datetime, first touch 
              (update) the row's modified and/or modifiedby columns.  
      
    For most applications leaving @StrictUserContext on is approriate. 
    Turning @StrictUserContext off is useful for two use cases: 
       1 - applications that manage their own user security and log into 
           SQL Server using a common security context. These applications 
           can pass the user name to AutoAudit by inserting into the base 
           table's CreatedBy column or updating the base table's Modified 
           column. 
       2 - when importing data from a previous database that already has
           legacy audit data. 
       
    The StrictUserContext = 0 requires the BaseTableDDL option enabled, 
        since the CreatedBy and ModifiedBy columns are used to pass in 
        the user context. 

---
@LogSQL determines if the SQL batch that fired the event is logged

    1 = the SQL Batch is logged in the SQLStatement column
    0 = (default) the SQL Batch is not logged
    
SQL logging is useful for debugging, however, it can **severely** BlOaT the 
audit log, so it should be normally set off (or the storage team will laugh 
at you when your 6 Gb database grows to 115Gb in a week ;-)  

--- 
@BaseTableDDL determines if the created, modified, and RowVersion columns
    are added to the base tables 

    0 = make no changes to the base tables
    1 = (default) add the created, createdby, modified, modifiedby, and 
        rowversion columns to the base tables

Adding the created, modified, and rowversion columns is appropriate for 
   most tables. However, some third party databases do not allow modifying 
   the base table. 
   
---
@LogInsert determines how much is logged to the audit trail on an insert 
    event. 

    0 - nothing is logged to the audit trail table
    1 - (default) the insert event is written to the audit trail table 
        as a single row
    2 - all columns are written to the audit trail table, essentially 
        creating a complete copy of the base table

Regardless of the @LogInsert setting, the created and createdby columns on 
    the base table are alway set. 

---

To change the options for a table, simply re-exec the pAutoAudit proc and 
    re-generate the triggers for the table.


--------------------------------------------------------------------
--------------------------------------------------------------------
* pAutoAuditAll Procedure

executes pAutoAudit for every basetable

no parameters


--------------------------------------------------------------------
--------------------------------------------------------------------
* pAutoAuditDrop Procedure

Removes columns, triggers, view, and function 
created by pAutoAudit for a single table. 

parameters: 
  @Schema SYSNAME - the schema of the table (default = 'dbo') 
  @TableName SYSNAME - the name of the table (required)

  (SYSNAME is NVARCHAR(128))

It does not remove the audit table or schemaAudit 
trigger or table created when this script is executed 
in a database. 


--------------------------------------------------------------------
--------------------------------------------------------------------
* pAutoAuditDropAll Procedure

executes pAutoAudit for every basetable

Important: pAutoAuditDropAll completely removes AutoAudit 
from the database, including removing the schema audit 
DDL trigger and table, and the Audit table. 




-----------------------------------------------------------------
-----------------------------------------------------------------
Development Change History

-----------------------------------
version 1.01 - Jan 15, 2007
   added rowversion column, incremented by the modified trigger
   cleaned up how the tablename is written to the audit.tablename column 
   added delete trigger, which just writes the table, pk, and operation ('d') to the audit table
   changed Audit.[Column] to Audit.ColumnName
-----------------------------------
version 1.02 - Jan 16, 2007
   fixed bug: Duplicate Columns. databases with user-defined types was causing the user-defined types to show up as system types. 
   added code gen to create [table]_deleted view that returns all deleted rows for the table

-----------------------------------
version 1.03 - Jan 16, 2007
   converted from cursor to Multiple Assignment Variable for building of for-each-column code
   added created, modified, and deleted columns to _deleted view 

-----------------------------------
version 1.04 - Jan 18, 2007
  minor clean-up on _deleted view. Removed extra Primary Key Column. 

-----------------------------------
version 1.05 - Jan 18, 2007
  changed from writing just the delete bit to writing the whole row. 
  modified _deleted view to return RowVersion

-----------------------------------
version 1.06 - Jan 30, 2007
  added host_name to audit trail
  improved modified trigger run-away recursive trigger detection
  added basic error-trapping

-----------------------------------
version 1.07 - Feb 6, 2007
  idea from Gary Lail - don't log inserts, only updates
  added pRollbackAudit procedure
  changed all stored procedure names to pName
  CREATE PROC usp AS SELECT OBJECT_NAME( @@PROCID )

-----------------------------------
version 1.08 - June 25, 2008
  case sensitive cleanup
  defaults named properly
  defaults and columns dropped in AutoAuditDrop proc

-----------------------------------
version 1.09 - Oct 15, 2008
  fixed @tablename bug in AutoAuditDrop
  changed audit time from GetDate() to inserted.created and inserted.modified to keep these times in synch
  changed from 'data type in()' to 'data type not in (xml, varbinary, image, text)'  
  added support for hierarchyID tracking (from Cast to Convert)
  added check: Table must have PK
  added check: PK must not be HierarchyID
  added RowVersion to dbo.Audit, and insert/update/delete procs
  added RowHistory Table Valued Function
  added SchemaAudit table and database trigger
  SchemaAuditDDLTrigger also fires pAutoAudit for Alter_Table events for tables with AutoAudit

-----------------------------------
version 1.09a - Oct 18, 2008
  fixed hard-coded path in _RowHistory dynamic SQL builder code
  changed _RowHistory values not updated from 0 to null
  
-----------------------------------
version 1.09b - Oct 23, 2008
  changed SchemaAudit.Schema and .Object to allow nulls for events that do not have schema.object
  
-----------------------------------
version 1.10 - Jan 24, 2010

  issue: NULL Updates that don't actually update anything 
    were still updating the modified column
    and incrementing the RowVersion
  fix: 
    eliminated  the Modified trigger
    moved updating the Modified Column and incrementing the version number to the Update Trigger
    
  moved update of created col to insert trigger
  added modified and RowVersion col to Updated
  
  improved error reporting slightly
  
  added capture of user's SQL Statement/Batch
  
  added SET ARITHABORT ON : bug and fix reported by pjl on CodePlex on Jun 15 2009 at 9:35 AM
 
  added CreatedBy and ModifiedBy columns. If names passed to tables, then this value captured for Audit trail.

-----------------------------------
version 1.10e - Mar 20, 2010
  
  cleaned up documentation
  cleaned up SYSNAME data type for parameters
  added .dbo as default to schema parameter
  added drop of audit tables and ddl trigger to pAutoAuditDropAll

-----------------------------------
version 2.00 - April 5, 2010

  Added StrictUserContext Option
     @StrictUserContext = 1
       if 1 then blocks DML inserting or updating created, createdby, modified, modifiedby
       if 0 then permits DML setting of created, createdby, modified, modifiedby
       
-----------------------------------
version 2.00c - April 26, 2010
   increased Audit.Application column to 128 to allow for SSIS package names 
 
-----------------------------------
version 2.00d - May, 2010 
   bug fixes for StrictSUer Context

-----------------------------------
version 2.00e July, August 2010

  more bug fixes for StrictSUer Context

-- Get Modified working tweak CreatedBy no updated logic
 
  added @LogSQL option
  added @BaseTableDDL option
  
-----------------------------------
version 2.00f July, August 2010
  added @LogInsert option 

-----------------------------------
version 2.00g August, 2010
  removed createdby, modifiedby from RowHistory function
  
  added sql_variant to the list of not audited data types
  it was giving the RowHistory function a conumption
  
  Added brackets around primary key column name in RowHistory function (reported by Anthony - SQLDownUnder) 
  
-----------------------------------
version 2.00g August, 2010
  fixed drop of SchemaTable in pAutoAuditDropAll (reported by Calvin Jones)
  changed StrictUserContext ModifiedBy column constraint to NOT NULL (reported by Calvin Jones)
  removed variable initialization for SQL Server 2005 compatability (reported by Calvin Jones) 


-----------------------------------
  Possible Next ideas: 
  
  QuoteName() (suggested by Rob Farley)
  ---
  set options in extended properties (suggested by Calvin Jones)
  
                EXEC sp_addextendedproperty 
                    @name = N'StrictUserContext', @value = 1,
                    @level0type = N'Schema', @level0name = Juror,
                    @level1type = N'Table',  @level1name = tblJuror,
                    @level2type = N'TRIGGER', @level2name = trg_Juror_Audit_Delete;

              SELECT * FROM ::fn_listextendedproperty('StrictUserContext', 'SCHEMA','Juror', 'TABLE','tblJuror', 'TRIGGER','trg_Juror_Audit_Delete')
              WHERE value=1
  ---
  Option Switch for DDLSchemaLog 
  ---
  Archiving of Audit table
    Move to Archive table proc set up as Job
    Indexing of Archive table
    View to Union Audit and AuditArchive tables
  ---
  Change all code to Audit schema (suggested by Calvin Jones)
  ---
  Exclude MS tables and Audit schema table from AutoAuditAll (suggested by Calvin Jones)
  ---
  Add MS_Description extended property to columns added by AutoAudit (suggested by Calvin Jones) 

*/

SET ANSI_NULL_DFLT_ON ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET QUOTED_IDENTIFIER ON

IF Object_id('SchemaAudit') IS NULL
  CREATE TABLE dbo.SchemaAudit (
    AuditDate DATETIME NOT NULL,
    UserName SYSNAME NOT NULL,
    [Event] SYSNAME NOT NULL,
    [Schema] SYSNAME NULL,
    [Object] VARCHAR(50) NULL,
    [TSQL] VARCHAR(max) NOT NULL,
    [XMLEventData] XML NOT NULL
    );
go 

If Exists(select * from sys.triggers where name = 'SchemaAuditDDLTrigger')
   DROP TRIGGER SchemaAuditDDLTrigger ON Database
   
go -----------------------------------------------
 
   
CREATE TRIGGER [SchemaAuditDDLTrigger]
ON DATABASE
FOR DDL_DATABASE_LEVEL_EVENTS
AS 
BEGIN
  -- Added by AutoAudit 
  -- www.SQLServerBible.com 
  -- Paul Nielsen 
  SET NoCount ON
  SET ARITHABORT ON
 
  DECLARE 
    @EventData XML,
    @Schema SYSNAME,
    @Object SYSNAME,
    @EventType SYSNAME,
    @SQL VARCHAR(max)
    
  SET @EventData = EventData()
  
  SET @Schema = @EventData.value('data(/EVENT_INSTANCE/SchemaName)[1]', 'VARCHAR(50)')
  SET @Object = @EventData.value('data(/EVENT_INSTANCE/ObjectName)[1]', 'VARCHAR(50)')
  SET @EventType = @EventData.value('data(/EVENT_INSTANCE/EventType)[1]', 'VARCHAR(50)')
  
  
  INSERT SchemaAudit (AuditDate, UserName, [Event], [Schema], Object, TSQL, [XMLEventData])
  SELECT 
    GetDate(),
    @EventData.value('data(/EVENT_INSTANCE/UserName)[1]', 'SYSNAME'),
    @EventType, @Schema, @Object,
    @EventData.value('data(/EVENT_INSTANCE/TSQLCommand/CommandText)[1]', 'VARCHAR(max)'),
    @EventData
    
  IF @EventType = 'ALTER_TABLE'
     AND Exists(Select * 
                  from sys.objects o
                    join syscomments sc
                      on  o.object_id = sc.id  
                  where name = @Object + '_Audit_Insert'
                   and text Like '%StrictUserContext = 1%'
                )
    BEGIN 
      SET @SQL = 'EXEC pAutoAudit ''' + @Schema + ''', ''' + @Object + ''', @StrictUserContext = 1'
      EXEC (@SQL)
    END 
    
  IF @EventType = 'ALTER_TABLE'
     AND Exists(Select * 
                  from sys.objects o
                    join syscomments sc
                      on  o.object_id = sc.id  
                  where name = @Object + '_Audit_Insert'
                   and text Like '%StrictUserContext = 0%'
                )
    BEGIN 
      SET @SQL = 'EXEC pAutoAudit ''' + @Schema+ ''', ''' + @Object + ''', @StrictUserContext = 0'
      EXEC (@SQL)
    END 
   
END   

go -----------------------------------------------

IF Object_id('Audit') IS NULL
	CREATE TABLE dbo.Audit (
	  AuditID BIGINT NOT NULL IDENTITY 
	    CONSTRAINT pkAudit PRIMARY KEY,
	  AuditDate DATETIME NOT NULL,
	  HostName sysname NOT NULL,
	  SysUser NVARCHAR(128) NOT NULL,
	  Application VARCHAR(128) NOT NULL,
	  TableName sysname NOT NULL,
	  Operation CHAR(1) NOT NULL, -- i,u,d
	  SQLStatement VARCHAR(max) NULL, -- new column to capture SQL Statement
	  PrimaryKey VARCHAR(25) NOT NULL, -- edit to suite, change to INT for databases with all INT Identity Columns
	  RowDescription VARCHAR(50) NULL,-- Optional, not used 
	  SecondaryRow VARCHAR(50) NULL, -- Optional, not used  
	  ColumnName sysname NULL, -- required for i,u, and now D (ver 1.07), should add check constraint
	  OldValue VARCHAR(max) NULL, -- edit to suite (Nvarchar() ?, varchar(MAX) ? ) 
	  NewValue VARCHAR(max) NULL, -- edit to suite (Nvarchar() ?, varchar(MAX) ? )
	  [RowVersion] INT NULL
	  )  -- optimzed for inserts, no non-clustered indexes

go --------------------------------------------------------------------

-- for backward compatability
-- RowVersion was added to Audit table in version 1.09
IF not exists( select *
			  from sys.tables t
				join sys.schemas s
				  on s.schema_id = t.schema_id
				join sys.columns c
				  on t.object_id = c.object_id
			  where  t.name = 'Audit' AND s.name = 'dbo' and c.name = 'RowVersion')
 ALTER TABLE dbo.Audit 
   ADD RowVersion INT NULL

go --------------------------------------------------------------------

IF Object_id('pAutoAudit') IS NOT NULL
  DROP PROC pAutoAudit
IF Object_id('pAutoAuditAll') IS NOT NULL
  DROP PROC pAutoAuditAll
IF Object_id('pAutoAuditDrop') IS NOT NULL
  DROP PROC pAutoAuditDrop
IF Object_id('pAutoAuditDropAll') IS NOT NULL
  DROP PROC pAutoAuditDropAll

go --------------------------------------------------------------------

CREATE PROC pAutoAudit (
   @SchemaName SYSNAME = 'dbo',
   @TableName SYSNAME,
   @StrictUserContext BIT = 1,  -- 2.00 if 0 then permits DML setting of created, createdby, modified, modifiedby
   @LogSQL BIT = 0,
   @BaseTableDDL BIT = 1,
   @LogInsert TINYINT = 1
) 
AS 
SET NoCount ON

DECLARE @version VARCHAR(5) 
  SET @version = '2.00h'
  
IF @StrictUserContext = 0 AND @BaseTableDDL = 0 
  BEGIN 
    RAISERROR('@StrictUserContext = 0 requires  @BaseTableDDL = 1. Cannot apply AutoAudit. ' , 16,1)
    RETURN 
  END   

-- script to create autoAudit triggers
DECLARE 
   @SQL NVARCHAR(max),
   @ColumnName  sysname,
   @PKColumnName sysname, 
   @PKDataType sysname
   
--Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id where o.name =  
--= 'Department' and s.name = 'HumanResources'


-- drop existing insert trigger
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''' + @TableName + '_Audit_Insert' + ''' )'
       + ' DROP TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Insert'
EXEC (@SQL)

-- drop existing update trigger
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''' + @TableName + '_Audit_Update' + ''' )'
       + ' DROP TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Update'
EXEC (@SQL)

-- drop existing delete trigger
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''' + @TableName + '_Audit_Delete' + ''' )'
       + ' DROP TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Delete'
EXEC (@SQL)

-- drop existing _deleted view
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''v' + @TableName + '_Deleted' + ''' )'
       + ' DROP VIEW ' + @SchemaName + '.v' + @TableName + '_Deleted'
EXEC (@SQL)

-- drop existing _RowHistory UDF
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''' + @TableName + '_RowHistory' + ''' )'
       + ' DROP FUNCTION ' + @SchemaName + '.' + @TableName + '_RowHistory'
EXEC (@SQL)


IF @BaseTableDDL = 1 
  BEGIN 
  
    -- add created column 
    IF not exists (select *
			      from sys.tables t
				    join sys.schemas s
				      on s.schema_id = t.schema_id
				    join sys.columns c
				      on t.object_id = c.object_id
			      where  t.name = @TableName AND s.name = @SchemaName and c.name = 'Created')
      BEGIN -- is this default causing an issue? 
        IF @StrictUserContext = 1                                                                                        
          SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD Created DateTime NOT NULL Constraint ' + @TableName + '_Created_df Default GetDate()'
        ELSE   
          SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD Created DateTime NULL Constraint ' + @TableName + '_Created_df Default GetDate()'
        EXEC (@SQL)
      END

    -- add createdBy column 
    IF not exists (select *
			      from sys.tables t
				    join sys.schemas s
				      on s.schema_id = t.schema_id
				    join sys.columns c
				      on t.object_id = c.object_id
			      where  t.name = @TableName AND s.name = @SchemaName and c.name = 'CreatedBy')
      BEGIN 
        IF @StrictUserContext = 1                                                                                        
          SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CreatedBy NVARCHAR(128) NOT NULL Constraint ' + @TableName + '_CreatedBy_df Default(Suser_SName())'
        ELSE   
          SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD CreatedBy NVARCHAR(128) NULL Constraint ' + @TableName + '_CreatedBy_df Default(Suser_SName())'
        EXEC (@SQL)
      END

    -- add Modified column 
    IF not exists( select *
			      from sys.tables t
				    join sys.schemas s
				      on s.schema_id = t.schema_id
				    join sys.columns c
				      on t.object_id = c.object_id
			      where  t.name = @TableName AND s.name = @SchemaName and c.name = 'Modified')
      BEGIN                                                                                               
        IF @StrictUserContext = 1                                                                                        
          SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD Modified DateTime NOT NULL Constraint ' + @TableName + '_Modified_df Default GetDate()'
        ELSE   
          SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD Modified DateTime NULL Constraint ' + @TableName + '_Modified_df Default GetDate()'
        EXEC (@SQL)
      END
      
    -- add createdBy column 
    IF not exists (select *
			      from sys.tables t
				    join sys.schemas s
				      on s.schema_id = t.schema_id
				    join sys.columns c
				      on t.object_id = c.object_id
			      where  t.name = @TableName AND s.name = @SchemaName and c.name = 'ModifiedBy')
      BEGIN 
        IF @StrictUserContext = 1                                                                                        
          SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD ModifiedBy NVARCHAR(128) NOT NULL Constraint ' + @TableName + '_ModifiedBy_df Default(Suser_SName())'
        ELSE  
          SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD ModifiedBy NVARCHAR(128) NULL Constraint ' + @TableName + '_ModifiedBy_df Default(Suser_SName())'
        EXEC (@SQL)
      END  

    -- add RowVersion column 
    IF not exists( select *
			      from sys.tables t
				    join sys.schemas s
				      on s.schema_id = t.schema_id
				    join sys.columns c
				      on t.object_id = c.object_id
			      where  t.name = @TableName AND s.name = @SchemaName and c.name = 'RowVersion')
      BEGIN   
        SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] ADD RowVersion INT NULL Constraint ' + @TableName + '_RowVersion_df Default 1'
        EXEC (@SQL)
      END
      
 END -- @BaseTableDDL = 1     

-- get PK Column (1)  
select @PKColumnName = c.name, @PKDataType = ty.name
  from sys.tables t
    join sys.schemas s
      on s.schema_id = t.schema_id
    join sys.indexes i
      on t.object_id = i.object_id
    join sys.index_columns ic
  	  on i.object_id = ic.object_id
      and i.index_id = ic.index_id
    join sys.columns c
      on ic.object_id = c.object_id
      and ic.column_id = c.column_id
	join sys.types as ty
	  on ty.user_type_id = c.user_type_id
  where is_primary_key = 1 AND t.name = @TableName AND s.name = @SchemaName AND ic.index_column_id = 1

-- Table no-PK Check  
  IF @PKColumnName IS NULL 
  BEGIN 
    PRINT '*** ' + @SchemaName + '.' + @TableName + ' invalid Table - no Primary Key. No triggers created.'
    RETURN
  END  
  
-- Table HierarchyID-PK Check  
  IF @PKDataType = 'HierarchyID'
  BEGIN 
    PRINT '*** ' + @SchemaName + '.' + @TableName + ' HierarchyID PK. No triggers created.'
    RETURN
  END  
  
--------------------------------------------------------------------------------
--------------------------------------------------------------------------------
-- build insert trigger 
  
SET @SQL = 'CREATE TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Insert' + ' ON ['+ @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
       + ' AFTER Insert' + Char(13) + Char(10) + ' NOT FOR REPLICATION AS' + Char(13) + Char(10)
       + ' SET NoCount On ' + Char(13) + Char(10)
       + ' SET ARITHABORT ON ' + Char(13) + Char(10)
      
       + ' -- generated by AutoAudit Version ' + @Version + ' on ' + Convert(VARCHAR(30), GetDate(),100)  + Char(13) + Char(10)
       + ' -- created by Paul Nielsen ' + Char(13) + Char(10)
       + ' -- www.SQLServerBible.com ' + Char(13) + Char(10)
       + ' -- autoaudit.codeplex.com ' + Char(13) + Char(10) + Char(13) + Char(10)

       + ' -- Options: ' + Char(13) + Char(10)
       + ' --   StrictUserContext  :' + CAST(@StrictUserContext as CHAR(1)) + Char(13) + Char(10)
       + ' --   LogSQL             :' + CAST(@LogSQL as CHAR(1)) + Char(13) + Char(10)
       + ' --   BaseTableDDL       :' + CAST(@BaseTableDDL as CHAR(1)) + Char(13) + Char(10) + Char(13) + Char(10)
       + ' --   LogInsert          :' + CAST(@LogInsert as CHAR(1)) + Char(13) + Char(10) + Char(13) + Char(10)
       
       + 'DECLARE ' + Char(13) + Char(10)
       + '  @AuditTime DATETIME, ' + Char(13) + Char(10)
       + '  @IsDirty BIT' + Char(13) + Char(10)
       
       -- keep the variable initialization separate for SQL Server 2005
       + 'SET @AuditTime = GetDate()' + Char(13) + Char(10) + Char(13) + Char(10)
       + 'SET @IsDirty = 0' + Char(13) + Char(10) + Char(13) + Char(10)
       
       + ' Begin Try ' + Char(13) + Char(10)
       
       
   IF @LogSQL = 1 
     BEGIN  
     	 select @SQL = @SQL
         + ' -- capture SQL Statement' + Char(13) + Char(10)
         + ' DECLARE @ExecStr varchar(50), @UserSQL nvarchar(max)' + Char(13) + Char(10)
         + ' DECLARE  @inputbuffer TABLE' + Char(13) + Char(10) 
         + ' (EventType nvarchar(30), Parameters int, EventInfo nvarchar(max))' + Char(13) + Char(10)
         + ' SET @ExecStr = ''DBCC INPUTBUFFER(@@SPID) with no_infomsgs''' + Char(13) + Char(10)
         + ' INSERT INTO @inputbuffer' + Char(13) + Char(10) 
         + '   EXEC (@ExecStr)' + Char(13) + Char(10)
         + ' SELECT @UserSQL = EventInfo FROM @inputbuffer' + Char(13) + Char(10)
         + Char(13) + Char(10) 
     END   
            
IF @LogInsert >= 1 -- log the PK row to the Audit Trail table as the insert event 
-- for PK column
	select @SQL = @SQL
          + Char(13) + Char(10)
		      + '   INSERT dbo.Audit (AuditDate, SysUser, Application, HostName, TableName, Operation, SQLStatement, PrimaryKey, RowDescription, SecondaryRow, ColumnName, NewValue, RowVersion)' + Char(13) + Char(10)
		      + '   SELECT ' 
		      
		      -- StrictUserOption
		      + CASE @StrictUserContext
		          WHEN 0 -- allow DML setting of created/modified user and datetimes
		            THEN ' COALESCE(Inserted.Created, @AuditTime), COALESCE(Inserted.CreatedBy, Suser_SName()),'
		          ELSE -- block DML setting of user context 
		             ' @AuditTime, Suser_SName(),'
		        END 
		      
		      + ' APP_NAME(), Host_Name(), ' 
          + '''' + @SchemaName + '.' + @TableName + ''', ''i'','
          
          -- if @LogSQL is off then the @UserSQL variable has not been declared
          + CASE @LogSQL
              WHEN 1 THEN ' @UserSQL, '
              ELSE ' NULL, ' 
           END  
          + Char(13) + Char(10)  
          
          + ' Inserted.[' + @PKColumnName + '],' + Char(13) + Char(10) 
          + '        NULL,     -- Row Description (e.g. Order Number)' + Char(13) + Char(10)   
          + '        NULL,     -- Secondary Row Value (e.g. Order Number for an Order Detail Line)' + Char(13) + Char(10)
          + '        ''[' + c.name + ']'','   
          + ' Cast(Inserted.[' + c.name + '] as VARCHAR(50)), 1' + Char(13) + Char(10)
          + '          FROM Inserted' + Char(13) + Char(10)
          + '          WHERE Inserted.['+ c.name + '] is not null' + Char(13) + Char(10)+ Char(13) + Char(10)
	  from sys.tables as t
		  join sys.columns as c
		    on t.object_id = c.object_id
		  join sys.schemas as s
		    on s.schema_id = t.schema_id
		  join sys.types as ty
		    on ty.user_type_id = c.user_type_id
		  -- v 1.09 changed to user type to accomodate SQL 2008 CLR data types
		  --join sys.types st
		  --  on ty.system_type_id = st.user_type_id
        where t.name = @TableName AND s.name = @SchemaName 
           AND c.name = @PKColumnName
           AND c.is_computed = 0
           -- version 1.09 modified list of data types
   	       -- v 1.09 changed to ty.name to accomodate SQL 2008 CLR data types
           AND ty.name NOT IN ('text', 'ntext', 'image', 'geography', 'xml', 'binary', 'varbinary', 'timestamp')
	  order by c.column_id


	  
IF @LogInsert = 2 -- log every column to the Audit Trail table 
----------------------------------------------------------------------------------
-- BEGIN FOR EACH COLUMN
----------------------------------------------------------------------------------	  
	select @SQL = @SQL  

	        --    1.10b Cautious that the overuse of IF branching may cause recompiles or bad query execution plans
	        --    +  ' IF UPDATE([' + c.name + '])' + Char(13) + Char(10)     

         + '   INSERT dbo.Audit (AuditDate, SysUser, Application, HostName, TableName, Operation, SQLStatement, PrimaryKey, RowDescription, SecondaryRow, ColumnName, NewValue '
         
         + CASE @BaseTableDDL  
             WHEN 1 THEN ', Rowversion) '
             ELSE ') ' -- gotta figure out a way to increment the RowVersion in the Audit table from the Audit table alone without killing perf !
           END  + Char(13) + Char(10)  

         + '     SELECT '

         -- StrictUserOption
   	     + CASE 
   	         WHEN @StrictUserContext = 1 THEN ' @AuditTime, SUSER_SNAME(),'
   	         ELSE 'COALESCE(Inserted.Modified, @AuditTime), COALESCE(Inserted.Modifiedby, Suser_Sname()),'
           END

         + ' APP_NAME(), Host_Name(), ' 
         + '''' + @SchemaName + '.' + @TableName + ''', ''i'','  

         -- if @LogSQL is off then the @UserSQL variable has not been declared
         + CASE @LogSQL
             WHEN 1 THEN ' @UserSQL, '
             ELSE ' NULL, ' 
           END  
       
       -- 1.09 handle HierarchyID PK  
       + ' Convert(VARCHAR(50), Inserted.[' + @PKColumnName + ']),' + Char(13) + Char(10) 
       ----------------
       + '        NULL,     -- Row Description (e.g. Order Number)' + Char(13) + Char(10)   
       + '        NULL,     -- Secondary Row Value (e.g. Order Number for an Order Detail Line)' + Char(13) + Char(10)
       + '        ''[' + c.name+ ']'',' 
         
       -- 1.09 changed from cast to convert to handle HierarchyID conversion 
       + ' Convert(VARCHAR(50), Inserted.[' + c.name + '])' + Char(13) + Char(10)
    
       + CASE @BaseTableDDL  
           WHEN 1 THEN ', 1'
           ELSE '' -- gotta figure out a way to increment the RowVersion in the Audit table !
         END  + Char(13) + Char(10)  
       
       + '          FROM Inserted' + Char(13) + Char(10)
       --+ '             JOIN Deleted' + Char(13) + Char(10)
       --+ '               ON Inserted.[' + @PKColumnName + '] = Deleted.[' + @PKColumnName + ']' + Char(13) + Char(10)
     --  + '               AND isnull(Inserted.[' + c.name + '],'''') <> isnull(Deleted.[' + c.name + '],'''')' + Char(13) + Char(10) 
              --  AND ISNULL(Inserted.[CBIGINT],'') <> ISNULL(Deleted.[CBIGINT],'')

	--+ CASE ty.name
	--    WHEN 'HierarchyID'  
	--      THEN '               AND ISNULL(Inserted.[' + c.name + '],0x999999) <> ISNULL(Deleted.[' + c.name + '],0x999999)'  + Char(13) + Char(10) 
	--    WHEN 'uniqueidentifier'  
	--      THEN '               AND ISNULL(Inserted.[' + c.name + '],''00000000-0000-0000-0000-000000000000'') <> ISNULL(Deleted.[' + c.name + '],''00000000-0000-0000-0000-000000000000'')'  + Char(13) + Char(10) 
	--    WHEN 'DECIMAL'  
	--      THEN '               AND ISNULL(Inserted.[' + c.name + '],0) <> ISNULL(Deleted.[' + c.name + '],0)'  + Char(13) + Char(10) 
	--    WHEN 'NUMERIC'  
	--      THEN '               AND ISNULL(Inserted.[' + c.name + '],0) <> ISNULL(Deleted.[' + c.name + '],0)'  + Char(13) + Char(10) 
	--    ELSE '               AND ISNULL(Inserted.[' + c.name + '],'''') <> ISNULL(Deleted.[' + c.name + '],'''')'  + Char(13) + Char(10) 
	--  END   
       
       + '   IF @@RowCount > 0 SET @IsDirty = 1' + Char(13) + Char(10)
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)
	  from sys.tables as t
		  join sys.columns as c
		    on t.object_id = c.object_id
		  join sys.schemas as s
		    on s.schema_id = t.schema_id
		  join sys.types as ty
		    on ty.user_type_id = c.user_type_id
	      -- v 1.09 changed to user type to accomodate SQL 2008 CLR data types
		  -- join sys.types st
		  --   on ty.system_type_id = st.user_type_id
        where t.name = @TableName AND s.name = @SchemaName 
           AND c.name NOT IN ('created', 'modified','CreatedBy', 'ModifiedBy','RowVersion')
           AND c.name <> @PKColumnName  -- The PK should not be updatable
           AND c.is_computed = 0
           -- version 1.09 modified list of data types
   	       -- v 1.09 changed to ty.name to accomodate SQL 2008 CLR data types
           AND ty.name NOT IN ('text', 'ntext', 'image',  'geography','xml', 'binary', 'varbinary', 'timestamp')
	  order by c.column_id
	  
----------------------------------------------------------------------------------
-- END FOR EACH COLUMN
----------------------------------------------------------------------------------	  
  
	  
	  
	IF @StrictUserContext = 1 AND @BaseTableDDL = 1
	select @SQL = @SQL 
	
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)
	     + ' -- Update the Created and Modified columns' + Char(13) + Char(10)
       + '   UPDATE [' + @SchemaName + '].[' + @TableName + ']'+ Char(13) + Char(10)
       + '     SET Created  = @AuditTime, ' + Char(13) + Char(10)
       + '         CreatedBy  = Suser_SName(), ' + Char(13) + Char(10)
       + '         Modified = @AuditTime, ' + Char(13) + Char(10)
       + '         ModifiedBy  = Suser_SName(), ' + Char(13) + Char(10)
       + '        [RowVersion] =  1' + Char(13) + Char(10)
       + '     FROM [' + @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
       + '       JOIN Inserted'  + Char(13) + Char(10)
       + '         ON [' + @TableName + '].[' + @PKColumnName + '] = Inserted.[' + @PKColumnName + ']'
       +  Char(13) + Char(10) + Char(13) + Char(10)
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)

	IF @StrictUserContext = 0 
	select @SQL = @SQL 
	
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)
	     + ' -- Update the Created and Modified columns' + Char(13) + Char(10)
       + '   UPDATE [' + @SchemaName + '].[' + @TableName + ']'+ Char(13) + Char(10)
       + '     SET Created  = COALESCE(Inserted.Created, @AuditTime), ' + Char(13) + Char(10)
       + '         CreatedBy  = COALESCE(Inserted.CreatedBy, Suser_SName()), ' + Char(13) + Char(10)
       + '         Modified = COALESCE(Inserted.Modified, Inserted.Created, @AuditTime), ' + Char(13) + Char(10)
       + '         ModifiedBy  = COALESCE(Inserted.ModifiedBy, Inserted.CreatedBy, Suser_SName()), ' + Char(13) + Char(10)
       + '        [RowVersion] =  1' + Char(13) + Char(10)
       + '     FROM [' + @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
       + '       JOIN Inserted'  + Char(13) + Char(10)
       + '         ON [' + @TableName + '].[' + @PKColumnName + '] = Inserted.[' + @PKColumnName + ']'
       +  Char(13) + Char(10) + Char(13) + Char(10)
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)

	select @SQL = @SQL + 
       + ' End Try ' + Char(13) + Char(10)
       + ' Begin Catch ' + Char(13) + Char(10)
       + '   DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;' + Char(13) + Char(10) 

       + '   SET @ErrorMessage = ERROR_MESSAGE();  ' + Char(13) + Char(10)
       + '   SET @ErrorSeverity = ERROR_SEVERITY(); ' + Char(13) + Char(10) 
       + '   SET @ErrorState = ERROR_STATE();  ' + Char(13) + Char(10)
       + '   RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState) with log;' + Char(13) + Char(10) 
--        + '   Raiserror(''error in [' + + @SchemaName + '].[' + @TableName +'_audit_insert] trigger'', 16, 1 ) with log' + Char(13) + Char(10)
       + ' End Catch '
EXEC (@SQL)

SET @SQL = '[' + @SchemaName + '].[' + @TableName + '_Audit_Insert]'

EXEC sp_settriggerorder 
  @triggername= @SQL, 
  @order='Last', 
  @stmttype = 'INSERT';

--------------------------------------------------------------------------------
--------------------------------------------------------------------------------
-- build update trigger 

SET @SQL = 'CREATE TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Update' + ' ON ['+ @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
       + ' AFTER Update' + Char(13) + Char(10) + ' NOT FOR REPLICATION AS' + Char(13) + Char(10)
       + ' SET NoCount On ' + Char(13) + Char(10)
       + ' -- generated by AutoAudit Version ' + @Version + ' on ' + Convert(VARCHAR(30), GetDate(),100)  + Char(13) + Char(10)
       + ' -- created by Paul Nielsen ' + Char(13) + Char(10)
       + ' -- www.SQLServerBible.com ' + Char(13) + Char(10)
       + ' -- autoaudit.codeplex.com ' + Char(13) + Char(10) + Char(13) + Char(10)
       
       + ' -- Options: ' + Char(13) + Char(10)
       + ' --   StrictUserContext : ' + CAST(@StrictUserContext as CHAR(1)) + Char(13) + Char(10)
       + ' --   LogSQL            : ' + CAST(@LogSQL as CHAR(1)) + Char(13) + Char(10)
       + ' --   BaseTableDDL      : ' + CAST(@BaseTableDDL as CHAR(1)) + Char(13) + Char(10) + Char(13) + Char(10)
       + ' --   LogInsert         : ' + CAST(@LogInsert as CHAR(1)) + Char(13) + Char(10) + Char(13) + Char(10)
       
       + 'DECLARE ' + Char(13) + Char(10)
       + '  @AuditTime DATETIME, ' + Char(13) + Char(10)
       + '  @IsDirty BIT' + Char(13) + Char(10)
       
       -- keep the variable initialization separate for SQL Server 2005
       + 'SET @AuditTime = GetDate()' + Char(13) + Char(10) + Char(13) + Char(10)
       + 'SET @IsDirty = 0' + Char(13) + Char(10) + Char(13) + Char(10)

       + '  Begin Try' + Char(13) + Char(10)
           
   	        /* -------------------------------------------------------------------------------
   	        -- enable this code to force a rollback of attempt to set user context when StrictUserContext is on
   	        IF @StrictUserContext = 1
              SELECT @SQL = @SQL
                   -- StrictUserContext so always set to system user function        
                 + '    SET @CreateUserName = SUser_SName()' + Char(13) + Char(10)
                 + '    SET @ModifyUserName = SUser_SName()' + Char(13) + Char(10) + Char(13) + Char(10)
                   -- StrictUserContext so updating audit column not permitted 
                 + '    IF @@NestLevel = 1 AND (UPDATE(Created) OR UPDATE(CreatedBy) OR UPDATE(Modified) OR UPDATE(ModifiedBy) OR UPDATE(RowVersion))' + Char(13) + Char(10)
                 + '      BEGIN ' + Char(13) + Char(10)
                 + '        RAISERROR(''Update of Created, Createdby, Modified, ModifiedBy, or RowVersion not permitted by AutoAudit when StrictUserContext is enabled.'', 16,1)' + Char(13) + Char(10)
                 + '        ROLLBACK' + Char(13) + Char(10)
                 + '      END ' + Char(13) + Char(10)   + Char(13) + Char(10)  
             */ -------------------------------------------------------------------------------
   
   IF @LogSQL = 1 
     BEGIN  
     	 select @SQL = @SQL
         + ' -- capture SQL Statement' + Char(13) + Char(10)
         + ' DECLARE @ExecStr varchar(50), @UserSQL nvarchar(max)' + Char(13) + Char(10)
         + ' DECLARE @inputbuffer TABLE' + Char(13) + Char(10) 
         + ' (EventType nvarchar(30), Parameters int, EventInfo nvarchar(max))' + Char(13) + Char(10)
         + ' SET @ExecStr = ''DBCC INPUTBUFFER(@@SPID) with no_infomsgs''' + Char(13) + Char(10)
         + ' INSERT INTO @inputbuffer' + Char(13) + Char(10) 
         + '   EXEC (@ExecStr)' + Char(13) + Char(10)
         + ' SELECT @UserSQL = EventInfo FROM @inputbuffer' + Char(13) + Char(10)
         + Char(13) + Char(10) 
     END   
     
----------------------------------------------------------------------------------
-- BEGIN FOR EACH COLUMN
----------------------------------------------------------------------------------	  

	select @SQL = @SQL  

	        --    1.10b Cautious that the overuse of IF branching may cause recompiles or bad query execution plans
	        --    +  ' IF UPDATE([' + c.name + '])' + Char(13) + Char(10)     

         + '   INSERT dbo.Audit (AuditDate, SysUser, Application, HostName, TableName, Operation, SQLStatement, PrimaryKey, RowDescription, SecondaryRow, ColumnName, OldValue, NewValue '
         
         + CASE @BaseTableDDL  
             WHEN 1 THEN ', Rowversion) '
             ELSE ') ' -- gotta figure out a way to increment the RowVersion in the Audit table from the Audit table alone without killing perf !
           END  + Char(13) + Char(10)  

         + '     SELECT '

         -- StrictUserOption
   	     + CASE 
   	         WHEN @StrictUserContext = 1 THEN ' @AuditTime, SUSER_SNAME(),'
   	         ELSE 'COALESCE(Inserted.Modified, @AuditTime), COALESCE(Inserted.Modifiedby, Suser_Sname()),'
           END

         + ' APP_NAME(), Host_Name(), ' 
         + '''' + @SchemaName + '.' + @TableName + ''', ''u'','  

         -- if @LogSQL is off then the @UserSQL variable has not been declared
         + CASE @LogSQL
             WHEN 1 THEN ' @UserSQL, '
             ELSE ' NULL, ' 
           END  
       
       -- 1.09 handle HierarchyID PK  
       + ' Convert(VARCHAR(50), Inserted.[' + @PKColumnName + ']),' + Char(13) + Char(10) 
       ----------------
       + '        NULL,     -- Row Description (e.g. Order Number)' + Char(13) + Char(10)   
       + '        NULL,     -- Secondary Row Value (e.g. Order Number for an Order Detail Line)' + Char(13) + Char(10)
       + '        ''[' + c.name+ ']'',' 
         
       -- 1.09 changed from cast to convert to handle HierarchyID conversion 
       + ' Convert(VARCHAR(50), Deleted.[' + c.name + ']), ' 
       + ' Convert(VARCHAR(50), Inserted.[' + c.name + '])' + Char(13) + Char(10)
    
       + CASE @BaseTableDDL  
           WHEN 1 THEN ', deleted.Rowversion + 1'
           ELSE '' -- gotta figure out a way to increment the RowVersion in the Audit table !
         END  + Char(13) + Char(10)  
       
       + '          FROM Inserted' + Char(13) + Char(10)
       + '             JOIN Deleted' + Char(13) + Char(10)
       + '               ON Inserted.[' + @PKColumnName + '] = Deleted.[' + @PKColumnName + ']' + Char(13) + Char(10)
     --  + '               AND isnull(Inserted.[' + c.name + '],'''') <> isnull(Deleted.[' + c.name + '],'''')' + Char(13) + Char(10) 
              --  AND ISNULL(Inserted.[CBIGINT],'') <> ISNULL(Deleted.[CBIGINT],'')

	+ CASE ty.name
	    WHEN 'HierarchyID'  
	      THEN '               AND ISNULL(Inserted.[' + c.name + '],0x999999) <> ISNULL(Deleted.[' + c.name + '],0x999999)'  + Char(13) + Char(10) 
	    WHEN 'uniqueidentifier'  
	      THEN '               AND ISNULL(Inserted.[' + c.name + '],''00000000-0000-0000-0000-000000000000'') <> ISNULL(Deleted.[' + c.name + '],''00000000-0000-0000-0000-000000000000'')'  + Char(13) + Char(10) 
	    WHEN 'DECIMAL'  
	      THEN '               AND ISNULL(Inserted.[' + c.name + '],0) <> ISNULL(Deleted.[' + c.name + '],0)'  + Char(13) + Char(10) 
	    WHEN 'NUMERIC'  
	      THEN '               AND ISNULL(Inserted.[' + c.name + '],0) <> ISNULL(Deleted.[' + c.name + '],0)'  + Char(13) + Char(10) 
	    ELSE '               AND ISNULL(Inserted.[' + c.name + '],'''') <> ISNULL(Deleted.[' + c.name + '],'''')'  + Char(13) + Char(10) 
	  END   
       
       + '   IF @@RowCount > 0 SET @IsDirty = 1' + Char(13) + Char(10)
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)
	  from sys.tables as t
		  join sys.columns as c
		    on t.object_id = c.object_id
		  join sys.schemas as s
		    on s.schema_id = t.schema_id
		  join sys.types as ty
		    on ty.user_type_id = c.user_type_id
	      -- v 1.09 changed to user type to accomodate SQL 2008 CLR data types
		  -- join sys.types st
		  --   on ty.system_type_id = st.user_type_id
        where t.name = @TableName AND s.name = @SchemaName 
           AND c.name NOT IN ('created', 'modified','CreatedBy', 'ModifiedBy','RowVersion')
           AND c.name <> @PKColumnName  -- The PK should not be updatable
           AND c.is_computed = 0
           -- version 1.09 modified list of data types
   	       -- v 1.09 changed to ty.name to accomodate SQL 2008 CLR data types
           AND ty.name NOT IN ('text', 'ntext', 'image',  'geography','xml', 'binary', 'varbinary', 'timestamp')
	  order by c.column_id
	  
----------------------------------------------------------------------------------
-- END FOR EACH COLUMN
----------------------------------------------------------------------------------	  

-- uniqueidentifier ??

  -- Update the created, createdby, modified, modifiedby columns 
	IF @StrictUserContext = 1 AND @BaseTableDDL = 1
	SELECT @SQL = @SQL 
       -- force the created and createdby to stay the same as it was (in the deleted table) 
       -- set the modified and modifiedby to the current @AuditTime and Suser_Sname()
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)
	     + ' -- Update the Created and Modified columns' + Char(13) + Char(10)
 	     + ' IF @IsDirty = 1 AND @@NestLevel = 1' + Char(13) + Char(10)
       + '   UPDATE [' + @SchemaName + '].[' + @TableName + ']'+ Char(13) + Char(10)
       + '     SET Created  = Deleted.Created, ' + Char(13) + Char(10)
       + '         CreatedBy = Deleted.CreatedBy, ' + Char(13) + Char(10)
       + '         Modified = @AuditTime, ' + Char(13) + Char(10)
       + '         ModifiedBy  = SUser_SName(), ' + Char(13) + Char(10)
       + '        [RowVersion] = [' + @TableName + '].[RowVersion] + 1 ' + Char(13) + Char(10)
       + '     FROM [' + @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
       + '       JOIN Inserted'  + Char(13) + Char(10)
       + '         ON [' + @TableName + '].[' + @PKColumnName + '] = Inserted.[' + @PKColumnName + ']' + Char(13) + Char(10)
       + '       JOIN Deleted'  + Char(13) + Char(10)
       + '         ON [' + @TableName + '].[' + @PKColumnName + '] = Deleted.[' + @PKColumnName + ']'  +  Char(13) + Char(10) + Char(13) + Char(10)
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)

	IF @StrictUserContext = 0  -- (For StrictUserContext to be set to 0, AutoAudit requires BaseTableDDL set to 1)
	SELECT @SQL = @SQL 
	     -- allow the DML to set the created, createdby columns, but default to no change
	     -- allow the DML to set the modified, modifiedby column, but default to @audittime and Suser_Sname()
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)
	     + ' -- Update the Created and Modified columns' + Char(13) + Char(10)
 	     + ' IF @@NestLevel = 1' + Char(13) + Char(10)
       + '   UPDATE [' + @SchemaName + '].[' + @TableName + ']'+ Char(13) + Char(10)
    --   + '     SET Created  = COALESCE(Inserted.Created, Deleted.Created), ' + Char(13) + Char(10)
    --   + '         CreatedBy = COALESCE(Inserted.Createdby, Deleted.Createdby), ' + Char(13) + Char(10)
       + '     SET Modified = COALESCE(Inserted.Modified, @AuditTime), ' + Char(13) + Char(10)
       + '         ModifiedBy  = COALESCE(Inserted.Modifiedby, Suser_Sname()) ' + Char(13) + Char(10)
       + '     FROM [' + @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
       + '       JOIN Inserted'  + Char(13) + Char(10)
       + '         ON [' + @TableName + '].[' + @PKColumnName + '] = Inserted.[' + @PKColumnName + ']'
       + '       JOIN Deleted'  + Char(13) + Char(10)
       + '         ON [' + @TableName + '].[' + @PKColumnName + '] = Deleted.[' + @PKColumnName + ']'
       +  Char(13) + Char(10) + Char(13) + Char(10)
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)
       
 	IF @StrictUserContext = 0  -- (For StrictUserContext to be set to 0, AutoAudit requires BaseTableDDL set to 1)
	SELECT @SQL = @SQL 
	     -- only update RowVersion if a user column is dirty (not just modified or modifiedby - for pre-delete touch)
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)
	     + ' -- Update the Created and Modified columns' + Char(13) + Char(10)
 	     + ' IF @IsDirty = 1 AND @@NestLevel = 1' + Char(13) + Char(10)
       + '   UPDATE [' + @SchemaName + '].[' + @TableName + ']'+ Char(13) + Char(10)
       + '     SET  [RowVersion] = [' + @TableName + '].[RowVersion] + 1 ' + Char(13) + Char(10)
       + '     FROM [' + @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
       + '       JOIN Inserted'  + Char(13) + Char(10)
       + '         ON [' + @TableName + '].[' + @PKColumnName + '] = Inserted.[' + @PKColumnName + ']'
       + '       JOIN Deleted'  + Char(13) + Char(10)
       + '         ON [' + @TableName + '].[' + @PKColumnName + '] = Deleted.[' + @PKColumnName + ']'
       +  Char(13) + Char(10) + Char(13) + Char(10)
       + '-----' + Char(13) + Char(10) + Char(13) + Char(10)

	select @SQL = @SQL + 
       + ' End Try ' + Char(13) + Char(10)
       + ' Begin Catch ' + Char(13) + Char(10)
       + '   DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;' + Char(13) + Char(10) 

       + '   SET @ErrorMessage = ERROR_MESSAGE();  ' + Char(13) + Char(10)
       + '   SET @ErrorSeverity = ERROR_SEVERITY(); ' + Char(13) + Char(10) 
       + '   SET @ErrorState = ERROR_STATE();  ' + Char(13) + Char(10)
       + '   RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState) with log;' + Char(13) + Char(10) 
--        + '   Raiserror(''error in [' + + @SchemaName + '].[' + @TableName +'_audit_insert] trigger'', 16, 1 ) with log' + Char(13) + Char(10)
       + ' End Catch ' 

EXEC (@SQL)

SET @SQL = '[' + @SchemaName + '].[' + @TableName + '_Audit_Update]'

EXEC sp_settriggerorder 
  @triggername= @SQL, 
  @order='Last', 
  @stmttype = 'UPDATE';

--------------------------------------------------------------------------------
--------------------------------------------------------------------------------
-- build delete trigger 
SET @SQL = 'CREATE TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Delete' + ' ON ['+ @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
       + ' AFTER Delete' + Char(13) + Char(10) + ' NOT FOR REPLICATION AS' + Char(13) + Char(10)
       + ' SET NoCount On ' + Char(13) + Char(10)
       + ' -- generated by AutoAudit Version ' + @Version + ' on ' + Convert(VARCHAR(30), GetDate(),100)  + Char(13) + Char(10)
       + ' -- created by Paul Nielsen ' + Char(13) + Char(10)
       + ' -- www.SQLServerBible.com ' + Char(13) + Char(10)
       + ' -- autoaudit.codeplex.com ' + Char(13) + Char(10) + Char(13) + Char(10)

       + ' -- Options: ' + Char(13) + Char(10)
       + ' --   StrictUserContext : ' + CAST(@StrictUserContext as CHAR(1)) + Char(13) + Char(10)
       + ' --   LogSQL            : ' + CAST(@LogSQL as CHAR(1)) + Char(13) + Char(10)
       + ' --   BaseTableDDL      : ' + CAST(@BaseTableDDL as CHAR(1)) + Char(13) + Char(10) + Char(13) + Char(10)
       + ' --   LogInsert         : ' + CAST(@LogInsert as CHAR(1)) + Char(13) + Char(10) + Char(13) + Char(10)
      
       + 'DECLARE @AuditTime DATETIME' + Char(13) + Char(10)
       + 'SET @AuditTime = GetDate()' + Char(13) + Char(10) + Char(13) + Char(10)
       
       + ' Begin Try ' + Char(13) + Char(10)
       
   IF @LogSQL = 1 
     BEGIN  
     	 select @SQL = @SQL
         + ' -- capture SQL Statement' + Char(13) + Char(10)
         + ' DECLARE @ExecStr varchar(50), @UserSQL nvarchar(max)' + Char(13) + Char(10)
         + ' DECLARE  @inputbuffer TABLE' + Char(13) + Char(10) 
         + ' (EventType nvarchar(30), Parameters int, EventInfo nvarchar(max))' + Char(13) + Char(10)
         + ' SET @ExecStr = ''DBCC INPUTBUFFER(@@SPID) with no_infomsgs''' + Char(13) + Char(10)
         + ' INSERT INTO @inputbuffer' + Char(13) + Char(10) 
         + '   EXEC (@ExecStr)' + Char(13) + Char(10)
         + ' SELECT @UserSQL = EventInfo FROM @inputbuffer' + Char(13) + Char(10)
         + Char(13) + Char(10) 
     END   

-- for each column
	select @SQL = @SQL + 
		     '   INSERT dbo.Audit (AuditDate, SysUser, Application, HostName, TableName, Operation, SQLStatement, PrimaryKey, RowDescription, SecondaryRow, ColumnName, OldValue, Rowversion)' + Char(13) + Char(10)
       + '   SELECT '
         		      
		      -- StrictUserOption
		      + CASE @StrictUserContext
		          WHEN 0 -- allow DML setting of created/modified user and datetimes
		            THEN ' COALESCE(Deleted.Modified, @AuditTime), COALESCE(Deleted.ModifiedBy, Suser_SName()),'
		          ELSE -- block DML setting of user context 
		             ' @AuditTime, Suser_SName(),'
		        END 
		   
          + ' APP_NAME(), Host_Name(), ' 
          + '''' + @SchemaName + '.' + @TableName + ''', ''d'','
          
          -- if @LogSQL is off then the @UserSQL variable has not been declared
          + CASE @LogSQL
             WHEN 1 THEN ' @UserSQL, '
             ELSE ' NULL, ' 
           END  
       
          + ' deleted.[' + @PKColumnName + '],' + Char(13) + Char(10) 
          + '        NULL,     -- Row Description (e.g. Order Number)' + Char(13) + Char(10)   
          + '        NULL,     -- Secondary Row Value (e.g. Oder Number for an Order Detail Line)' + Char(13) + Char(10)
          + '        ''[' + c.name + ']'','   

          + ' Convert(VARCHAR(50), Deleted.[' + c.name + ']), ' 
  
         + CASE @BaseTableDDL  
             WHEN 1 THEN ' deleted.Rowversion '
             ELSE ' 0'
           END   
             
          + '          FROM deleted' + Char(13) + Char(10)
          + '          WHERE deleted.['+ c.name + '] is not null' + Char(13) + Char(10)+ Char(13) + Char(10)
	  from sys.tables as t
		  join sys.columns as c
		    on t.object_id = c.object_id
		  join sys.schemas as s
		    on s.schema_id = t.schema_id
		  join sys.types as ty
		    on ty.user_type_id = c.user_type_id
          -- v 1.09 changed to ty.name to accomodate SQL 2008 CLR data types
		  --join sys.types st
		  --  on ty.system_type_id = st.user_type_id
        where t.name = @TableName AND s.name = @SchemaName 
           AND c.is_computed = 0
           -- version 1.09 modified list of data types
           -- v 1.09 changed to ty.name to accomodate SQL 2008 CLR data types
           AND ty.name NOT IN ('text', 'ntext', 'image', 'geography','xml', 'binary', 'varbinary', 'timestamp')
	  order by c.column_id

	select @SQL = @SQL + 
       + ' End Try ' + Char(13) + Char(10)
       + ' Begin Catch ' + Char(13) + Char(10)
       + '   DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;' + Char(13) + Char(10) 

       + '   SET @ErrorMessage = ERROR_MESSAGE();  ' + Char(13) + Char(10)
       + '   SET @ErrorSeverity = ERROR_SEVERITY(); ' + Char(13) + Char(10) 
       + '   SET @ErrorState = ERROR_STATE();  ' + Char(13) + Char(10)
       + '   RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState) with log;' + Char(13) + Char(10) 
--        + '   Raiserror(''error in [' + + @SchemaName + '].[' + @TableName +'_audit_insert] trigger'', 16, 1 ) with log' + Char(13) + Char(10)
       + ' End Catch ' 

EXEC (@SQL)

SET @SQL = '[' + @SchemaName + '].[' + @TableName + '_Audit_Delete]'

EXEC sp_settriggerorder 
  @triggername= @SQL, 
  @order='Last', 
  @stmttype = 'DELETE';

--------------------------------------------------------------------------------------------
-- build _Deleted view

/* Sample: 
CREATE VIEW Production.vCulture_Deleted
as
	SELECT 
		Max(Case ColumnName WHEN '[CultureID]' THEN OldValue ELSE '' END) AS [CultureID],
		Max(Case ColumnName WHEN '[Name]' THEN OldValue ELSE '' END) AS [Name],
		Max(Case ColumnName WHEN '[Created]' THEN OldValue ELSE '' END) AS [Created],
		Max(Case ColumnName WHEN '[Modified]' THEN OldValue ELSE '' END) AS Modified,
		Max(Case ColumnName WHEN '[Rowversion]' THEN OldValue ELSE '' END) AS [Rowversion],
        MAX(AuditDate) AS 'Deleted'
	FROM Audit 
	Where TableName = 'Production.Culture' AND Operation = 'd'
	GROUP BY PrimaryKey 
*/

SET @SQL = 'CREATE VIEW ' + @SchemaName + '.v' + @TableName + '_Deleted' + Char(13) + Char(10)
       + 'AS ' + Char(13) + Char(10) 
       + ' -- generated by AutoAudit Version ' + @Version + ' on ' + Convert(VARCHAR(30), GetDate(),100)  + Char(13) + Char(10)
       + ' -- created by Paul Nielsen ' + Char(13) + Char(10)
       + ' -- www.SQLServerBible.com ' + Char(13) + Char(10)
       + ' -- autoaudit.codeplex.com ' + Char(13) + Char(10) + Char(13) + Char(10)

       + 'SELECT ' + Char(13) + Char(10)

-- for each column
SELECT @SQL = @SQL +
		  '     Max(Case ColumnName WHEN ''[' + c.name + ']'' THEN OldValue ELSE '''' END) AS [' + c.name +'],'  + Char(13) + Char(10)
	  from sys.tables as t
		join sys.columns as c
		  on t.object_id = c.object_id
		join sys.schemas as s
		  on s.schema_id = t.schema_id
		join sys.types as ty
		  on ty.user_type_id = c.user_type_id
		--join sys.types st
		--  on ty.system_type_id = st.user_type_id
      where t.name = @TableName AND s.name = @SchemaName 
         AND c.is_computed = 0
         -- version 1.09 modified list of data types
         -- v 1.09 changed to ty.name to accomodate SQL 2008 CLR data types
         AND ty.name NOT IN ('text', 'ntext', 'image',  'geography','xml', 'binary', 'varbinary', 'timestamp')
	  order by c.column_id

SET @SQL = @SQL
        + '      MAX(AuditDate) AS ''Deleted'''  + Char(13) + Char(10)
	    + '  FROM dbo.Audit'   + Char(13) + Char(10)
	    + '  Where TableName = ''' +@SchemaName + '.' + @TableName + ''' AND Operation = ''d'''  + Char(13) + Char(10)
	    + '  GROUP BY PrimaryKey' 

EXEC (@SQL)

--------------------------------------------------------------------------------------------
-- build _RowHistory Table-Valued UDF

/* sample:
CREATE FUNCTION HumanResources.Department_RowHistory (@PK INT) 
RETURNS TABLE
RETURN
(
-- Initial Row Values
Select '1999-01-01' as AuditDate, --Created
  'i' as Operation, 
    -- Name 
    (SELECT Coalesce( 
        (Select top 1 OldValue 
          FROM dbo.Audit 
          Where TableName = 'HumanResources.Department' 
            and PrimaryKey = @PK
            and Operation = 'u' 
            and ColumnName = '[Name]'
          order by AuditID),
      Name)
    FROM HumanResources.Department
      WHERE DepartmentID = @PK
  ) as Name,
  
    -- GroupName 
   ( SELECT Coalesce( 
        (Select top 1 OldValue 
          FROM dbo.Audit 
          Where TableName = 'HumanResources.Department' 
            and PrimaryKey = @PK 
            and Operation = 'u' 
            and ColumnName = '[GroupName]'
          order by AuditID),
      GroupName)
    FROM HumanResources.Department
      WHERE DepartmentID = @PK
  ) as GroupName
  FROM HumanResources.Department
  where DepartmentID = @PK

UNION ALL

SELECT AuditDate, 'u'
     ,Max(Case ColumnName WHEN '[Name]' THEN NewValue ELSE '' END) AS [Name]
     ,Max(Case ColumnName WHEN '[GroupName]' THEN NewValue ELSE '' END) AS [GroupName]
  FROM Audit
  Where TableName = 'HumanResources.Department' 
    AND PrimaryKey = @PK
    and Operation = 'u' 
  GROUP BY PrimaryKey, AuditDate
);

*/

SET @SQL = 'CREATE FUNCTION ' + @SchemaName + '.' + @TableName + '_RowHistory (@PK INT)' + Char(13) + Char(10)
       + 'RETURNS TABLE ' + Char(13) + Char(10) 
       + ' -- generated by AutoAudit Version ' + @Version + ' on ' + Convert(VARCHAR(30), GetDate(),100)  + Char(13) + Char(10)
       + ' -- created by Paul Nielsen ' + Char(13) + Char(10)
       + ' -- www.SQLServerBible.com ' + Char(13) + Char(10)
       + ' -- autoaudit.codeplex.com ' + Char(13) + Char(10) + Char(13) + Char(10)

       + 'RETURN ' + Char(13) + Char(10)
       + '( ' + Char(13) + Char(10)
       + '-- Initial Row Values ' + Char(13) + Char(10)
       + 'Select '
       
       IF @BaseTableDDL = 1 
         SET @SQL = @SQL + ' Created as AuditDate '
       ELSE 
         BEGIN 
           SET @SQL = @SQL
            + '    (Select AuditDate' + Char(13) + Char(10)
            + '       FROM dbo.Audit' + Char(13) + Char(10)
            + '       WHERE TableName = ''' + @SchemaName + '.' + @TableName + '''' + Char(13) + Char(10)
            + '         and PrimaryKey = @PK' + Char(13) + Char(10)
            + '         and Operation = ''i'''  + Char(13) + Char(10)
            + '     ) as AuditDate' + Char(13) + Char(10)  + Char(13) + Char(10)
         END   
       
      SET @SQL = @SQL + ', ''i'' as Operation, 1 as RowVersion' 

-- for each column
SELECT @SQL = @SQL 
      + ', ' + Char(13) + Char(10)
      + '-- ' + c.name + Char(13) + Char(10)
      + '(SELECT Coalesce(' + Char(13) + Char(10)
      + '    (Select top 1 OldValue' + Char(13) + Char(10)
      + '       FROM dbo.Audit' + Char(13) + Char(10)
      + '       WHERE TableName = ''' + @SchemaName + '.' + @TableName + '''' + Char(13) + Char(10)
      + '         and PrimaryKey = @PK' + Char(13) + Char(10)
      + '         and Operation = ''u'''  + Char(13) + Char(10)
      + '         and ColumnName = ''[' + c.name + ']''' + Char(13) + Char(10)
      + '       ORDER BY AuditID),' + Char(13) + Char(10)
      + '     [' + c.name + '])' + Char(13) + Char(10)
      + '  FROM [' + @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
      + '  WHERE [' + @PKColumnName + '] = @PK '  + Char(13) + Char(10)
      + ' ) as [' + c.name + ']' + Char(13) + Char(10)
	  from sys.tables as t
		join sys.columns as c
		  on t.object_id = c.object_id
		join sys.schemas as s
		  on s.schema_id = t.schema_id
		join sys.types as ty
		  on ty.user_type_id = c.user_type_id
		--join sys.types st
		--  on ty.system_type_id = st.user_type_id
      where t.name = @TableName AND s.name = @SchemaName 
         AND c.is_computed = 0
         -- version 1.09 modified list of data types
         -- v 1.09 changed to ty.name to accomodate SQL 2008 CLR data types
         AND ty.name NOT IN ('text', 'ntext', 'image',  'geography','xml', 'binary', 'varbinary', 'timestamp', 'SQL_VARIANT')
         AND c.name NOT IN ('created', 'createdby', 'modified', 'modifiedby', 'RowVersion')
	  order by c.column_id

SELECT @SQL = @SQL 
      + '  FROM [' + @SchemaName + '].[' + @TableName + ']' + Char(13) + Char(10)
      + '    WHERE [' + @PKColumnName +  '] = @PK )' + Char(13) + Char(10) + Char(13) + Char(10)
      
      + 'UNION ALL' + Char(13) + Char(10) + Char(13) + Char(10)
      
 -- Updated values from Audit Trail     
      + 'SELECT AuditDate, ''u'', RowVersion' + Char(13) + Char(10)

-- for each column
SELECT @SQL = @SQL 
      + '     ,Max(Case ColumnName WHEN ''[' + c.name + ']'' THEN NewValue ELSE null END) AS [' + c.name + ']' + Char(13) + Char(10)
	  from sys.tables as t
		join sys.columns as c
		  on t.object_id = c.object_id
		join sys.schemas as s
		  on s.schema_id = t.schema_id
		join sys.types as ty
		  on ty.user_type_id = c.user_type_id
		--join sys.types st
		--  on ty.system_type_id = st.user_type_id
      where t.name = @TableName AND s.name = @SchemaName 
         AND c.is_computed = 0
         -- version 1.09 modified list of data types
         -- v 1.09 changed to ty.name to accomodate SQL 2008 CLR data types
         AND ty.name NOT IN ('text', 'ntext', 'image',  'geography','xml', 'binary', 'varbinary', 'timestamp', 'SQL_VARIANT')
         AND c.name NOT IN ('created', 'createdby', 'modified', 'modifiedby', 'RowVersion')
	  order by c.column_id

SELECT @SQL = @SQL 
      + '  FROM dbo.Audit'  + Char(13) + Char(10)
      + '    Where TableName = '''  +@SchemaName + '.' + @TableName + ''''   + Char(13) + Char(10)
      + '      AND PrimaryKey = @PK' + Char(13) + Char(10)
      + '      and Operation = ''u'''  + Char(13) + Char(10)
      + '    GROUP BY PrimaryKey, AuditDate, RowVersion'  + Char(13) + Char(10)
     -- + ' ORDER BY RowVersion'
     
EXEC (@SQL)

RETURN -- END OF SPROC

go --------------------------------------------------------------------
CREATE PROC pAutoAuditDrop (
   @SchemaName SYSNAME  = 'dbo',
   @TableName SYSNAME
) 
AS 
SET NoCount ON

DECLARE 
   @SQL NVARCHAR(max)

-- drop default constraints

If Exists (select * 
             from sys.objects o 
               join sys.schemas s on o.schema_id = s.schema_id   
             where o.name = @TableName + '_Created_df'
               and s.name = @SchemaName)
  BEGIN 
    SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] drop constraint ' + @TableName + '_Created_df'
    EXEC (@SQL)
  END


If Exists (select * 
             from sys.objects o 
               join sys.schemas s on o.schema_id = s.schema_id   
             where o.name = @TableName + '_Modified_df'
               and s.name = @SchemaName)
  BEGIN 
    SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] drop constraint ' + @TableName + '_Modified_df'
    EXEC (@SQL)
  END

If Exists (select * 
             from sys.objects o 
               join sys.schemas s on o.schema_id = s.schema_id   
             where o.name = @TableName + '_RowVersion_df'
               and s.name = @SchemaName)
  BEGIN 
    SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] drop constraint ' + @TableName + '_RowVersion_df'
    EXEC (@SQL)
  END

-- drop Created column 
IF exists (select *
			  from sys.tables t
				join sys.schemas s
				  on s.schema_id = t.schema_id
				join sys.columns c
				  on t.object_id = c.object_id
			  where  t.name = @TableName AND s.name = @SchemaName and c.name = 'Created')
  BEGIN
    SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] DROP COLUMN Created'
    EXEC (@SQL)
  END

-- drop Modified column 
IF exists( select *
			  from sys.tables t
				join sys.schemas s
				  on s.schema_id = t.schema_id
				join sys.columns c
				  on t.object_id = c.object_id
			  where  t.name = @TableName AND s.name = @SchemaName and c.name = 'Modified')
  BEGIN   
    SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] DROP COLUMN Modified'
    EXEC (@SQL)
  END

-- drop RowVersion column 
IF exists( select *
			  from sys.tables t
				join sys.schemas s
				  on s.schema_id = t.schema_id
				join sys.columns c
				  on t.object_id = c.object_id
			  where  t.name = @TableName AND s.name = @SchemaName and c.name = 'RowVersion')
  BEGIN   
    SET @SQL = 'ALTER TABLE [' + @SchemaName + '].[' + @TableName + '] DROP COLUMN RowVersion'
    EXEC (@SQL)
  END


-- drop existing insert trigger
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''' + @TableName + '_Audit_Insert' + ''' )'
       + ' DROP TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Insert'
EXEC (@SQL)

-- drop existing update trigger
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''' + @TableName + '_Audit_Update' + ''' )'
       + ' DROP TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Update'
EXEC (@SQL)

-- drop existing delete trigger
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''' + @TableName + '_Audit_Delete' + ''' )'
       + ' DROP TRIGGER ' + @SchemaName + '.' + @TableName + '_Audit_Delete'
EXEC (@SQL)

-- drop existing _deleted view
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''v' + @TableName + '_Deleted' + ''' )'
       + ' DROP VIEW ' + @SchemaName + '.v' + @TableName + '_Deleted'
EXEC (@SQL)

-- drop existing _RowHistory UDF
SET @SQL = 'If EXISTS (Select * from sys.objects o join sys.schemas s on o.schema_id = s.schema_id  '
       + ' where s.name = ''' + @SchemaName + ''''
       + '   and o.name = ''' + @TableName + '_RowHistory' + ''' )'
       + ' DROP FUNCTION ' + @SchemaName + '.' + @TableName + '_RowHistory'
EXEC (@SQL)


go -----------------------------------------------------------------------
CREATE 
-- ALTER 
PROC pAutoAuditAll 
AS 
SET NoCount ON 
DECLARE 
   @TableName SYSNAME, 
   @SchemaName SYSNAME, 
   @SQL NVARCHAR(max)
-- for each table
-- 1
DECLARE cTables CURSOR FAST_FORWARD READ_ONLY
  FOR  SELECT s.name, t.name 
			  from sys.tables t
				join sys.schemas s
				  on t.schema_id = s.schema_id
			 where t.name <> 'audit'
--2 
OPEN cTables
--3 
FETCH cTables INTO @SchemaName, @TableName   -- prime the cursor
WHILE @@Fetch_Status = 0 
  BEGIN
		SET @SQL = 'EXEC pAutoAudit ''' + @SchemaName + ''', ''' + @TableName + ''''
		PRINT @SQL
		EXEC (@SQL)
      FETCH cTables INTO @SchemaName, @TableName   -- fetch next
  END
-- 4  
CLOSE cTables
-- 5
DEALLOCATE cTables

RETURN 

go -----------------------------------------------------------------------
CREATE PROC pAutoAuditDropAll 
AS 
SET NoCount ON 
DECLARE 
   @TableName SYSNAME, 
   @SchemaName SYSNAME, 
   @SQL NVARCHAR(max)
   
   
-- remove Schema DDL trigger 
IF Exists(select * from sys.triggers where name = 'SchemaAuditDDLTrigger')
  DROP TRIGGER SchemaAuditDDLTrigger ON Database

-- remove Schema Table
IF Object_id('SchemaAudit') IS NOT NULL
  DROP TABLE dbo.SchemaAudit 

-- remove Audit table
IF Object_id('Audit') IS NOT NULL
  DROP TABLE dbo.Audit

-- for each table
-- 1
DECLARE cTables CURSOR FAST_FORWARD READ_ONLY
  FOR  SELECT s.name, t.name 
			  from sys.tables t
				join sys.schemas s
				  on t.schema_id = s.schema_id
			 where t.name <> 'audit'
--2 
OPEN cTables
--3 
FETCH cTables INTO @SchemaName, @TableName   -- prime the cursor
WHILE @@Fetch_Status = 0 
  BEGIN
		SET @SQL = 'EXEC pAutoAuditDrop ''' + @SchemaName + ''', ''' + @TableName + ''''
		EXEC (@SQL)
      FETCH cTables INTO @SchemaName, @TableName   -- fetch next
  END
-- 4  
CLOSE cTables
-- 5
DEALLOCATE cTables


go --
use tempdb




