using System.Collections.Generic;
using System.Linq;
using DAgents.Common;

namespace EomApp1.Security
{
    public class UserBase
    {
        string _IpAddress;
        string IpAddress
        {
            get
            {
                if (_IpAddress == null)
                {
                    _IpAddress = WindowsIdentityHelper.GetIpAddress();
                }
                return _IpAddress;
            }
        }

        List<Role> _Roles;
        List<Role> Roles
        {
            get
            {
                if (_Roles == null)
                    using (var db = new Security.EomToolSecurityEntities())
                    {
                        _Roles = (from g in db.Groups.ToList()
                                  where WindowsIdentityHelper.DoesCurrentUserHaveIdentity(g.WindowsIdentity.ToArray(','))
                                  from r in g.Roles
                                  select r).ToList();
                        if (_Roles.Count == 0 && !string.IsNullOrWhiteSpace(IpAddress))
                        {
                            _Roles = (from g in db.Groups.ToList()
                                      where g.IpAddress == IpAddress
                                      from r in g.Roles
                                      select r).ToList();
                        }
                    }
                return _Roles;
            }
        }

        List<string> _RoleNames;
        List<string> RoleNames
        {
            get
            {
                if (_RoleNames == null)
                    _RoleNames = Roles.Select(r => r.Name).ToList();
                return _RoleNames;
            }
        }

        List<int> _RoleIds;
        List<int> RoleIds
        {
            get
            {
                if (_RoleIds == null)
                    _RoleIds = Roles.Select(r => r.Id).ToList();
                return _RoleIds;
            }
        }

        List<string> _PermissionTags;
        List<string> PermissionTags
        {
            get
            {
                if (_PermissionTags == null)
                    using (var db = new Security.EomToolSecurityEntities())
                        _PermissionTags = (from r in db.Roles
                                           where RoleIds.Contains(r.Id)
                                           from p in r.Permissions
                                           select p.Tag).ToList();
                return _PermissionTags;
            }
        }

        protected bool HasPermission(string permissionTag)
        {
            return PermissionTags.Contains(permissionTag);
        }
    }
}
