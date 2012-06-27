CREATE FUNCTION [dbo].[tousd] 
(@Currency int, @Amount money)
RETURNS money
AS
BEGIN
return (@Amount * (select to_usd_multiplier from Currency where name=@Currency))
end
