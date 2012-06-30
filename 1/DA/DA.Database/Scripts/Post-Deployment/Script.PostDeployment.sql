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
DELETE FROM [dbo].[EomDatabases]
INSERT [dbo].[EomDatabases] ([Id], [Name], [ConnectionString], [FriendlyName], [StartDate]) 
VALUES (
 1, 
 N'DADatabase_aaron', N'Data Source=biz2\da;Initial Catalog=DADatabase_aaron;Integrated Security=True', 
 N'Aaron 6/12', 
 '6/1/2012'
)