/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

----insert AdManager rows
--SET IDENTITY_INSERT [$(DatabaseName)].[dbo].[AdManager] ON
--INSERT 
--	[$(DatabaseName)].[dbo].[AdManager] (
--		[id], 
--		[name]
--	) 
--SELECT 
--	*
--FROM 
--	DADatabaseFeb2012.dbo.[AdManager]
--SET IDENTITY_INSERT [$(DatabaseName)].[dbo].[AdManager] OFF
----insert AccountManager rows
--SET IDENTITY_INSERT [$(DatabaseName)].[dbo].[AccountManager] ON
--INSERT 
--	[$(DatabaseName)].[dbo].[AccountManager] (
--		[id], 
--		[name]
--	) 
--SELECT 
--	*
--FROM 
--	DADatabaseFeb2012.dbo.[AccountManager]
--SET IDENTITY_INSERT [$(DatabaseName)].[dbo].[AccountManager] OFF

INSERT INTO [dbo].[InvoiceStatus] VALUES
	 (0, 'default')
	,(1, 'am_review')
	,(2, 'acct_review')
	,(3, 'generated')
GO

INSERT INTO [dbo].[MediaBuyerApprovalStatus] VALUES 
	 ('1', 'default')
	,('2', 'Queued')
	,('3', 'Sent')
	,('4', 'Approved')
	,('5', 'Held')
GO

insert [TrackingSystem] values 
  (1, 'Direct Track'),
  (2, 'Cake Marketing')
go

INSERT INTO [dbo].[PaymentBatchState] VALUES 
	 ('1', 'default')
	,('3', 'Sent')
	,('6', 'Complete')
GO

GRANT EXECUTE ON [dbo].[tousd3] TO [DIRECTAGENTS\Account Managers]
GO
GRANT CONNECT TO [DIRECTAGENTS\Account Managers]
GO
ALTER USER [DIRECTAGENTS\Account Managers] WITH DEFAULT_SCHEMA=[dbo]
GO

GRANT SELECT ON [dbo].[MediaBuyerConfirmation_Megan] TO [DIRECTAGENTS\MConahan]
GO
GRANT SELECT ON [dbo].[MediaBuyerConfirmation_MarkG] TO [DIRECTAGENTS\MGlauberson]
GO

/*
ALTER TABLE dbo.Item ADD
	payment_batch_id int NULL
GO
ALTER TABLE dbo.Item ADD CONSTRAINT
	FK_Item_PaymentBatch FOREIGN KEY
	(
	payment_batch_id
	) REFERENCES dbo.PaymentBatch
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
GO

//TODO: Add first and second batch approvers to DAMain1 Settings table
//      + thresholds
*/
