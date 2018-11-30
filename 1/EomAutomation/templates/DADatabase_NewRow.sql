USE [{%COMMON_DATABASE_NAME%}]
GO

INSERT [dbo].[DADatabase] (
	[name], 
	[connection_string], 
	[effective_date], 
	[am_view_name], 
	[initialized]
) 
VALUES (
	N'{%NAME%}', 
	N'{%CONNECTION_STRING%}', 
	CAST('{%EFFECTIVE_DATE%}' AS DateTime), 
	N'AccountManagerView', 
	1
)
GO
