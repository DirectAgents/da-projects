using System.Linq;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;

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
            var identityGroups = groups.Where(c => c.WindowsIdentity.Split(',').Any(d => d.ToUpper() == windowsIdentity.ToUpper()));
            return identityGroups.FirstOrDefault();
        }
    }
}
