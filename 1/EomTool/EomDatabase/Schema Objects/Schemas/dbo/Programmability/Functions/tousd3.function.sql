CREATE FUNCTION [dbo].[tousd3] 
(@Currency int, @Amount money)
RETURNS money
AS
BEGIN
return (@Amount * (select to_usd_multiplier from Currency where id=@Currency))
end
