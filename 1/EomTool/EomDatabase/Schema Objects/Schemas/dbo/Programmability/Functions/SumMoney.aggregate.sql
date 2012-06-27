CREATE AGGREGATE [dbo].[SumMoney](@value MONEY)
    RETURNS MONEY
    EXTERNAL NAME [DADatabaseSqlServer].[Aggregate1];

