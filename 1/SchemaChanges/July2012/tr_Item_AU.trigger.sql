USE [DADatabaseJune2012]
GO

/****** Object:  Trigger [dbo].[tr_Item_AU]    Script Date: 07/17/2012 14:26:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Aaron Anodide
-- Create date: 7/17/2012
-- Description:	
-- =============================================
CREATE TRIGGER [dbo].[tr_Item_AU] 
   ON  [dbo].[Item] 
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	insert into Item_changes (
	   [id]
      ,[name]
      ,[d_name]
      ,[pid]
      ,[d_pid]
      ,[affid]
      ,[d_affid]
      ,[source_id]
      ,[d_source_id]
      ,[unit_type_id]
      ,[d_unit_type_id]
      ,[stat_id_n]
      ,[d_stat_id_n]
      ,[revenue_currency_id]
      ,[d_revenue_currency_id]
      ,[cost_currency_id]
      ,[d_cost_currency_id]
      ,[revenue_per_unit]
      ,[d_revenue_per_unit]
      ,[cost_per_unit]
      ,[d_cost_per_unit]
      ,[num_units]
      ,[d_num_units]
      ,[notes]
      ,[d_notes]
      ,[accounting_notes]
      ,[d_accounting_notes]
      ,[item_accounting_status_id]
      ,[d_item_accounting_status_id]
      ,[item_reporting_status_id]
      ,[d_item_reporting_status_id]
	)
	select 
	   deleted.[id]
	   
      ,deleted.[name]
      ,inserted.[name]
      
      ,deleted.[pid]
      ,inserted.[pid]
      
      ,deleted.[affid]
      ,inserted.[affid]
      
      ,deleted.[source_id]
      ,inserted.[source_id]
      
      ,deleted.[unit_type_id]
      ,inserted.[unit_type_id]
      
      ,deleted.[stat_id_n]
      ,inserted.[stat_id_n]
      
      ,deleted.[revenue_currency_id]
      ,inserted.[revenue_currency_id]
      
      ,deleted.[cost_currency_id]
      ,inserted.[cost_currency_id]
      
      ,deleted.[revenue_per_unit]
      ,inserted.[revenue_per_unit]
      
      ,deleted.[cost_per_unit]
      ,inserted.[cost_per_unit]
      
      ,deleted.[num_units]
      ,inserted.[num_units]
      
      ,deleted.[notes]
      ,inserted.[notes]
      
      ,deleted.[accounting_notes]
      ,inserted.[accounting_notes]
      
      ,deleted.[item_accounting_status_id]
      ,inserted.[item_accounting_status_id]
      
      ,deleted.[item_reporting_status_id]
      ,inserted.[item_reporting_status_id]
      
	from deleted inner join inserted on deleted.id = inserted.id
END

GO

