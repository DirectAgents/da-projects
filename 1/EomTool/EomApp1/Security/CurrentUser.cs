using System.Collections.Generic;
using System.Linq;
using DAgents.Common;

namespace EomApp1.Security
{
    public static class CurrentUser
    {
        private static List<Role> _Roles;
        public static List<Role> Roles
        {
            get
            {
                if (_Roles == null)
                    using (var db = new Security.EomToolSecurityEntities())
                        _Roles = (from g in db.Groups.ToList()
                                  where WindowsIdentityHelper.DoesCurrentUserHaveIdentity(g.WindowsIdentity)
                                  from r in g.Roles
                                  select r).ToList();
                return _Roles;
            }
        }
        private static List<string> _RoleNames;
        public static List<string> RoleNames
        {
            get
            {
                if (_RoleNames == null)
                    _RoleNames = Roles.Select(r => r.Name).ToList();
                return _RoleNames;
            }
        }
        private static List<int> _RoleIds;
        public static List<int> RoleIds
        {
            get
            {
                if (_RoleIds == null)
                    _RoleIds = Roles.Select(r => r.Id).ToList();
                return _RoleIds;
            }
        }
        private static List<string> _PermissionTags;
        public static List<string> PermissionTags
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
        public static bool CanFinalize(string accountManagerName)
        {
            return RoleNames.Contains("AM: " + accountManagerName);
        }
        public static bool CanVerify
        {
            get
            {
                return PermissionTags.Contains("Workflow.Verify");
            }
        }
    }
}
