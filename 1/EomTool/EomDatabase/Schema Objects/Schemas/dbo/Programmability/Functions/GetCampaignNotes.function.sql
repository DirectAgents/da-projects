CREATE FUNCTION [dbo].[GetCampaignNotes]
(@pid INT)
RETURNS NVARCHAR (4000)
AS
 EXTERNAL NAME [SqlServerProject1].[UserDefinedFunctions].[GetCampaignNotes]


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFile', @value = N'GetCampaignNotes.cs', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'GetCampaignNotes';


GO
EXECUTE sp_addextendedproperty @name = N'SqlAssemblyFileLine', @value = N'14', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'FUNCTION', @level1name = N'GetCampaignNotes';

