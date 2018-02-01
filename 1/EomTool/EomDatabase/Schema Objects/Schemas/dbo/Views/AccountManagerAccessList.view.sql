CREATE VIEW [dbo].[AccountManagerAccessList]
AS
select substring(P.Tag,19,100) as Name
from [$(EomToolSecurity)].[dbo].Groups G
join [$(EomToolSecurity)].[dbo].RoleGroup RG ON G.Id=RG.Groups_Id
join [$(EomToolSecurity)].[dbo].RolePermission RP ON RG.Roles_Id=RP.Roles_Id
join [$(EomToolSecurity)].[dbo].Permissions P ON RP.Permissions_Id=P.Id
where WindowsIdentity like '%' + SUSER_NAME() + '%' and Tag like 'Workflow.Finalize%'
