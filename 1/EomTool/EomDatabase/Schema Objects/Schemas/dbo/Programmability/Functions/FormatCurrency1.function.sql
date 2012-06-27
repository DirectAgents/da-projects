CREATE FUNCTION [dbo].[FormatCurrency1]
(@amount MONEY)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [DADatabaseSqlServer].[UserDefinedFunctions].[FormatCurrency1]

