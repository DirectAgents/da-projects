using System.Linq;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using System.Security.Principal;
using System.Collections.Generic;

namespace EomTool.Domain.Concrete
{
    public class SecurityRepository : ISecurityRepository
    {
        EomToolSecurityEntities db;

        public SecurityRepository()
        {
            db = new EomToolSecurityEntities();
        }

        public Group WindowsIdentityGroup(string windowsIdentity)
        {
            var groups = db.Groups.ToList();
            var identityGroups = groups.Where(g => g.WindowsIdentity.Split(',').Any(wi => wi.ToUpper() == windowsIdentity.ToUpper()));
            return identityGroups.FirstOrDefault();
        }

        public IEnumerable<Group> GroupsForUser(IPrincipal user)
        {
            var allGroups = db.Groups.ToList();
            var groups = allGroups.Where(g => g.WindowsIdentity.Split(',').Any(wi => user.IsInRole(wi)));
            return groups;
        }
    }
}
