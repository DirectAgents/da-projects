
CREATE FUNCTION [dbo].[ListAnalysts]
(@pid int, @affid int)
RETURNS [varchar](4000)
AS 
BEGIN

DECLARE @analystString varchar(4000)

SELECT @analystString = COALESCE(@analystString + ', ', '') + first_name + ' ' + last_name
FROM AnalystRole ar
JOIN Person p ON ar.person_id = p.id
WHERE ar.pid = @pid AND ar.affid = @affid
ORDER BY first_name,last_name

RETURN ISNULL(@analystString,'')

END