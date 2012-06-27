CREATE AGGREGATE [dbo].[SumKeys](@value INT)
    RETURNS NVARCHAR (4000)
    EXTERNAL NAME [SqlServerProjectForDADatabaseR3].[SumKeys];

