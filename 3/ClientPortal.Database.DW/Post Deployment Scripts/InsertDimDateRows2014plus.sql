WITH DateCTE AS
(
SELECT CAST('2014-01-01' AS DATE) AS DateValue
UNION ALL
SELECT DATEADD(d,1,DateValue)
FROM DateCTE
WHERE DATEADD(d,1,DateValue) < '2018-01-01'
)
INSERT INTO DimDate
SELECT
CONVERT(DATETIME,DateValue) AS PK_Date
, DATENAME(dw, DateValue) +', '+ DATENAME(MONTH,DateValue) +' '+ CONVERT(VARCHAR(2),DATEPART(d,DateValue)) +' '+ CONVERT(CHAR(4),YEAR(DateValue)) AS Date_Name
, CONVERT(DATETIME,CONVERT(CHAR(4),YEAR(DateValue)) +'-01-01') AS Year
, 'Calendar ' + CONVERT(CHAR(4),YEAR(DateValue)) AS Year_Name
, CONVERT(DATETIME,CONVERT(CHAR(4),YEAR(DateValue)) +'-'+ CONVERT(VARCHAR(2),MONTH(DateValue)) +'-1') AS Month
, DATENAME(MONTH,DateValue) +' '+ CONVERT(CHAR(4),YEAR(DateValue)) AS Month_Name
, DATEPART(dy,DateValue) AS Day_Of_Year
, 'Day ' + CONVERT(VARCHAR(3),DATEPART(dy, DateValue)) AS Day_Of_Year_Name
, DATEPART(d,DateValue) AS Day_Of_Month
, 'Day ' + CONVERT(VARCHAR(2),DATEPART(d,DateValue)) AS Day_Of_Month_Name
, MONTH(DateValue) AS Month_Of_Year
, 'Month ' + CONVERT(VARCHAR(2),MONTH(DateValue)) AS Month_Of_Year_Name

FROM DateCTE a
ORDER BY PK_Date
OPTION (MAXRECURSION 5000)
GO