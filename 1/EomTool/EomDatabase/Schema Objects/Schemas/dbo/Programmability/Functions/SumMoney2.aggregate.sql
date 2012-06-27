CREATE AGGREGATE [dbo].[SumMoney2](@value MONEY)
    RETURNS NVARCHAR (4000)
    EXTERNAL NAME [SqlServerProjectForDADatabaseR3].[Aggregate1];

