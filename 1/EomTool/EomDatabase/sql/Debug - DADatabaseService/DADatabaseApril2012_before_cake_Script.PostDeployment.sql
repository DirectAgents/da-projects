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
